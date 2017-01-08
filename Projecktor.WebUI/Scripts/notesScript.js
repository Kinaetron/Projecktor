function OpenNotes(noteId) {
    $('#notes_' + noteId).css('display', 'block');
}

$(document).ready(function () {
    $("body").click(function (e) {
        if (e.target.className != "post-notes-link") {
            $('.notes-information').css('display', 'none');
        }
    });
});