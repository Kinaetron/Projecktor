function GlassOn() {
    $(".post-forms-glass").css({ "opacity": "1", "display": "block" });
    $("body").css({ "overflow": "hidden", "height": "100%" })
}

function GlassOff() {
    $(".post-forms-glass").css({ "opacity": "0", "display": "none" });
    $("body").removeAttr("style");
}