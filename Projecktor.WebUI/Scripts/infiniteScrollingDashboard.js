var pageSize = 10;
var pageIndex = 1;
var callScript = true;


$(document).ready(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 100 && callScript == true) {
            CheckPosts('getposts', 'showpost');
            callScript = false;
            $("#progress").show();
        }
    });
});

function CheckPosts(url, showpost) {
    $.ajax
   ({
       type: 'GET',
       url: "/" + url + "check",
       data: { 'pageIndex': JSON.stringify(pageIndex), 'pageSize': JSON.stringify(pageSize) },
       dataType: 'json',
       async: 'false',
       success: function (check) {
           if (check == true) {
               GetPosts(url, showpost);
           }
           else {
               $("#progress").hide();
           }
       },
   })
}

function GetPosts(url, showpost) {
    $.ajax
    ({
        type: 'GET',
        url: "/" + url,
        data: { 'pageIndex': JSON.stringify(pageIndex), 'pageSize': JSON.stringify(pageSize) },
        dataType: 'json',
        async: 'false',
        success: function (dataId) {
            $.post(showpost, $.param({ data: dataId }, true), function (data) {
                $('#container').append(data);
            })
        },
        complete: function () {
            pageIndex++;
            callScript = true;
            $("#progress").hide();
        },
    })
}
