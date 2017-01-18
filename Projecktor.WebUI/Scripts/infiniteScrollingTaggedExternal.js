function CheckPostsTaggedExternal(url, showpost, postId) {
    $.ajax
   ({
       type: 'GET',
       url: "/" + url + "check",
       data: { 'pageIndex': JSON.stringify(pageIndex), 'pageSize': JSON.stringify(pageSize), 'term': JSON.stringify(postId) },
       dataType: 'json',
       async: 'false',
       success: function (check) {
           if (check == true) {
               GetPostsTaggedExternal(url, showpost, postId);
               $("#progress").show();
           }
       },
   })
}

function GetPostsTaggedExternal(url, showpost, postId) {
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
        complete: function () {
            $("#progress").hide();
        },
    })
}

$(document).ready(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height()) {
            var pathname = window.location.pathname;
            var arr = pathname.split('/');

            CheckPostsTaggedExternal('gettagged', 'showuserpost', arr[2]);
        }
    });
});