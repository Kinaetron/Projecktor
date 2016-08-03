﻿function DeletePost(postId) {
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
            alert("Something went terribly wrong " + result);
        }
    })
}

function LikePost(postId) {
    if ($('#like_' + postId).val() == "Like") {
        $.ajax
        ({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{postId:' + JSON.stringify(postId) + '}',
            url: 'like',
            success: function () {
                $('#like_' + postId).val("Unlike");
            },
            error: function () {
                alert("Something went terribly wrong");
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
                alert("Something went terribly wrong");
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
           alert("Something went terribly wrong " + result);
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
           alert("Something went terribly wrong " + result);
       }
   })
}