var dataTable;

$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll",
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "email", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "company.name", "width": "10%" },
            { "data": "role", "width": "30%" },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                                   <div class="text-center">
                                   <a onclick=LockUnLock('${data.id}') class="btn btn-danger text-white">
                                   <i class="fas fa-unlock"></i>&nbspUnLock
                                   </a>
                                </div>
                        `;
                    }
                    else {
                        return `
                                   <div class="text-center">
                                   <a onclick=LockUnLock('${data.id}') class="btn btn-success text-white">
                                   <i class="fas fa-lock"></i>&nbspLock
                                   </a>
                                </div>
                        `;
                    }
                }
            }
        ]
    })
}
function LockUnLock(id) {
    $.ajax({
        type: "Post",
        url: "/Admin/User/LockUnLock",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
}