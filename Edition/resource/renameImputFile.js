

$(function () {
    $("#editionDiv input:file").each(function() {
        var n = $(this).attr('name');
        $(this).attr('name', '#id#' + n);
    });
});

$(function () {

});
