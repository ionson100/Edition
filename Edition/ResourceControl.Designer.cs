﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.17929
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Edition {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ResourceControl {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceControl() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Edition.ResourceControl", typeof(ResourceControl).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на $(function intAccordion() { var userpanel = $(&quot;##acc#&quot;); var index = $.cookie(&quot;##acc#&quot;); var active = 0; if (index !== undefined) { active = userpanel.find(&quot;h2:eq(&quot; + index + &quot;)&quot;); } if (index == null) active = 0;$(&quot;##acc#&quot;).accordion({ header: &quot;h2&quot;, autoHeight: false, active: active, change: function (event, ui) { var i = $(this).find(&quot;h2&quot;).index(ui.newHeader[0]); $.cookie(&quot;##acc#&quot;, i, { path: &quot;/&quot; }); } });});.
        /// </summary>
        internal static string accordionCore {
            get {
                return ResourceManager.GetString("accordionCore", resourceCulture);
            }
        }
        
       
        internal static string Base {
            get {
                return ResourceManager.GetString("Base", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на  /*Code Compressed with JS Code Compressor v.1.0.3 - http://www.sarmal.com/*/function submitValidateRegExp(obj,regs){if (!new RegExp(regs,&apos;g&apos;).test($(obj).val())){$(obj).css(&apos;background-color&apos;,&apos;#FF0000&apos;);return 1;}else{return 0;}}function submitValidateIsEmpty(obj,isNull,strlength){var str = $(obj).val();if (strlength &lt;= 0)strlength = 4000000000000000;if (str.length &gt; strlength){alert(&apos;Big amount symbol!&apos;);$(obj).css(&apos;background-color&apos;,&apos;#FF0000&apos;);return 1;}if (isNull){if ($.trim(str).length == 0){$(obj).css [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string Base_compressed {
            get {
                return ResourceManager.GetString("Base_compressed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на $(function () {$(&quot;input:file&quot;).parents().each(function () { if ($(this).is($(&apos;form&apos;))) {$(this).attr(&apos;enctype&apos;, &apos;multipart/form-data&apos;);}});});.
        /// </summary>
        internal static string cb {
            get {
                return ResourceManager.GetString("cb", resourceCulture);
            }
        }
        
       
        internal static string datePicker {
            get {
                return ResourceManager.GetString("datePicker", resourceCulture);
            }
        }
        
       
        internal static string loader {
            get {
                return ResourceManager.GetString("loader", resourceCulture);
            }
        }
        
        
        internal static string maska {
            get {
                return ResourceManager.GetString("maska", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на $(function(){$dialog = $(&apos;#editionmodal&apos;).dialog({autoOpen: false, title: &apos;#titl#&apos;, dialogClass: &apos;alert&apos;, modal: true,buttons: [{ text: &quot;#n#&quot;, click: function () { $(this).dialog(&quot;close&quot;); } }, { text: &quot;#y#&quot;, click: function () { $(this).dialog(&quot;close&quot;);#code# $(&quot;##form#&quot;).submit(); } }]});$(&apos;##button#&apos;).bind(&apos;click&apos;, function () {$dialog.dialog(&quot;open&quot;);  return false;});});.
        /// </summary>
        internal static string modalDialog {
            get {
                return ResourceManager.GetString("modalDialog", resourceCulture);
            }
        }
     
        internal static string submitValidate {
            get {
                return ResourceManager.GetString("submitValidate", resourceCulture);
            }
        }
        
     
        internal static string tabsCore {
            get {
                return ResourceManager.GetString("tabsCore", resourceCulture);
            }
        }
        
       
        internal static string tinyMCE {
            get {
                return ResourceManager.GetString("tinyMCE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на $(function load#name#(){tynyUp(&apos;#name#&apos;);});.
        /// </summary>
        internal static string updateModel {
            get {
                return ResourceManager.GetString("updateModel", resourceCulture);
            }
        }
        
       
        internal static string valudateCore {
            get {
                return ResourceManager.GetString("valudateCore", resourceCulture);
            }
        }
    }
}