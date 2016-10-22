function GalleryOne(imagestring, vignetteString)
{
    $("<img>", { src: vignetteString, id: "vignette" }).prependTo("#projecktor_lightbox");
    $("<img>", { src: imagestring, id: "centreImage" }).prependTo("#displayImage");
    $("#vignette").css({ "position": "absolute", "width": "100%", "height": "100%", "left": "0px", "top": "0px" });
    $("#projecktor_lightbox").css({
        "position": "fixed", "top": "0px", "bottom": "0px", "left": "0px", "right": "0px", "z-index": "2147483647",
        "overflow": "hidden", "background-color": "rgba(17, 17, 17, 0.921569)"
    });

    $("body").css({ "overflow": "hidden" })
    ImageCall();
}

function GalleryShowMany(images, vignetteString) {
    $("<img>", { src: vignetteString, id: "vignette" }).prependTo("#projecktor_lightbox");
    $("<img>", { src: images[0], id: "centreImage" }).prependTo("#displayImage");
    $("<img>", { src: images[1], id: "leftImage" }).prependTo("#displayImage");

    $("#vignette").css({ "position": "absolute", "width": "100%", "height": "100%", "left": "0px", "top": "0px" });
    $("#projecktor_lightbox").css({
        "position": "fixed", "top": "0px", "bottom": "0px", "left": "0px", "right": "0px", "z-index": "2147483647",
        "overflow": "hidden", "background-color": "rgba(17, 17, 17, 0.921569)"
    });

    $("body").css({ "overflow": "hidden" })
}


function GalleryMany(postNumber, vignetteString) {

    var postSplit = postNumber.split("-");
    $.ajax
   ({
       type: 'GET',
       url: 'gallery',
       data: { 'id': postSplit[0] },
       dataType: 'json',
       success: function (dataId) {
           GalleryShowMany(dataId, vignetteString)
       }
   })
}

$(document).ready(function() {
    $(window).bind("resize", function() {
        ImageFunc();
    });

    $('body').on('click', '#projecktor_lightbox', function () {
        $("#vignette").removeAttr("style");
        $("#projecktor_lightbox").removeAttr("style");
        $("#displayImage").removeAttr("style");
        $("body").removeAttr("style");
        $("#centreImage").remove();
        $("#leftImage").remove();
        $('#vignette').remove();
    });
})


function ImageCall()
{
    var img = document.getElementById("centreImage");

    img.onload = function () {
        ImageFunc();
    }
}

function ImageFunc()
{
    var img = document.getElementById("centreImage");

    var aspectRatio = img.width / img.height;
    var boxRatio = $(window).width() / $(window).height();
    var scaleFactor;

    $("#displayImage").css({ "position": "absolute", "left": "50%", "top": "50%" });

    if (boxRatio > aspectRatio) {
        scaleFactor = $(window).height() / img.height;
    }
    else {
        scaleFactor = $(window).width() / img.width;
    }

    var reduceHeightBy = (img.height * scaleFactor / 100) * 15;
    var reduceWidthBy = (img.width * scaleFactor / 100) * 15;

    var newHeight = Math.min(img.height * scaleFactor - reduceHeightBy, img.naturalHeight);
    var newWidth = Math.min(img.width * scaleFactor - reduceWidthBy, img.naturalWidth);

    $("#centreImage").css({ "position": "absolute", "display": "inline-block", "height": newHeight, "width": newWidth, "left": -newWidth / 2, "top": -newHeight / 2 });
}