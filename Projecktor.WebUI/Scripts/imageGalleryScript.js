function Gallery(imagestring, vignetteString) {
    $("<img>", { src: vignetteString, id: "vignette" }).prependTo("#projecktor_lightbox");
    $("<img>", { src: imagestring, id: "centreImage" }).prependTo("#displayImage");
    $("#vignette").css({ "position": "absolute", "width": "100%", "height": "100%", "left": "0px", "top": "0px" });
    $("#projecktor_lightbox").css({ "position": "fixed", "top": "0px", "bottom": "0px", "left": "0px", "right": "0px", "z-index": "2147483647", "overflow": "hidden", "background-color": "rgba(17, 17, 17, 0.921569)" });
    $("body").css({ "overflow": "hidden" })
    ImageSize();
}

$(document).ready(function() {
    $(window).bind("resize", function() {
        ImageSize();
    });

    $('body').on('click', '#projecktor_lightbox', function () {
        $("#vignette").removeAttr("style");
        $("#projecktor_lightbox").removeAttr("style");
        $("body").css({ 'overflow': 'visible' });
        $("#centreImage").remove();
        $('#vignette').remove();
    });
})

function ImageSize() {
    var img = document.getElementById("centreImage");


    if (img.naturalWidth >= $(window).width()) {
        $('#centreImage').css("max-width", $(window).width() - img.naturalWidth / 4);
    }
    else {
        $('#centreImage').css("max-width",img.naturalWidth);
    }

    if (img.naturalHeight >= $(window).height()) {
        $('#centreImage').css("max-height", $(window).height() - img.naturalHeight / 4);
    }
    else {
        $('#centreImage').css("max-height", img.naturalHeight);
    }

    $('#centreImage').css("width", "100%");
    $('#centreImage').css("height", "100%");

    $("#displayImage").css({ "position": "fixed", "top": "50%", "left": "50%", "margin-top": -img.height / 2, "margin-left": -img.width / 2 });
}