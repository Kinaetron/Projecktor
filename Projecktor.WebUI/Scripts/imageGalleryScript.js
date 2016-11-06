var index = 0;
var galleryImages;
var imageString;
var vignetteURL;

var galleryOne = false;
var galleryMany = false;

var boxRatio = 0;
var aspectRatio = 0;
var scaleFactor = 0;
var reduceHeightBy = 0;
var reduceWidthBy = 0;
var newHeight = 0;
var newWidth = 0;

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
        ImageSwitch();
    });

    $('body').on('click', '#projecktor_lightbox_right_link', function () {

        if (parseInt(index) + 1 >= galleryImages.length) {
            index = 0;
        }
        else {
            index++;
        }
        ImageSwitch();
    });

    $('body').on('click', '#projecktor_lightbox_left_link', function () {

        if (parseInt(index) - 1 <= 0) {
            index = 0;
        }
        else {
            index--;
        }
        ImageSwitch();
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
            ImageSwitch();
        }

        if (e.keyCode == 37 && galleryMany == true) { // left
            if (parseInt(index) - 1 < 0) {
                index = 0;
                return;
            }
            else {
                index--;
            }
            ImageSwitch();
        }
    });

    $('body').on('click', '#vignette', function () {

        if (galleryMany == true)
        {
            AssetRemoval();

            galleryOne = false;
            galleryMany = false;
        }
    });

    $('body').on('click', '#projecktor_lightbox', function () {

        if (galleryOne == true) {
            AssetRemoval();

            galleryOne = false;
            galleryMany = false;
        }
    });
})


function AssetRemoval()
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
}

function ImageSwitch() {
    $("#vignette").remove();
    $("#projecktor_lightbox_centre_link").remove();
    $("#projecktor_lightbox_left_link").remove();
    $("#projecktor_lightbox_right_link").remove();
    GalleryShowMany();
}

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

                    $("#displayImage").css({ "position": "absolute", "left": "50%", "top": "50%" });

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
            $("#displayImage").css({ "position": "absolute", "left": "50%", "top": "50%" });

            ImageFunc(img);
        }
    }
}

function ImageFuncLeft(imgLeft) {
    ImageFunction(imgLeft, "#leftImage");
}


function ImageFunc(img) {
    ImageFunction(img, "#centreImage");
}

function ImageFuncRight(imgRight) {
    ImageFunction(imgRight, "#rightImage");
}

function ImageFunction(image, elementName)
{
    boxRatio = $(window).width() / $(window).height();

    aspectRatio = image.width / image.height;
    scaleFactor;

    if (boxRatio > aspectRatio) {
        scaleFactor = $(window).height() / image.height;
    }
    else {
        scaleFactor = $(window).width() / image.width;
    }

    
    reduceHeightBy = (image.height * scaleFactor / 100) * 15;
    reduceWidthBy = (image.width * scaleFactor / 100) * 15;

    newHeight = Math.min(image.height * scaleFactor - reduceHeightBy, image.naturalHeight);
    newWidth = Math.min(image.width * scaleFactor - reduceWidthBy, image.naturalWidth);

    var left = 0;

    if (elementName == "#centreImage") {
        left = -newWidth / 2;
    }
    else if (elementName == "#leftImage") {
        left = 0 - newWidth - 0.42 * $(window).width();
    }
    else if (elementName == "#rightImage") {
        left = 0.42 * $(window).width();
    }

    $(elementName).css({ "position": "absolute", "display": "inline-block", "height": newHeight, "width": newWidth, "left": left, "top": -newHeight / 2 });
    $(elementName).attr('src', image.src);
}