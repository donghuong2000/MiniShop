

$(document).ready(function () {
    var idname = $('#donnhaphangid').text();
    var html = '';
    console.log(idname)
    $.ajax({
        method: 'GET',
        url: '/DonNhapHang/GetDetail/' + idname,
        success: function (data) {
            console.log(data) 
            var count = data.data[0].NHA_CUNG_CAP[0].CHI_TIET_DON_NHAP_HANG.length
            console.log(count)
            $('#nccid').text('Mã nhà cung cấp: ' + data.data[0].NHA_CUNG_CAP[0].MANCC)
            $('#nccname').text('Tên nhà cung cấp: ' + data.data[0].NHA_CUNG_CAP[0].TENNCC)
            $('#dateadded').text( data.data[0].NGAYNHAP)
            for (var i = 0; i < count; i++) {
                html += ` <tr>
                                    <td>${data.data[0].NHA_CUNG_CAP[0].CHI_TIET_DON_NHAP_HANG[i].MAHANG}</td>
                                    <td class="text-center">${data.data[0].NHA_CUNG_CAP[0].CHI_TIET_DON_NHAP_HANG[i].MAT_HANG[0].TENMH}</td>>
                                    <td class="text-right">${data.data[0].NHA_CUNG_CAP[0].CHI_TIET_DON_NHAP_HANG[i].SOLUONG}</td>
                                </tr>` 

            }
            $('#productdetail').html(html);
            
        }
    })
});

