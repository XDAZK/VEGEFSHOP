$(window).scroll(function () {
    if ($(this).scrollTop() != 0) {
        $('#bttop').show();
    }
    else {
        $('#bttop').hide();
    }
});


$('#bttop').click(function () {
    $('html').animate({
        scrollTop: 0
    }, 500);
});