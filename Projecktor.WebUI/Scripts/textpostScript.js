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