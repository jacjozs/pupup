$(document).ready(() => {
    
	$('.slider').css({
		'transform': 'translate(' + $(".nav-active").position().left + 'px, 0)',
		'width': $(".nav-active").outerWidth()
		});
})