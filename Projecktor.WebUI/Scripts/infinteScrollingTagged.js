function CheckPostsTagged(url, showpost, postId) {
    $.ajax
   ({
       type: 'GET',
       url: "/" + url + "check",
       data: { 'pageIndex': JSON.stringify(pageIndex), 'pageSize': JSON.stringify(pageSize), 'term': JSON.stringify(postId) },
       dataType: 'json',
       async: 'false',
       success: function (check) {
           if (check == true) {
               GetPostsTagged(url, showpost, postId);
           }
       },
   })
}

function GetPostsTagged(url, showpost, postId) {
    $.ajax
    ({
        type: 'GET',
        url: "/" + url,
        data: { 'pageIndex': JSON.stringify(pageIndex), 'pageSize': JSON.stringify(pageSize), 'term': JSON.stringify(postId) },
        dataType: 'json',
        async: 'false',
        success: function (dataId) {
            $.post("/" + showpost, $.param({ data: dataId }, true), function (data) {
                $('#container').append(data);
            })
            pageIndex++;
        },
        beforeSend: function () {

        },
        complete: function () {
            $("#progress").hide();
        },
    })
}

$(document).ready(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height())
        {
            var pathname = window.location.pathname;
            var arr = pathname.split('/');

            CheckPostsTagged('gettagged', 'showpost', arr[2]);
        }
    });
});