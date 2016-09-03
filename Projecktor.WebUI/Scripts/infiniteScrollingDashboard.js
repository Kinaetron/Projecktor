$(document).ready(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height()) {
            GetPosts('getposts');
        }
    });
});
