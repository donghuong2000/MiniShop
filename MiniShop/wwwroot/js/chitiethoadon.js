

$(document).ready(function () {
    var idname = $('#hoadonid').text();
    var html = '';
    console.log(idname)
    $.ajax({
        method: 'GET',
        url: '/HoaDon/GetDetail/' + idname,
        success: function (data) {
            var count = data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON.length
            var magiamgia = ""
            if (data.data[0].MAGIAMGIA == undefined) {
                magiamgia = 'Không có'
            }
            else {
                magiamgia = data.data[0].MAGIAMGIA
            }
            console.log(data)
            $('#staffid').text('Mã nhân viên: ' + data.data[0].MANV)
            $('#staffname').text('Tên nhân viên: ' + data.data[0].NHAN_VIEN[0].TENNV)
            $('#customerid').text('Mã khách hàng: ' + data.data[0].MAKH)
            $('#customername').text('Tên khách hàng: ' + data.data[0].NHAN_VIEN[0].KHACH_HANG[0].TENKH)
            $('#orderdate').text(data.data[0].NGAYLHD)
            var subtotal = 0;
            for (var i = 0; i < count; i++) {
                subtotal += data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].MAT_HANG[0].TONGGIASANPHAM;
                html += ` <tr>
                                    <td>${data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].MAMH}</td>
                                    <td class="text-center">${data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].MAT_HANG[0].TENMH}</td>
                                    <td class="text-center">${data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].MAT_HANG[0].GIA}</td>
                                    <td class="text-center">${data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].SOLUONG}</td>
                                    <td class="text-right">${data.data[0].NHAN_VIEN[0].KHACH_HANG[0].CHI_TIET_HOA_DON[i].MAT_HANG[0].TONGGIASANPHAM}</td>
                                </tr>` 

            }
            html += `<tr>
                                    <td class="thick-line"></td>
                                    <td class="thick-line"></td>
                                    <td class="thick-line"></td>
                                    <td class="thick-line text-center"><strong>Tạm tính : </strong></td>
                                    <td class="thick-line text-right">${subtotal}</td>
                                </tr>
                                <tr>
                                    <td class="no-line"></td>
                                    <td class="no-line"></td>
                                    <td class="no-line"></td>
                                    <td class="no-line text-center"><strong>Mã giảm giá : </strong></td>
                                    <td class="no-line text-right">${magiamgia}</td>
                                </tr>
                                <tr>
                                    <td class="no-line"></td>
                                    <td class="no-line"></td>
                                    <td class="no-line"></td>
                                    <td class="no-line text-center"><strong>Tổng tiền</strong></td>
                                    <td class="no-line text-right">${data.data[0].TONGTIEN}</td>
                                </tr>`
            $('#productdetail').html(html);
            
        }
    })
});

