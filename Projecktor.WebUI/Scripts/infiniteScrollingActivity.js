$(document).ready(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {
            CheckPosts('getactivity', 'showactivity');
        }
    });
});
