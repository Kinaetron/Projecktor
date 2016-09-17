$(window).scroll(function () {

    if ($(this).scrollTop()) {
        $('#toTop').fadeIn();
    } else {
        $('#toTop').fadeOut();
    }
});

$(document).ready(function () {
    $("#toTop").click(function () {
        $("html, body").animate({ scrollTop: 0 }, 200);
    });
});