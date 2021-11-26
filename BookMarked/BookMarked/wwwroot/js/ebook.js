var dataTable1;

$(document).ready(function () {
    loadDataTable1();
});


function loadDataTable1() {
    dataTable1 = $('#tblData1').DataTable({
        "ajax": {
            "url": "Admin/EBook/GetAllBooks"
        },
        "columns": [
            { "data1": "title", "width": "15%" },
            { "data1": "price", "width": "15%" },
            { "data1": "author", "width": "15%" },
            { "data1": "category.categoryName", "width": "15%" },
            {
                "data1": "eBookId",
                "render": function (data1) {
                    return `
                            <div class="text-center">
                                <a href="Admin/EBook/AddNewBook/${data1}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("Admin/EBook/Delete/${data1}") class="btn btn-danger text-white" style="cursor:pointer">
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
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data1) {
                    if (data1.success) {
                        toastr.success(data1.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data1.message);
                    }
                }
            });
        }
    });
}