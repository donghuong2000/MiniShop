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
	});




});

