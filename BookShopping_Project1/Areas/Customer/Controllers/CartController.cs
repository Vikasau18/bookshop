using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Models.ViewModels;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShopping_Project1.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;

        }
        [BindProperty]
        public ShoppingCartVm ShoppingCartVm { get; set; }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)(User.Identity);
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                ShoppingCartVm = new ShoppingCartVm()
                {
                    ListCart = new List<ShoppingCart>()
                };
                return View(ShoppingCartVm);
            }

            ShoppingCartVm = new ShoppingCartVm()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value, includeproperties: "Product")

            };
            ShoppingCartVm.OrderHeader.OrderTotal = 0;
            ShoppingCartVm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value, includeproperties: "Company");
            foreach (var list in ShoppingCartVm.ListCart)
            {
                list.Price = SD.GetPriceBasedonQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                ShoppingCartVm.OrderHeader.OrderTotal += (list.Price * list.Count);
                list.Product.Discription = SD.ConvertToRawHtml(list.Product.Discription);
                if (list.Product.Discription.Length > 100)
                {
                    list.Product.Discription = list.Product.Discription.Substring(0, 99) + "...";
                }

            }
            return View(ShoppingCartVm);
        }

        public IActionResult Plus(int cartid)
        {
            var cart = _unitOfWork.ShoppingCart.FirstOrDefault(sc => sc.Id == cartid, includeproperties: "Product");
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartid)
        {
            var cart = _unitOfWork.ShoppingCart.FirstOrDefault(sc => sc.Id == cartid, includeproperties: "Product");
            cart.Count -= 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartid)
        {
            var cart = _unitOfWork.ShoppingCart.FirstOrDefault(sc => sc.Id == cartid, includeproperties: "Product");
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVm = new ShoppingCartVm()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value,
                includeproperties: "Product")
            };

            ShoppingCartVm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value, includeproperties: "Company");
            foreach (var list in ShoppingCartVm.ListCart)
            {
                list.Price = SD.GetPriceBasedonQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                ShoppingCartVm.OrderHeader.OrderTotal += (list.Price * list.Count);
                list.Product.Discription = SD.ConvertToRawHtml(list.Product.Discription);
            }
            ShoppingCartVm.OrderHeader.Name = ShoppingCartVm.OrderHeader.ApplicationUser.Name;
            ShoppingCartVm.OrderHeader.PhoneNumber = ShoppingCartVm.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVm.OrderHeader.StreetAddress = ShoppingCartVm.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVm.OrderHeader.City = ShoppingCartVm.OrderHeader.ApplicationUser.City;
            ShoppingCartVm.OrderHeader.State = ShoppingCartVm.OrderHeader.ApplicationUser.State;
            ShoppingCartVm.OrderHeader.PostalCode = ShoppingCartVm.OrderHeader.ApplicationUser.PostalCode;

            return View(ShoppingCartVm);
        }

        //when placeorderbutton clicked info will be saved in status pending
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(string stripeToken)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value, includeproperties: "Company");

            ShoppingCartVm.ListCart = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value, includeproperties: "Product");
            ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVm.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVm.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVm.OrderHeader.ApplicationUserId = claim.Value;

            _unitOfWork.OrderHeader.Add(ShoppingCartVm.OrderHeader);
            _unitOfWork.Save();

            //save detail in order detail

            foreach (var item in ShoppingCartVm.ListCart)
            {
                item.Price = SD.GetPriceBasedonQuantity(item.Count, item.Product.Price, item.Product.Price50, item.Product.Price100);

                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderId = ShoppingCartVm.OrderHeader.Id,
                    Price = item.Price,
                    Count = item.Count
                };
                ShoppingCartVm.OrderHeader.OrderTotal += orderDetails.Price * orderDetails.Count;
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Save();
            }

            //delete shopping cart after order placed

            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVm.ListCart);
            _unitOfWork.Save();

            HttpContext.Session.SetInt32(SD.Ss_session, 0);
            #region stripepayment
            if (stripeToken == null)
            {
                ShoppingCartVm.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusDelayPayment;
                ShoppingCartVm.OrderHeader.OrderStatus = SD.PaymentStatusApproved;
            }
            else
            {

                //payment process/form
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(ShoppingCartVm.OrderHeader.OrderTotal),
                    Currency = "inr",
                    Description = "Order Id:" + ShoppingCartVm.OrderHeader.Id,
                Source=stripeToken
                };

                //payment/for chanrgeservices
                var service = new ChargeService();
                Charge charge = service.Create(options);//how much to be charged
                if (charge.BalanceTransactionId == null)
                    ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                else
                    ShoppingCartVm.OrderHeader.TransactionId = charge.BalanceTransactionId;
                if (charge.Status.ToLower() == "succeeded")
                {
                    ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    ShoppingCartVm.OrderHeader.OrderStatus = SD.StatusApproved;
                    ShoppingCartVm.OrderHeader.PaymentDate = DateTime.Now;
                }
                    
             }
            _unitOfWork.Save();

            #endregion
            return RedirectToAction("OrderConfirmation", "Cart", new { Id = ShoppingCartVm.OrderHeader.Id });


        }
        public IActionResult OrderConfirmation(int Id)
        {
            return View(Id);
        }

        public IActionResult ConfirmEmail()
        {

            return View();
        }
    }
}
