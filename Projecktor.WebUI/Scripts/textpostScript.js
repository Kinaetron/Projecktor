$(function () {
    $('#textpost').on('click', function () {
        if ($(this).hasClass('picked')) {
            unselect($(this));
        }
        else {
            $(this).addClass('picked');
            $('.open').slideFadeToggle();
        }
        return false;
    });

    $('.stop').on('click', function () {
        $(this).closest('form').find("#TextPost, textarea").val("");
        $(this).closest('form').find("#Hashtags, textarea").val("");
        unselect($('#textpost'));
        return false;
    });
});

function unselect(e) {
    $('.open').slideFadeToggle(function () {
        e.removeClass('picked');
    });
}

$.fn.slideFadeToggle = function (easing, callback) {
    return this.animate({ opacity: 'toggle', height: 'toggle' }, 'fast', easing, callback);
};