var pageSize = 15;
var pageIndex = 1;
var callScript = true;

function CheckPosts(url, showpost) {
    $.ajax
   ({
       type: 'GET',
       url: "/" + url + "check",
       data: { 'pageIndex': JSON.stringify(pageIndex), 'pageSize': JSON.stringify(pageSize) },
       dataType: 'json',
       async: 'false',
       success: function (check) {
           if (check == true && callScript == true) {
               GetPosts(url, showpost);
               callScript = false;
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
            pageIndex++;
            callScript = true;
        },
        beforeSend: function () {
            $("#progress").show();
        },
        complete: function () {
            $("#progress").hide();
        },
    })
}