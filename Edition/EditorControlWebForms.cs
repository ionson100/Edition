using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Edition.Attribute;

namespace Edition
{
    /// <summary>
    /// A form for editing the object entity
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:EditorControl runat=server></{0}:EditorControl>")]
    internal class EditorControlWebForms : WebControl
    {
        /// <summary>
        /// Width
        /// </summary>
        public new int Width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        public new int Height { get; set; }

        /// <summary>
        /// Object to insert after editing
        /// </summary>
        /// <param name="obj"></param>
        public void SetObject(Object obj)
        {
        }



        /// <summary>
        /// Get object after editing
        /// </summary>
        /// <returns></returns>
        public dynamic GetObject()
        {
            var formf = Context.Request.Form;
            var formFile = Context.Request.Files;
            var strType = formf.Get(EditionControl.NameTypeField);
            var strAssebly = formf.Get(EditionControl.NameAcssemblyField);
            var obj = Activator.CreateInstance(strAssebly, strType).Unwrap();
            var rr = EditionControl.GetCustomAttributesEditionBaseAttribute(obj);
            if (rr != null)
            {
                var eBaseAttributes = rr as EBaseAttribute[] ?? rr.ToArray();
                foreach (var ba in eBaseAttributes)
                {
                    var pr = obj.GetType().GetProperty(ba.PropertyName);
                    if (!ba.IsNotReadOnly) continue;
                    var str = formf.Get(ba.PropertyName);
                    if (str == null) continue;
                    if (pr.PropertyType == typeof(string))
                    {
                        pr.SetValue(obj, str, null);
                        continue;
                    }

                    if (pr.PropertyType == typeof(Color))
                    {
                        pr.SetValue(obj, ColorTranslator.FromHtml(str), null);
                        continue;
                    }
                    if (pr.PropertyType == typeof(HttpPostedFile))
                    {

                        continue;
                    }

                    if (pr.PropertyType.BaseType == typeof(Enum))
                    {
                        pr.SetValue(obj, Enum.Parse(pr.PropertyType, str), null);
                        continue;
                    }
                    var parse = pr.PropertyType.GetMethod("Parse", new[] { typeof(String) }) ??
                                pr.PropertyType.GetProperty("Value").PropertyType.GetMethod("Parse", new[] { typeof(String) });

                    var dd = pr.PropertyType.BaseType == typeof(ValueType) && pr.PropertyType.IsGenericType;
                    if (dd && parse != null)
                    {
                        pr.SetValue(obj, string.IsNullOrEmpty(str) ? null : parse.Invoke(pr, new object[] { str }), null);
                        continue;
                    }

                    if (parse != null)
                    {
                        var o = parse.Invoke(pr, new object[] { str });
                        pr.SetValue(obj, o, null);
                    }
                    else
                        pr.SetValue(obj, null, null);
                }

              

                foreach (var ba in eBaseAttributes)
                {
                    var pr = obj.GetType().GetProperty(ba.PropertyName);
                    var file = formFile.Get(ba.PropertyName);
                    if (pr.PropertyType == typeof(HttpPostedFile))
                    {

                        if (file != null && file.ContentLength != 0)
                        {
                            pr.SetValue(obj, file, null);
                        }
                    }
                }
               
            }
            return obj;
        }

       
    }
}
