var postCount = 0;

(function () {
    $.ajax
    ({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: '/postcount',
        success: function (data) {
            if (postCount == 0) {
                postCount = data;
            }
            else if (data - postCount > 99) {
                $("#count-display").text("99+");
            }
            else if (data - postCount > 0) {
                $("#count-display").text(data - postCount);
            }

        }
    })
    setTimeout(arguments.callee, 5000);
})();