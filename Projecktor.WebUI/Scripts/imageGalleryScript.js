var index = 0;
var galleryImages;
var imageString;
var vignetteURL;

var galleryOne = false;
var galleryMany = false;

function GalleryOne(imagestring, vignetteString)
{
    galleryOne = true;
    galleryMany = false;

    imageString = imagestring;

    $("<img>", { src: vignetteString, id: "vignette" }).prependTo("#projecktor_lightbox");
    $("<img>", {id: "centreImage" }).prependTo("#displayImage");
    $("#vignette").css({ "position": "absolute", "width": "100%", "height": "100%", "left": "0px", "top": "0px" });
    $("#projecktor_lightbox").css({
        "position": "fixed", "top": "0px", "bottom": "0px", "left": "0px", "right": "0px", "z-index": "2147483647",
        "overflow": "hidden", "background-color": "rgba(17, 17, 17, 0.921569)"
    });

    $("body").css({ "overflow": "hidden" })
    ImageCall();
}

function GalleryShowMany()
{
    galleryMany = true;
    galleryOne = false;

    $("<img>", { src: vignetteURL, id: "vignette" }).prependTo("#projecktor_lightbox");

    $("<a>", { id: "projecktor_lightbox_right_link", href: "#" }).prependTo("#displayImage");
    $("<a>", { id: "projecktor_lightbox_centre_link", href: "#" }).prependTo("#displayImage");
    $("<a>", { id: "projecktor_lightbox_left_link", href: "#" }).prependTo("#displayImage");


    $("<img>", { id: "rightImage" }).prependTo("#projecktor_lightbox_right_link");
    $("<img>", { id: "centreImage" }).prependTo("#projecktor_lightbox_centre_link");
    $("<img>", { id: "leftImage" }).prependTo("#projecktor_lightbox_left_link");

    $("#vignette").css({ "position": "absolute", "width": "100%", "height": "100%", "left": "0px", "top": "0px" });
    $("#projecktor_lightbox").css({
        "position": "fixed", "top": "0px", "bottom": "0px", "left": "0px", "right": "0px", "z-index": "2147483647",
        "overflow": "hidden", "background-color": "rgba(17, 17, 17, 0.921569)"
    });

    $("body").css({ "overflow": "hidden" })
    ImageCall();
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
           index = postSplit[1];
           galleryImages = dataId;
           vignetteURL = vignetteString;
           GalleryShowMany()
       }
   })
}

$(document).ready(function () {

    $(window).bind("resize", function () {
        ImageCall();
    });

    $('body').on('click', '#projecktor_lightbox_centre_link', function () {

        if (parseInt(index) + 1 >= galleryImages.length) {
            index = 0;
        }
        else {
            index++;
        }
        $("#vignette").remove();
        $("#projecktor_lightbox_centre_link").remove();
        $("#projecktor_lightbox_left_link").remove();
        $("#projecktor_lightbox_right_link").remove();
        GalleryShowMany();
    });

    $('body').on('click', '#projecktor_lightbox_right_link', function () {

        if (parseInt(index) + 1 >= galleryImages.length) {
            index = 0;
        }
        else {
            index++;
        }
        $("#vignette").remove();
        $("#projecktor_lightbox_centre_link").remove();
        $("#projecktor_lightbox_left_link").remove();
        $("#projecktor_lightbox_right_link").remove();
        GalleryShowMany();
    });

    $('body').on('click', '#projecktor_lightbox_left_link', function () {

        if (parseInt(index) - 1 <= 0) {
            index = 0;
        }
        else {
            index--;
        }
         $("#vignette").remove();
        $("#projecktor_lightbox_centre_link").remove();
        $("#projecktor_lightbox_left_link").remove();
        $("#projecktor_lightbox_right_link").remove();
        GalleryShowMany();
    });

    $(document).keydown(function (e) {
        if (e.keyCode == 39 && galleryMany == true) { // right
            if (parseInt(index) + 1 >= galleryImages.length) {
                index = galleryImages.length - 1;
                return;
            }
            else {
                index++;
            }
            $("#vignette").remove();
            $("#projecktor_lightbox_centre_link").remove();
            $("#projecktor_lightbox_left_link").remove();
            $("#projecktor_lightbox_right_link").remove();
            GalleryShowMany();
        }

        if (e.keyCode == 37 && galleryMany == true) { // left
            if (parseInt(index) - 1 < 0) {
                index = 0;
                return;
            }
            else {
                index--;
            }
            $("#vignette").remove();
            $("#projecktor_lightbox_centre_link").remove();
            $("#projecktor_lightbox_left_link").remove();
            $("#projecktor_lightbox_right_link").remove();
            GalleryShowMany();
        }
    });

    $('body').on('click', '#vignette', function () {

        if (galleryMany == true)
        {
            $("#vignette").remove();
            $("#projecktor_lightbox").removeAttr("style");
            $("#displayImage").removeAttr("style");
            $("body").removeAttr("style");
            $("#centreImage").remove();
            $("#leftImage").remove();
            $("#rightImage").remove();
            $('#vignette').remove();
            $("#projecktor_lightbox_centre_link").remove();
            $("#projecktor_lightbox_left_link").remove();
            $("#projecktor_lightbox_right_link").remove();

            galleryOne = false;
            galleryMany = false;
        }
    });

    $('body').on('click', '#projecktor_lightbox', function () {

        if (galleryOne == true) {
            $("#vignette").remove();
            $("#projecktor_lightbox").removeAttr("style");
            $("#displayImage").removeAttr("style");
            $("body").removeAttr("style");
            $("#centreImage").remove();
            $("#leftImage").remove();
            $("#rightImage").remove();
            $('#vignette').remove();
            $("#projecktor_lightbox_centre_link").remove();
            $("#projecktor_lightbox_left_link").remove();
            $("#projecktor_lightbox_right_link").remove();

            galleryOne = false;
            galleryMany = false;
        }
    });
})


