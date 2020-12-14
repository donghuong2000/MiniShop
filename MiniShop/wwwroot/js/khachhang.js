// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/khachhang/GetAll"
        },
        "columns": [
            { "data": "MAKH" },
            { "data": "TENKH" },
            { "data": "TUOI" },
            { "data": "GIOITINH" },
            { "data": "CMND" },
            { "data": "SDT" },
            { "data": "DIACHI" },
            { "data": "LOAI_KHACH_HANG.0.TENLOAI" },

            {
                "data": "MAKH",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a data-toggle="modal" data-target="#EditModal" data-whatever="${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onClick=Delete("/khachhang/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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
        url: '/khachhang/get/' + idname,
        success: function (data) {
            modal.find('#ma').val(data.data[0].MAKH)
            modal.find('#tkh').val(data.data[0].TENKH)
            modal.find('#ns').val(data.data[0].NGAYSINH)
            modal.find('#cmnd').val(data.data[0].CMND)
            modal.find('#sdt').val(data.data[0].SDT)
            modal.find('#gt').val(data.data[0].GIOITINH)
            modal.find('#dc').val(data.data[0].DIACHI)
            modal.find('#lkh').val(data.data[0].LOAIKH)
        }
    })
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
    
    
})

$('#updateform').click(function () {
    var a = $('#tkh').val()
    var b = $('#ns').val()
    var c = $('#cmnd').val()
    var d = $('#sdt').val()
    var e = $('#gt').val()
    var f = $('#dc').val()
    var g = $('#lkh').val()
    var h = $('#ma').val()
  
    $.ajax({
        method: 'POST',
        data: {a:a,b:b,c:c,d:d,e:e,f:f,g:g,h:h},
        url: '/khachhang/update/',
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