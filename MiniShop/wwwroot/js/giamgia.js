// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Giamgia/GetAll"
        },
        "columns": [
            { "data": "MAGG" },
            { "data": "TEN" },
            { "data": "NGAYBD" },
            { "data": "NGAYKT" },
            { "data": "PT_GIAM" },
            { "data": "TIENGIAM" },
            { "data": "CONLAI" },
            {
                "data": "MAGG",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a data-toggle="modal" data-target="#EditModal" data-whatever="${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onClick=Delete("/giamgia/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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
        url: '/chucvu/get/' + idname,
        success: function (data) {
            modal.find('#ma_chuc_vu').val(data.data[0].MACHUCVU)
            modal.find('#ma_chuc_vu_old').val(data.data[0].MACHUCVU)
            modal.find('#ten_chuc_vu').val(data.data[0].TENCHUCVU)
            modal.find('#luong').val(data.data[0].LUONG)
        }
    })
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
    
    
})

$('#updateform').click(function () {
    var oldId = $('#ma_chuc_vu_old').val();
    var newId = $('#ma_chuc_vu').val();
    var newnameValue = $('#ten_chuc_vu').val();
    var newluongValue = $('#luong').val();
    $.ajax({
        method: 'POST',
        data: {oldId:oldId,newId:newId,newValue:newnameValue,luong:newluongValue},
        url: '/giamgia/update/',
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

// show selection của các cách áp dụng mã giảm giá
$('#ggsp').click(function () {
    console.log('aa');
})