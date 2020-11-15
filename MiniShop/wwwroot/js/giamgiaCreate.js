// show selection của các cách áp dụng mã giảm giá
var currchecked = "none";
$('#ggsp').click(function () {
    var popup = $('#giamgiatheosanpham');
    if (this.checked) {
        if (currchecked == 'none')
            currchecked = 'ggsp';
        else if (currchecked == 'ggpl')
            currchecked = 'all';
        else
            currchecked = 'ggpl';
        popup.removeAttr("hidden");
    }
    else {
        if (currchecked == 'ggsp')
            currchecked = 'none';
        else if (currchecked == 'all')
            currchecked = 'ggpl';
        popup.attr('hidden', 'true');
    }
    document.getElementById("ggall").checked = false;
    console.log(currchecked);
})
$('#ggpl').click(function () {
    var popup = $('#giamgiatheophanloai');
    if (this.checked) {
        if (currchecked == 'none')
            currchecked = 'ggpl';
        else if (currchecked == 'ggsp')
            currchecked = 'all';
        else
            currchecked = 'ggsp';
        popup.removeAttr("hidden");
    }
    else
    {
        if (currchecked == 'ggpl')
            currchecked = 'none';
        else if (currchecked == 'all')
            currchecked = 'ggsp';
        popup.attr('hidden', 'true');
    }
    document.getElementById("ggall").checked = false;
    console.log(currchecked);
})
$('#ggall').click(function () {
    var ggsp = document.getElementById("ggsp");
    var ggpl = document.getElementById("ggpl");
    var popupsp = $('#giamgiatheosanpham');
    var popuppl = $('#giamgiatheophanloai');
    console.log(currchecked);
    if (this.checked) {
        ggpl.checked = false;
        ggsp.checked = false;
        popupsp.attr('hidden', 'true');
        popuppl.attr('hidden', 'true');
    }
    else {
        if (currchecked == 'ggsp') {
            ggsp.checked = true;
            popupsp.removeAttr("hidden");
        }
        if (currchecked == 'ggpl') {
            ggpl.checked = true;
            popuppl.removeAttr("hidden");
        }
        if (currchecked == 'all')
        {
            ggpl.checked = true;
            ggsp.checked = true;
            popupsp.removeAttr("hidden");
            popuppl.removeAttr("hidden");
        }
    }
        
})