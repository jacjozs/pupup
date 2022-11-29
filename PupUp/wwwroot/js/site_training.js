var bg = document.querySelector('.item-bg');
var items = document.querySelectorAll('.step__item');
var item = document.querySelector('.step__item');


if ($(window).width() >= 768) {
    $(document).on("mouseover", ".step__item", function (_event, _element) {

        var stepItem = document.querySelectorAll('.step__item');
        stepItem.forEach(function (element, index) {
            element.addEventListener('mouseover', function () {
                $('.step__item').removeClass('active');
            });
            element.addEventListener('mouseleave', function () {
                $('.step__item').removeClass('active');
            });

        });

    });
}


var swiper = new Swiper('.step-slider', {
    effect: 'coverflow',
    grabCursor: true,
    loop: false,
    centeredSlides: true,
    keyboard: true,
    spaceBetween: 0,
    slidesPerView: 'auto',
    speed: 300,
    coverflowEffect: {
        rotate: 0,
        stretch: 0,
        depth: 0,
        modifier: 3,
        slideShadows: false
    },
    breakpoints: {
        480: {
            spaceBetween: 0,
            centeredSlides: true
        }
    },
    simulateTouch: true,
    navigation: {
        nextEl: '.step-slider-next',
        prevEl: '.step-slider-prev'
    },
    pagination: {
        el: '.step-slider__pagination',
        clickable: true
    },
    on: {
        init: function () {
            var activeItem = document.querySelector('.swiper-slide-active');

            var sliderItem = activeItem.querySelector('.step__item');

            sliderItem.classList.add('active');
        }
    }
});

swiper.on('touchEnd', function () {
    $('.step__item').removeClass('active');
    $(this).addClass('active');
});

swiper.on('slideChange', function () {
    $('.step__item').removeClass('active');
    var activeItem = document.querySelector('.swiper-slide-active');

    var sliderItem = activeItem.querySelector('.step__item');

    sliderItem.classList.add('active');
});

swiper.on('slideChangeTransitionEnd', function () {
    $('.step__item').removeClass('active');
    var activeItem = document.querySelector('.swiper-slide-active');

    var sliderItem = activeItem.querySelector('.step__item');

    sliderItem.classList.add('active');
});