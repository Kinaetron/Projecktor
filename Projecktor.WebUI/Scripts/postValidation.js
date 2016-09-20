$(document).ready(function () {
    $('#imagebutton').prop('disabled', true);
    $('#textbutton').prop('disabled', true);


    $("#TextPost").change(function () {

        if ($(this).val().length > 0) {
            $('#textbutton').prop('disabled', false);
        }
        else {
            $('#textbutton').prop('disabled', true);
        }
    });

    $("#Images").change(function () {
        var imageInp = $("#Images");
        var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
        var count = $(this).get(0).files.length;

        if (count > 0)
        {
            for (var i = 0; i < count; ++i) {
                if ($.inArray($(this).get(0).files[i].name.split('.').pop().toLowerCase(), fileExtension) == -1) {
                    $('#imagebutton').prop('disabled', true);
                }
                else {
                    $('#imagebutton').prop('disabled', false);
                }
            }
        }
        else {
            $('#imagebutton').prop('disabled', true);
        }
    });
});
