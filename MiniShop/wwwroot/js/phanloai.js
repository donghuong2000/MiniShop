// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/PhanLoai/GetAll"
        },
        "columns": [
            { "data": "MALOAIMH" },
            { "data": "TENLOAIMH" },
            {
                "data": "MALOAIMH",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a data-toggle="modal" data-target="#EditModal" data-whatever="${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onClick=Delete("/PhanLoai/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>                           
                            
                           `;
                }
  
            }
             
        ]

    });
});

const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function Delete(url) {
    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                        $('#dataTable').DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            'Can not delete this, maybe it not exit or error from sever',
                            'error'
                        )
                    }
                }

            })

        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your record is safe :)',
                'error'
            )
        }
    })
}



$('#EditModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idname = button.data('whatever') // Extract info from data-* attributes
    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/phanloai/get/' + idname,
        success: function (data) {
            modal.find('#ma_phan_loai').val(data.data[0].MALOAIMH)
            modal.find('#ma_phan_loai_old').val(data.data[0].MALOAIMH)
            modal.find('#ten_phan_loai').val(data.data[0].TENLOAIMH)
        }
    })
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
    
    
})

$('#updateform').click(function () {
    var oldId = $('#ma_phan_loai_old').val();
    var newId = $('#ma_phan_loai').val();
    var newValue = $('#ten_phan_loai').val();
    $.ajax({
        method: 'POST',
        data: {oldId:oldId,newId:newId,newValue:newValue},
        url: '/phanloai/update/',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#dataTable').DataTable().ajax.reload();
            }
            else {

                toastr.error(data.message);
            }
        }
    })

}
)