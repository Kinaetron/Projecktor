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
    ImageSize();
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
        ImageSize();
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

function ImageSize() {

    //if (img.naturalWidth >= $(window).width()) {
    //    $('#centreImage').css("max-width", $(window).width() - img.naturalWidth / 4);
    //}
    //else {
    //    $('#centreImage').css("max-width",img.naturalWidth);
    //}

    //if (img.naturalHeight >= $(window).height()) {
    //    $('#centreImage').css("max-height", $(window).height() - img.naturalHeight / 4);
    //}
    //else {
    //    $('#centreImage').css("max-height", img.naturalHeight);
    //}

    //$('#centreImage').css("width", "100%");
    //$('#centreImage').css("height", "100%");

    $('#centreImage').css("display", "block");

    $('#centreImage').css("width", "auto");
    $('#centreImage').css("height", "auto");


    var img = document.getElementById("centreImage");

    //if (img.naturalWidth >= $(window).width() || img.naturalHeight >= $(window).height()) {
    //    $('#centreImage').css("max-width", "85%");
    //    $('#centreImage').css("max-height", "80%");
    //}
    //else
    //{
        $('#centreImage').css("max-width", "100%");
        $('#centreImage').css("max-height", "90%");
    //}

    var imgWidth = $("#centreImage").width();
    var imgHeight = $("#centreImage").height();

    $("#displayImage").css({ "position": "absolute", "top": "50%", "left": "50%", "margin-top": -imgHeight / 2, "margin-left": -imgWidth / 2 });
}