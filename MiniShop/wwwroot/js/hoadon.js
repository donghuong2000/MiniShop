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


$('.btn-get').click(function ()  {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idname = button.data('whatever') // Extract info from data-* attributes
    console.log(idname)
    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/HoaDon/get/' + idname,
        success: function (data) {
            console.log(data)
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