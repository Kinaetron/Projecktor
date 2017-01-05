$(function () {
    $("#searchTerm").autocomplete({
        source: function (request, response) {
            $.ajax(
        {
            url: '/autocomplete',
            dataType: "json",
            data: { id: request.term, },
            success: function (data) {
                response(data);
            }
        });
        },
        select: function (event, ui) {
            $(this).val(ui.item.value);
            $("#searchButton").click();
        }
    });
});

$(document).ready(function () {
    $("#searchTerm").keypress(function (event) {
        if (event.keyCode == 13) {
            $("#searchButton").click();
        }
    })

    var pathname = window.location.pathname;
    var arr = pathname.split('/');

    if ($.isNumeric(arr[2]) == false) {
        $('#searchTerm').val(arr[2].replace(/\%20/g, ' '));
    }
});

function SearchLink() {
    var searchTerm = $('#searchTerm').val();
    var initUrl = '/search/xxxx';
    var url = initUrl.replace("xxxx", searchTerm);
    location.href = url;
    return false;
}