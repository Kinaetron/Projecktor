var pageSize = 10;
var pageIndex = 1;

$(document).ready(function ()
{
    $(window).scroll(function ()
    {
        if($(window).scrollTop() == $(document).height() - $(window).height()) {
            GetPosts();
        }
    });
});

function GetPosts()
{
  $.ajax
  ({
      type: 'GET',
      url:  'getposts',
      data: {'pageIndex': JSON.stringify(pageIndex), 'pageSize': JSON.stringify(pageSize) },
      dataType: 'json',
      async: 'false',
      success: function (dataId)
      {
          $.post("showpost", $.param({ data: dataId }, true), function (data) {
              $('#container').append(data);
          })
          pageIndex++;
      },
      beforeSend: function () {
          $("#progress").show();
      },
      complete: function () {
          $("#progress").hide();
      },
      error: function () {
          alert("Something went terribly wrong with infinite scrolling " + pageIndex);
      }
  })
}


function DeletePost(postId) {
    $.ajax
    ({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: '{postId:' + JSON.stringify(postId) + '}',
        url: 'deletepost',
        success: function () {
            $("#posts_" + postId).remove();
        },
        error: function (result) {
            alert("Something went terribly wrong delete" + result);
        }
    })
}

function LikePost(postId, sourceId) {
    if ($('#like_' + postId).val() == "Like") {
        $.ajax
        ({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{postId:' + JSON.stringify(postId) + ', sourceId:' + JSON.stringify(sourceId) + '}',
            url: 'like',
            success: function () {
                $('#like_' + postId).val("Unlike");
            },
            error: function () {
                alert("Something went terribly wrong with like");
            }
        })
    }
    else if ($('#like_' + postId).val() == "Unlike") {
        $.ajax
        ({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{postId:' + JSON.stringify(postId) + '}',
            url: 'unlike',
            success: function () {
                $('#like_' + postId).val("Like");
            },
            error: function () {
                alert("Something went terribly wrong with unlike");
            }
        })
    }
}

function Reblog(textId, reblogId, sourceId) {
    $.ajax
   ({
       type: 'POST',
       contentType: 'application/json; charset=utf-8',
       data: '{textId:' + JSON.stringify(textId) + ', reblogId:' + JSON.stringify(reblogId) + ', sourceId:' + JSON.stringify(sourceId) + '}',
       url: 'reblog',
       error: function (result) {
           alert("Something went terribly wrong reblog" + result);
       }
   })
}

function DeleteReblog(postId) {
    $.ajax
   ({
       type: 'POST',
       contentType: 'application/json; charset=utf-8',
       data: '{postId:' + JSON.stringify(postId) + '}',
       url: 'deletereblog',
       success: function () {
           $("#posts_" + postId).remove();
       },
       error: function (result) {
           alert("Something went terribly wrong delete reblog" + result);
       }
   })
}