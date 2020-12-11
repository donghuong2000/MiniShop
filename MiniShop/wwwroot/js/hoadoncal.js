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
	tax_sum = total / 100 * $('#tax').val();
	$('#discount_amount').val(tax_sum.toFixed(2));
	$('#total_amount').val((tax_sum + total).toFixed(2));
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
$('#tableproduct').on('change', '#select-discount', function () {
	var optionSelected = $(this).find("option:selected");
	var parent = $(this).parent().parent();
	var valueSelected = optionSelected.val();
	var price = parent.find('#discount_amount');
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