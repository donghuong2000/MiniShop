// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/HoaDon/GetAll"
        },
        "columns": [
            { "data": "MAHD" },
            { "data": "MAKH" },
            { "data": "MANV" },
            { "data": "NGAYLHD" },
            { "data": "TONGTIEN" },
            { "data": "MAGIAMGIA" },
            {
                "data": "MAHD",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a href="HoaDon/Detail/${data}"  class="btn btn-success text-white btn-get" style="cursor:pointer">
                                   <i class="fas fa-info-circle"></i>
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

//$('#updateform').click(function () {


