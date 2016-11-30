function DeletePost(postId) {
    $.ajax
    ({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: '{postId:' + JSON.stringify(postId) + '}',
        url: '/deletepost',
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
            url: '/like',
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
            url: '/unlike',
            success: function () {
                $('#like_' + postId).val("Like");
            },
            error: function () {
                alert("Something went terribly wrong with unlike");
            }
        })
    }
}

function FollowUser(username, userId) {
    if ($('#follow_' + userId).val() == "Follow") {
        $.ajax
        ({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{username:' + JSON.stringify(username) + '}',
            url: '/follow',
            success: function () {
                $('#follow_' + userId).val("Unfollow");
            },
            error: function () {
                alert("Something went terribly wrong with like");
            }
        })
    }
    else if ($('#follow_' + userId).val() == "Unfollow") {
        $.ajax
        ({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{username:' + JSON.stringify(username) + '}',
            url: '/unfollow',
            success: function () {
                $('#follow_' + userId).val("Follow");
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
       url: '/reblog',
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
       url: '/deletereblog',
       success: function () {
           $("#posts_" + postId).remove();
       },
       error: function (result) {
           alert("Something went terribly wrong delete reblog" + result);
       }
   })
}