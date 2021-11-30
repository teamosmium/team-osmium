var dataTable;

$(document).ready(function () {
    loadDataTable();

});

function loadDataTable() {
    dataTable = $('#tblData2').DataTable({
        "ajax":
        {
            "url": "/Admin/Categories/GetAll"

        },
        "columns": [
            { "data": "categoryName", "width": "60%" },
            {
                "data": "categoryId",
                "render": function (data) {
                    return ` <div class="text-center">
                                <a href="/Admin/Categories/InsertOrUpdate/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>

                                <a  class="btn btn-danger text-white" style="cursor:pointer" onclick =Delete("/Admin/Categories/Delete/${data}")>
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>
                                    `;
                }, "width": "40%"
            }
        ]
    });

}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete",
        text: "You will not be able to restore the data ! ",
        icon: "warning",
        buttons: true,
        dangerMode: true


    }).then((willDeleate) => {
        if (willDeleate) {
            $.ajax({
                type: "DELETE",
                url: url,
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

    })
    {


    };
}