$('.form').find('input, textarea').on('keyup blur focus', function (e) {

    var $this = $(this),
        label = $this.prev('label');

    if (e.type === 'keyup') {
        if ($this.val() === '') {
            label.removeClass('active highlight');
        } else {
            label.addClass('active highlight');
        }
    } else if (e.type === 'blur') {
        if ($this.val() === '') {
            label.removeClass('active highlight');
        } else {
            label.removeClass('highlight');
        }
    } else if (e.type === 'focus') {

        if ($this.val() === '') {
            label.removeClass('highlight');
        }
        else if ($this.val() !== '') {
            label.addClass('highlight');
        }
    }

});

$('.tab a').on('click', function (e) {

    e.preventDefault();

    $(this).parent().addClass('active');
    $(this).parent().siblings().removeClass('active');

    target = $(this).attr('href');

    hidePage(target);
});

window.addEventListener("load", (event) => {
    target = $('.tab.active a').attr('href');
    hidePage(target);

    var elements = $('.tab-content input');
    elements.each(function (i) {
        var $this = $(this);
        if ($this !== undefined) {
            var label = $this.prev('label');
            if ($this.val() !== '') {
                label.addClass('active highlight');
            }
        }
    });
});

function hidePage(id) {
    $('.tab-content > div').not(id).hide();
    $(id).fadeIn(600);
}