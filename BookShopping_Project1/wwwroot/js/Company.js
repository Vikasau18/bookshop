var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streatAddress", "width": "15%" },
            { "data": "city", "width": "15%"},
            { "data": "state", "width": "15%" },
            { "data": "postalCode", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "isAuthorizedCompany",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" disabled checked />`;
                    }
                    else {
                        return `<input type="checkbox" disabled />`;
                    }
                }
            },
            {
                "data":"id",
                "render":function (data) {
                    return `
                        <div class="text-center">
                        <a class="btn btn-info" href="/Admin/Company/Upsert/${data}">
                        <i class="fas fa-edit"></i>
                        </a>
                        <a class="btn btn-danger" onclick=Delete('/Admin/Company/Delete/${data}')>
                        <i class="fas fa-trash-alt"></i>
                        </a>
                        </div>
                    `;
                }
            }
        ]
    })
}
function Delete(url) {
    swal({
        title: "Want to delete data ?",
        buttons: true,
        icon: "Warning",
        text: "Delete Information !!!",
        dangerModel: true
    }).then((willdelete) => {
        if (willdelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        DataTable.api().ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }

                }

            })
        }
    })
}