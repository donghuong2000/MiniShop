

$(document).ready(function () {
    var idname = $('#hoadonid').text();
    var html = '';
    console.log(idname)
    $.ajax({
        method: 'GET',
        url: '/HoaDon/GetDetail/' + idname,
        success: function (data) {
            var count = data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON.length
            console.log(data.data[0].NHAN_VIEN)
            $('#staffid').text('Mã nhân viên: ' + data.data[0].MANV)
            $('#staffname').text('Tên nhân viên: ' + data.data[0].NHAN_VIEN[0].TENNV)
            $('#customerid').text('Mã khách hàng: ' + data.data[0].MAKH)
            $('#customername').text('Tên khách hàng: ' + data.data[0].NHAN_VIEN[0].KHACH_HANG[0].TENKH)
            $('#orderdate').text(data.data[0].NGAYLHD)
            for (var i = 0; i < count; i++) {
                html += ` <tr>
                                    <td>${data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].MAMH}</td>
                                    <td class="text-center">${data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].MAT_HANG[0].TENMH}</td>
                                    <td class="text-center">${data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].SOLUONG}</td>
                                    <td class="text-center">${data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].MAT_HANG[0].GIA}</td>
                                    <td class="text-right">${data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].MAT_HANG[0].TONGGIASANPHAM}</td>
                                </tr>` 

            }
            html += `<tr>
                                    <td class="thick-line"></td>
                                    <td class="thick-line"></td>
                                    <td class="thick-line"></td>
                                    <td class="thick-line text-center"><strong>Subtotal</strong></td>
                                    <td class="thick-line text-right">$670.99</td>
                                </tr>
                                <tr>
                                    <td class="no-line"></td>
                                    <td class="no-line"></td>
                                    <td class="no-line"></td>
                                    <td class="no-line text-center"><strong>Shipping</strong></td>
                                    <td class="no-line text-right">$15</td>
                                </tr>
                                <tr>
                                    <td class="no-line"></td>
                                    <td class="no-line"></td>
                                    <td class="no-line"></td>
                                    <td class="no-line text-center"><strong>Total</strong></td>
                                    <td class="no-line text-right">$685.99</td>
                                </tr>`
            $('#productdetail').html(html);
            
        }
    })
});

