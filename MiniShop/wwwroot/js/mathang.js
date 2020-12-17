// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/MatHang/GetAll"
        },
        "columns": [
            { "data": "MAMH" },
            { "data": "TENMH" },
            { "data": "NGAYSX" },
            { "data": "HANSD" },
            { "data": "GIA" },
            { "data": "PHAN_LOAI.0.TENLOAIMH" },
            {
                "data": "MAMH",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a data-toggle="modal" data-target="#EditModal" data-whatever="${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onClick=Delete("/MatHang/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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
                            data.message,
                            'success'
                        );
                        $('#dataTable').DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            data.message,
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
        url: '/mathang/get/' + idname,
        success: function (data) {
           
            modal.find('#aa').val(data.data[0].MAMH)
            modal.find('#a').val(data.data[0].MAMH)
            modal.find('#b').val(data.data[0].TENMH)
            modal.find('#c').val(data.data[0].NGAYSX)
            modal.find('#d').val(data.data[0].HANSD)
            modal.find('#f').val(data.data[0].GIA)
            modal.find('#e').val(data.data[0].LOAI)
           
           
        }
    })
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
    
    
})

$('#updateform').click(function () {
    var a = $('#a').val()
    var aa = $('#aa').val()
    var b = $('#b').val()
    var c = $('#d').val()
    var d = $('#d').val()
    var e = $('#e').val()
    var f = $('#f').val()
   
    $.ajax({
        method: 'POST',
        data: {a:a,b:b,c:c,d:d,e:e,f:f,aa:aa},
        url: '/mathang/update/',
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