$(document).ready(function () {
	var i = 1;
	$("#add_row").click(function () {
		b = i - 1;
		$('#addr' + i).html($('#addr' + b).html()).find('td:first-child').html(i + 1);
		$('#tab_logic').append('<tr id="addr' + (i + 1) + '"></tr>');
		i++;
	});
	$("#delete_row").click(function () {
		if (i > 1) {
			$("#addr" + (i - 1)).html('');
			i--;
		}
		calc();
	});

	$('#tab_logic tbody').on('keyup change', function () {
		calc();
	});
	$('#tax').on('keyup change', function () {
		calc_total();
	});


});

function calc() {
	$('#tab_logic tbody tr').each(function (i, element) {
		var html = $(this).html();
		if (html != '') {
			var qty = $(this).find('.qty').val();
			var price = $(this).find('.price').val();
			$(this).find('.total').val(qty * price);

			calc_total();
		}
	});
}
// ma giam gia 
function calc_total() {
	total = 0;
	$('.total').each(function () {
		total += parseInt($(this).val());
	});
	$('#sub_total').val(total.toFixed(2));
	discount_minus = total / 100 * $('#discount_percent').val();
	console.log($('#discount_percent').val())
	console.log($('#discount_money').val())
	
	if (discount_minus > $('#discount_money').val())
		discount_minus =   parseFloat($('#discount_money').val())
	console.log(discount_minus)
	discount_sum = total - discount_minus;
	console.log(discount_sum)
	$('#discount_amount').val(discount_minus.toFixed(2));
	$('#total_amount').val((discount_sum).toFixed(2));
}


$('#tableproduct').on('change', '.select-product', function () {
	var optionSelected = $(this).find("option:selected");
	var parent = $(this).parent().parent();
	var valueSelected = optionSelected.val();
	var price = parent.find('.price');
	$.ajax({
		method: 'get',
		url: '/HoaDon/GetPrice/' + valueSelected,
		success: function (data) {
			console.log(data.data)
			price.val(data.data)
			calc()
		}
	})

});
$('#tableprice').on('change', '#select-discount', function () {
	var optionSelected = $(this).find("option:selected");
	var parent = $(this).parent().parent();
	var valueSelected = optionSelected.val();
	console.log(valueSelected)
	var percent = parent.find('#discount_percent');  
	var money = parent.find('#discount_money');
	if (valueSelected == "") {
		console.log('null')
		percent.val(0)
		money.val(0)
		calc()
	}
	else {
		$.ajax({
			method: 'get',
			url: '/GiamGia/Get/' + valueSelected,
			success: function (data) {
				console.log(valueSelected)
				console.log('not null')
				percent.val(data.data[0].PT_GIAM)
				money.val(data.data[0].TIENGIAM)
				calc()
				console.log('done')
			}
		})
    }
	

});