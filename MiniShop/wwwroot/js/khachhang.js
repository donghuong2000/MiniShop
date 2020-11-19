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
                "data": "MANV",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a data-toggle="modal" data-target="#EditModal" data-whatever="${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onClick=Delete("/nhanvien/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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
           
            modal.find('#ma_nhan_vien').val(data.data[0].MANV)
            modal.find('#ma_nhan_vien_old').val(data.data[0].MANV)
            modal.find('#ten_nhan_vien').val(data.data[0].TENNV)
            modal.find('#ngay_sinh').val(data.data[0].NGAYSINH)
            modal.find('#gioi_tinh').val(data.data[0].GIOITINH)
            modal.find('#cmnd').val(data.data[0].CMND)
            modal.find('#sdt').val(data.data[0].SDT)
            modal.find('#chuc_vu').val(data.data[0].CHUCVU)
            modal.find('#ngay_lam_viec').val(data.data[0].NGAYLAMVIEC)
            modal.find('#dia_chi').val(data.data[0].DIACHI)
            modal.find('#user_name').val(data.data[0].USERNAME)
        }
    })
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
    
    
})

$('#updateform').click(function () {
    var a = $('#ma_nhan_vien').val()
    var b = $('#ma_nhan_vien_old').val()
    var c = $('#ten_nhan_vien').val()
    var d = $('#ngay_sinh').val()
    var e = $('#gioi_tinh').val()
    var f = $('#cmnd').val()
    var g = $('#sdt').val()
    var h = $('#chuc_vu').val()
    var k = $('#dia_chi').val()
    var l = $('#user_name').val()
    $.ajax({
        method: 'POST',
        data: {a:a,b:b,c:c,d:d,e:e,f:f,g:g,h:h,k:k,l:l},
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