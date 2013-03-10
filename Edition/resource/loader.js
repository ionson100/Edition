
$($(function () {
    var obj = $.parseJSON('#str#');
    $.each(obj, function (i, val) {



        if ($('#' + i + '').is('input') || $('#' + i + '').is('select') || $('#' + i + '').is('textarea')) {

            if ($('#' + i + '').is('[type =checkbox]') && val == true) {
                $('#' + i + '').attr('checked', 'checked');
            $('[type =hidden]&&[id = ' + i + ']').remove();

        }
        $('#' + i + '').val(val);
    }
    else {
            alert(i);
            $('#' + i + '').html(val);
        }
        
       

    });
}));



