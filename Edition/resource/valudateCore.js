 function validateCore() {

 if( $('#IsValidate').length ){
   if($('#IsValidate').val()=='False') return true;
 }
 var error1=0; var error2=0;  #validate# if(error1 > 0||error2 > 0)
  {
   alert('Error input..');if(error1 > 0) {$('#error_1').css('display','block');   }if(error2>0) {  $('#error_2').css('display','block'); }return false;} $('#error_1').css('display','none'); $('#error_2').css('display','none');  return true; }