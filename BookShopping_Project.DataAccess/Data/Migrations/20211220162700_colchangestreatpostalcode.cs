using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShopping_Project.DataAccess.Migrations
{
    public partial class colchangestreatpostalcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Postalcode",
                table: "OrderHeaders",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "StreatAddress",
                table: "AspNetUsers",
                newName: "StreetAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "OrderHeaders",
                newName: "Postalcode");

            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "AspNetUsers",
                newName: "StreatAddress");
        }
    }
}
