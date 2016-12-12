$(function () {
    $('#imagepost').on('click', function () {
        if ($(this).hasClass('selected')) {
            deselect($(this));
        }
        else {
            $(this).addClass('selected');
            $('.pop').slideFadeToggle();
        }
        return false;
    });

    $('.close').on('click', function () {
        deselect($('#imagepost'));
        $(this).closest('form').find("#Text, textarea").val("");
        $(this).closest('form').find("#Hashtags, textarea").val("");
        $(this).closest('form').find("#Images, file").val("");
        return false;
    });
});

function deselect(e) {
    $('.pop').slideFadeToggle(function () {
        e.removeClass('selected');
    });
}

$.fn.slideFadeToggle = function (easing, callback) {
    return this.animate({ opacity: 'toggle', height: 'toggle' }, 'fast', easing, callback);
};