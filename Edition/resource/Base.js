



function submitValidateRegExp(obj, regs) {

    if (!new RegExp(regs, 'g').test($(obj).val())) {
        $(obj).css('background-color', '#FF0000');
        return 1;
    }
    else {
        return 0;
    }
}
function submitValidateIsEmpty(obj, isNull, strlength) {
    var str = $(obj).val();
    if (strlength <= 0)
        strlength = 4000000000000000;
    if (str.length > strlength) {
        alert('Big amount symbol!');
        $(obj).css('background-color', '#FF0000');
        return 1;
    }
    if (isNull) {
        if ($.trim(str).length == 0) {
            $(obj).css('background-color', 'rgb(167, 30, 136');
            return 1 ;
        }
        else {
            return 0;
        }
    }
    else {
        return 0;
    }
}
jQuery.fn.ShowMaska = function () {
    this.width($(window).width());
    this.height($(window).height() + 200);
    this.css({ opacity: .4 });
    this.css('display', 'block');
    return this;
};
jQuery.fn.CloseMaska = function () {
    this.css('display', 'none');
    return this;
};
jQuery.fn.center = function () {
    var w = $(window);
    this.css("position", "fixed");
    this.css("top", (w.height() - this.height()) / 2  + "px");
    this.css("left", (w.width() - this.width()) / 2 + "px");
    return this;
};

function showDatepicker(div, format) {
    if ($('#' + div + '_div').css('display') == 'block') {
        closeDatepicker(div);
        return;
    }
    var dat = new Date($('#' + div+'').val().replace(/(\d+).(\d+).(\d+)/, '$3/$2/$1'));
    $("#" + div + "_hour [value='" + dat.getHours() + "']").attr("selected", "selected");
    $("#" + div + "_min [value='" + dat.getMinutes() + "']").attr("selected", "selected");
    $('#' + div + '_div').css('display', 'block');
    $('#' + div + '_calendar').datepicker({
        dateFormat: format.split(' ')[0].toLowerCase(). replace("yyyy", "yy"),
        onSelect: function (dateText) {
            $('#' + div + '_date').val(dateText);
        }
    });
};

function showDatepicker2(div, format) {
    var dp = $('#' + div + '_calendar').datepicker({
            dateFormat: format.split(' ')[0].toLowerCase().replace("yyyy", "yy"),
            onSelect: function (dateText) {
                $('#' + div + '').val(dateText);
                dp.datepicker('destroy');
            }
        });
}






function closeDatepicker(div) {
    $('#' + div + '_div').hide();
};
function saveDatepicker(div, format) {
    var d = $('#' + div + '_date').val() + ' ' + $("#" + div + "_hour :selected").val() + ':' + $("#" + div + "_min :selected").val()+":00";
    var dat = new Date(d.replace(/(\d+).(\d+).(\d+)/, '$3/$2/$1'));
    $('#' + div + '').val(d);
    closeDatepicker(div);
}
function ClickColorDp(dp) {
    if ($('#' + dp + '_divc').css('display') == 'block') {
        $('#' + dp + '_divc').hide();
        return;
    }
    $('#' + dp + '_divc').css('display', 'block');
}
function validate(obj, regs, isNull, strlength,errorMessage) {
    if (strlength <= 0)
        strlength = 4000000000000000;
    if ($(obj).val().length > strlength) {
        errorMessage.trim().length==0? alert('Big amount symbol!'):alert(errorMessage);
      
       $(obj).css('background-color', '#FF0000');
       return;
   }
    if (isNull) {
        if ($.trim($(obj).val()).length == 0) {
            errorMessage.trim().length == 0 ? alert('Is not correct the data format. Is not Emptiness..' ) : alert(errorMessage);
            $(obj).css('background-color', '#FF0000');
            return;
        }
    }
    if (!new RegExp(regs, 'g').test($(obj).val())) {
        errorMessage.trim().length == 0 ? alert('Is not correct the data format: ' + $(obj).val()) : alert(errorMessage);
        $(obj).css('background-color', '#FF0000');
    }
    else {
        $(obj).css('background-color', '');
    }
};
function showPanel(pn) {
    $("#maska").ShowMaska();
  //----------------------------------------------------------------------------------------
    $('#' + pn + '_edit_ifr').contents().find('body').html($('#' + pn + '').val());//вствака в редактор
    $('#' + pn + '_panel').center().show();
    $('#' + pn + '_panel').draggable({ handle: $('#' + pn + '_darag')
    });
    return false;
};
function closePanel(pn) {
    $('#maska').CloseMaska();
    $('#' + pn + '').val($('#' + pn + '_edit_ifr').contents().find('body').html());
    $('#' + pn + '_res').html($('#' + pn + '_edit_ifr').contents().find('body').html());
    $('#' + pn + '_panel').hide();
    $('#' + pn + '_panel').draggable("destroy");
    return false;
};
function cancelShowPanel(pn) {
    $('#maska').CloseMaska();
    $('#' + pn + '_panel').hide();
    $('#' + pn + '_panel').draggable("destroy");
    return false;
};
jQuery.fn.exists = function() { return this.length > 0; };


function tynyUp(tb) {
        tinyMCE_GZ.init({
            plugins: 'autolink,lists,pagebreak,style,layer,table,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,filion',
            themes: 'advanced',
            languages: 'ru',
            disk_cache: true,
            debug: false
        });
        tinyMCE.init({
            mode: 'exact',
            elements: ''+tb+'' ,
            theme: "advanced", //simple
            language: "ru",
            plugins: "autolink,lists,pagebreak,style,layer,table,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,filion",
            theme_advanced_buttons1: "newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
            theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,filion,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
            theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
            theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft",
            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_statusbar_location: "bottom",
            theme_advanced_resizing: true,
           
            style_formats: [
			{ title: 'Bold text', inline: 'b' },
			{ title: 'Red text', inline: 'span', styles: { color: '#ff0000'} },
			{ title: 'Red header', block: 'h1', styles: { color: '#ff0000'} },
			{ title: 'Example 1', inline: 'span', classes: 'example1' },
			{ title: 'Example 2', inline: 'span', classes: 'example2' },
			{ title: 'Table styles' },
			{ title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
		]
        });
};