function ImageCall()
{
    var imgs = [];
    var cnt = 0;

    if (galleryMany == true)
    {
        for (var i = 0; i < galleryImages.length; i++) {
            var img = new Image();
            img.onload = function () {
                ++cnt;
                if (cnt >= galleryImages.length) {
                    var imgCentre = imgs[index];
                    var imgLeft = imgs[parseInt(index) - 1];
                    var imgRight = imgs[parseInt(index) + 1]

                    if (imgCentre != null) {
                        ImageFunc(imgCentre);
                    }
                    if (imgLeft != null) {
                        ImageFuncLeft(imgLeft);
                    }
                    if (imgRight != null) {
                        ImageFuncRight(imgRight);
                    }
                }
            };
            img.src = galleryImages[i];
            imgs.push(img);
        }
    }

    if (galleryOne == true) {
        var img = new Image();
        img.src = imageString;
        img.onload = function () {
            ImageFunc(img);
        }
    }
}

function ImageFuncLeft(imgLeft)
{
    var boxRatio = $(window).width() / $(window).height();

    $("#displayImage").css({ "position": "absolute", "left": "50%", "top": "50%" });

    var aspectRatioLeft = imgLeft.width / imgLeft.height;
    var scaleFactorLeft;

    if (boxRatio > aspectRatioLeft) {
        scaleFactorLeft = $(window).height() / imgLeft.height;
    }
    else {
        scaleFactorLeft = $(window).width() / imgLeft.width;
    }

    var reduceHeightByLeft = (imgLeft.height * scaleFactorLeft / 100) * 15;
    var reduceWidthByLeft = (imgLeft.width * scaleFactorLeft / 100) * 15;

    var newHeightLeft = Math.min(imgLeft.height * scaleFactorLeft - reduceHeightByLeft, imgLeft.naturalHeight);
    var newWidthLeft = Math.min(imgLeft.width * scaleFactorLeft - reduceWidthByLeft, imgLeft.naturalWidth);

    var leftLeft = 0 - newWidthLeft - 0.42 * $(window).width();

    $("#leftImage").css({ "position": "absolute", "display": "inline-block", "height": newHeightLeft, "width": newWidthLeft, "left": leftLeft, "top": -newHeightLeft / 2 });
    $("#leftImage").attr('src', galleryImages[index - 1]);
}


function ImageFunc(img)
{
    var boxRatio = $(window).width() / $(window).height();

    $("#displayImage").css({ "position": "absolute", "left": "50%", "top": "50%" });

    var aspectRatio = img.width / img.height;
    var scaleFactor;

    if (boxRatio > aspectRatio) {
        scaleFactor = $(window).height() / img.height;
    }
    else {
        scaleFactor = $(window).width() / img.width;
    }

    var reduceHeightBy = (img.height * scaleFactor / 100) * 15;
    var reduceWidthBy = (img.width * scaleFactor / 100) * 15;

    var newHeightCentre = Math.min(img.height * scaleFactor - reduceHeightBy, img.naturalHeight);
    var newWidthCentre = Math.min(img.width * scaleFactor - reduceWidthBy, img.naturalWidth);

    var leftCentre = -newWidthCentre / 2;

    $("#centreImage").css({ "position": "absolute", "display": "inline-block", "height": newHeightCentre, "width": newWidthCentre, "left": leftCentre, "top": -newHeightCentre / 2 });
    $("#centreImage").attr('src', img.src);
}

function ImageFuncRight(imgRight)
{
    var boxRatio = $(window).width() / $(window).height();

    $("#displayImage").css({ "position": "absolute", "left": "50%", "top": "50%" });

    var aspectRatioRight = imgRight.width / imgRight.height;
    var scaleFactorRight;

    if (boxRatio > aspectRatioRight) {
        scaleFactorRight = $(window).height() / imgRight.height;
    }
    else {
        scaleFactorRight = $(window).width() / imgRight.width;
    }

    var reduceHeightByRight = (imgRight.height * scaleFactorRight / 100) * 15;
    var reduceWidthByRight = (imgRight.width * scaleFactorRight / 100) * 15;

    var newHeightRight = Math.min(imgRight.height * scaleFactorRight - reduceHeightByRight, imgRight.naturalHeight);
    var newWidthRight = Math.min(imgRight.width * scaleFactorRight - reduceWidthByRight, imgRight.naturalWidth);

    var leftRight = 0.42 * $(window).width();

    $("#rightImage").css({ "position": "absolute", "display": "inline-block", "height": newHeightRight, "width": newWidthRight, "left": leftRight, "top": -newHeightRight / 2 });
    $("#rightImage").attr('src', imgRight.src);
}