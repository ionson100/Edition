using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Edition.Accordion;
using Edition.Attribute;

using Image = System.Web.UI.WebControls.Image;


namespace Edition
{
  /// <summary>
    ///Static class for HtmlHelper extension
  /// </summary>

    public static partial  class EditionControl
  {
      internal const string RegExEmail = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}";
      internal const string RegExDouble = @"^-?(\d[,.]{0,1})+$";
      internal const string RegExNumber = @"^\d+$";

        /// <summary>
      /// The format for the time display
        /// </summary>
        public static string DateTimeFormat = "dd.MM.yyyy hh:mm:ss";
        /// <summary>
        /// ID hidden field type object being edited
        /// </summary>
        public  const string NameTypeField = "TypeModel_312312";
        /// <summary>
        /// ID hidden field type object being edited
        /// </summary>
        public const string NameAcssemblyField = "AcssemblyModel_312312";
       

        internal static string GetTypeModel(Object obj)
        {
            return String.Format("<input id=\"{1}\" type=\"hidden\" value=\"{0}\" name=\"{1}\"><input id=\"{2}\" type=\"hidden\" value=\"{3}\" name=\"{2}\">", 
                obj.GetType().FullName, NameTypeField, NameAcssemblyField,obj.GetType().Assembly.FullName);
        }


      
        /// <summary>
        /// The basic method to get the markup html
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="obj">Edit an object that</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <returns>html</returns>
        public static MvcHtmlString Edition(this HtmlHelper helper, Object obj, int width, int height)
        {
            var dd = helper.ViewContext.RequestContext.HttpContext.AllErrors;
            return MvcHtmlString.Create(obj == null ? "" : BaseHeadMethod(obj, width, height, helper.ViewContext.RequestContext.HttpContext,dd));

        }


        /// <summary>
        /// The basic method to get the markup html
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">Edit an object that</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <typeparam name="TModel">type model</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <returns>html</returns>
        public static MvcHtmlString EditionFor<TModel, T>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, T>> expression, int width, int height)
        {
            var dd = htmlHelper.ViewContext.RequestContext.HttpContext.AllErrors;
            var obj = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            return MvcHtmlString.Create(obj == null ? "" : BaseHeadMethod(obj, width, height, htmlHelper.ViewContext.RequestContext.HttpContext, dd));

        }

        internal static string BaseHeadMethod(Object obj, int width, int height, HttpContextBase ex, Exception[] exx)
        {
            var hiddenList = new List<EBaseAttribute>();
            var sbJoson = new StringBuilder();
            var sbToolTipJs = new StringBuilder();
            var sbJs = new StringBuilder();
            var sbJsG = new StringBuilder();
            var sbColrJs = new StringBuilder();
            var sbValidateJs = new StringBuilder();
            var modal = new List<Control>();
            var basePanel = new Panel { ID = "editionDiv" };
            ITabsAccord accordion = new Accordion.Accordion();
            var vv = obj.GetType().GetCustomAttributes(typeof(ETypeShowAttribute), true);


            if (vv.Any() && ((ETypeShowAttribute)vv[0]).Type == TypeShow.Tabs)
            {
                accordion = new Tabs.Tabs();
            }

           


            var ee = Opachki(obj, width, height, modal, accordion, sbJsG, sbColrJs, sbValidateJs, sbToolTipJs, sbJoson,hiddenList,exx);
            basePanel.Controls.Add(ee);
            foreach (var i in modal)
            {
                basePanel.Controls.Add(i);
            }

            if (!string.IsNullOrEmpty(sbToolTipJs.ToString()))
            {
                sbToolTipJs.Insert(0, "$(function () {  if( $('#IsShowToolTip').length ){if($('#IsShowToolTip').val()== 'False') return;}");
                sbToolTipJs.Append("});");
            }

            basePanel.Style.Add("z-index", "0");


            foreach (var ba in hiddenList)
            {
                var er = (EHiddenFieldAttribute)ba;
                var dd = ba.PropertyInfo.GetValue(obj, null);
                var con = new HiddenField { ID = er.PropertyName, Value = dd == null ? "" : dd.ToString() };
                basePanel.Controls.Add(con);
            }


            
            var sw = new StringWriter();
            var tw = new HtmlTextWriter(sw);

            basePanel.RenderControl(tw);
            sbJs.Append("<script type='text/javascript'>");
            sbJs.Append(ResourceControl.Base);
            sbJs.Append("</script>");
            
            if (!string.IsNullOrEmpty(sbJoson.ToString()))
            {
                sbJoson.Insert(0, "<script type=\"text/javascript\"> function GetJSON() { var o = new Object(); ");
                sbJoson.Append("return $.toJSON(o);};");
                //sbJoson.Append(ResourceControl.valudateCore.Replace("#validate#", sbValidateJs.ToString()));
                sbJoson.Append("   </script>");

            }
            var addin=new StringBuilder();
     
            addin.Append(sbToolTipJs);
            addin.Append(accordion.JScript());
            addin.Append(sbJsG);
            addin.Append(sbColrJs);
           
           
            addin.Insert(0,"<script type=\"text/javascript\" > function InitBase(){");
            addin.Append("}$(document).ready(InitBase);</script>");






            ex.ClearError();

           
            return sbJs + GetTypeModel(obj) + addin + sw + ResourceControl.maska +sbJoson;
        }

     

   

       internal static Control Opachki(Object obj,
           int width, 
           int height,
           
           List<Control> modal, 
            ITabsAccord accordion,
           StringBuilder sb,
            StringBuilder sbColor,
           StringBuilder sbValidateJs, 
          
           StringBuilder sbToolTipJs,
           StringBuilder sbJoson,
           List<EBaseAttribute> hiddenList,
           Exception [] ex
           )
        {


            var unitW = width == 0 ? new Unit("100%") : new Unit(width + "px");
            var unitH = height == 0 ? new Unit("100%") : new Unit(height + "px");

            var listAttr = GetCustomAttributesEditionBaseAttribute(obj);
           if (listAttr != null) 
           {
               var eBaseAttributes = listAttr as EBaseAttribute[] ?? listAttr.ToArray();
               foreach (var item in eBaseAttributes.Where(a => a.TypeParent == typeof(EHiddenFieldAttribute)))
               {
                   hiddenList.Add(item); 
               }
               var ll = eBaseAttributes.Where(a=>a.DisplayNameItem!=null).OrderBy(a=>a.DisplayNameItem).Select(a => a.DisplayNameItem).Distinct();
            
               foreach (var name in ll)
               {
                   var ia = new ItemAccordion { HeaderText = name };
                   var name1 = name;
                   var lion = eBaseAttributes.Where(a => a.DisplayNameItem == name1).ToList();

                   var baseaccodeon = new List<List<EBaseAttribute>>();
                   var listA = lion.Where(a => a.SortIndex <= 20).ToList();
                   baseaccodeon.Add(listA);
                   var listB = lion.Where(a => a.SortIndex > 20 && a.SortIndex <= 40).ToList();
                   baseaccodeon.Add(listB);
                   var listC = lion.Where(a => a.SortIndex > 40 && a.SortIndex <= 60).ToList();
                   baseaccodeon.Add(listC);
                   var listD = lion.Where(a => a.SortIndex > 60 && a.SortIndex <= 80).ToList();
                   baseaccodeon.Add(listD);
                 
                   using (var baseTableA = new Table { Width = unitW, Height = unitH })
                   {
                       using (var basetablerowA = new TableRow())
                       {
                          
                           foreach (var list in baseaccodeon)
                           {
                               list.Sort();
                               list.Reverse();
                               using (var basetablecell = new TableCell())
                               {
                                  
                                   
                                   basetablecell.Style.Add("vertical-align","top");
                                 
                                   basetablecell.Controls.Add(RenderTable(list, obj, modal, sb, sbColor, sbValidateJs, sbToolTipJs, sbJoson, hiddenList, ex));
                                       basetablerowA.Cells.Add(basetablecell);
                                       baseTableA.Rows.Add(basetablerowA);
                               }
                           }
                           ia.Control.Add(baseTableA);
                           accordion.Items.Add(ia);
                       }
                   }
               }
           }

           using (var containerTable = new Table())
           {
              
               using (var containerAccordion = new TableRow ())
               {
                   containerAccordion.Style.Add("vertical-align","top");
                 
                       using (var celAc = new TableCell())
                       {
                           var sb1 = new StringBuilder();
                           var stringWriter = new StringWriter(sb1);
                           var htmlTextWriter = new HtmlTextWriter(stringWriter);
                           ((Control)accordion).RenderControl(htmlTextWriter);
                          

                          celAc.Controls.Add(new Literal { Text = Regex.Replace(Regex.Replace(sb1.ToString(), "^<span>", ""), "</span>$", "") });
                           containerAccordion.Cells.Add(celAc);
                           containerTable.Rows.Add(containerAccordion);
                           containerTable.Width = unitW;
                           containerTable.Height = unitH;
                           return containerTable;
                       }
                  
               }
           }
        }

        static void AddJSon(StringBuilder sb ,EBaseAttribute pr)
             {
                 if (pr.IsNotAddJson) return;
                 sb.Append("o." + pr.PropertyName + "=$('#" + pr.PropertyName + "').val();");
                 
             }

       internal static Table RenderTable(IEnumerable<EBaseAttribute> list,
           Object obj, 
            List<Control> modal,
           StringBuilder sb,
           StringBuilder sbColor,
            StringBuilder sbValidateJs,
          
           StringBuilder sbToolTipJs,
           StringBuilder sbJoson,
           List<EBaseAttribute> hiddenList,
            Exception[] ex)
        {
            var table = new Table();
            foreach (var item in list)
            {
                AddJSon(sbJoson, item);
                if (item.PropertyInfo.PropertyType == typeof(Int16) || 
                    item.PropertyInfo.PropertyType == typeof(Int32) ||
                    item.PropertyInfo.PropertyType == typeof(Int64)||
                    item.PropertyInfo.PropertyType == typeof(UInt16) ||
                    item.PropertyInfo.PropertyType == typeof(UInt32) || 
                    item.PropertyInfo.PropertyType == typeof(UInt64))
                {
                    if(item.BaseValidateMvc==null)item.BaseValidateMvc=new BaseValidateMvc();
                    if(item.BaseValidateMvc.RegularExpression==null)
                    {
                        item.BaseValidateMvc.RegularExpression =
                        new RegularExpressionAttribute(RegExNumber) {ErrorMessage = @"No Number format.."};

                    }
                }


                if (item.PropertyInfo.PropertyType == typeof(Double) ||
                   item.PropertyInfo.PropertyType == typeof(float) ||
                   item.PropertyInfo.PropertyType == typeof(decimal))
                {
                    if (item.BaseValidateMvc == null) item.BaseValidateMvc = new BaseValidateMvc();
                    if (item.BaseValidateMvc.RegularExpression == null)
                    {
                        item.BaseValidateMvc.RegularExpression =
                       new RegularExpressionAttribute(RegExDouble) { ErrorMessage = @"No Double format.." };
                    }
                }

                if (item.EDataType != null && item.EDataType.DataType == EDataType.DropDownCore)
                {

                    #region EDataType.DropDownCore
                    var er = item;
                    using (var r = new TableRow())
                    {
                        using (var c = new TableCell())
                        {
                            var dd = obj.GetType().GetProperty(er.PropertyName).GetValue(obj, null);
                            using (var con = new DropDownList { ID = er.PropertyName, Enabled = item.IsNotReadOnly })
                            {
                                var o = Activator.CreateInstance(item.EDataType.DropDownCore);
                                if (o != null)
                                    ((IDropDownCore)o).ListItems.ForEach(a => con.Items.Add(a));

                                var itemddl = con.Items.FindByValue(dd.ToString());
                                if (itemddl != null)
                                    itemddl.Selected = true;

                                AddControl(c, null, er.DisplayName);
                                c.Controls.Add(con);
                                r.Cells.Add(c);
                                table.Rows.Add(r);
                                if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                {
                                    AddValueAttribute(item, con, c, ex);
                                }
                                AddToolTip(con, item, sbToolTipJs);
                                ScripJsonAttribute(con, item, "0");
                                continue;
                            }
                        }
                    }
                    #endregion
                }

                if (item.TypeParent == typeof(EHiddenFieldAttribute))
                {
                    hiddenList.Add(item); continue;
                }
                if (item.TypeParent == typeof(ECustomtAttribute))
                {

                    #region typeof(ECustomtAttribute)
                    var er = (ECustomtAttribute)item;
                    var pr = item.PropertyInfo;

                    using (var r = new TableRow())
                    {
                        using (var c = new TableCell())
                        {
                            using (var page = new PageEditor())
                            {
                               
                               if(!File.Exists(HttpContext.Current.Server.MapPath(er.UrlForControl)))
                                   throw new Exception("no control:" + er.UrlForControl);
                                using (var con = page.LoadControl(er.UrlForControl))
                                {
                                    var dd = pr.GetValue(obj, null);
                                    ((IEdition)con).SetValue = dd;
                                    c.Style.Add("vertical-align","top");
                                    var sw = new StringWriter();
                                    var tw = new HtmlTextWriter(sw);
                                    page.Controls.Add(con);
                                    con.RenderControl(tw);
                                    using (var cc = new Literal { Text = sw.ToString().Replace(con.UniqueID + page.IdSeparator, "") })
                                    {
                                        AddControl(c, cc, er.DisplayName);
                                        r.Cells.Add(c);
                                        table.Rows.Add(r);
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                
             
                    #endregion
                }

                if (item.DataType == null && item.EDataType == null)
                {
                    #region DataType.Null
                    var er = item; var pr = item.PropertyInfo;
                    using (var r = new TableRow())
                    {
                        using (var ctextbox = new TableCell())
                        {
                            var dd = pr.GetValue(obj, null);
                            using (var tb = new TextBox
                            {
                                Enabled = item.IsNotReadOnly,
                                ID = er.PropertyName,
                                Text = dd == null ? "" : GetValue(dd, item).ToString()
                            })
                            {
                                AddControl(ctextbox, tb, er.DisplayName);
                                ctextbox.Style.Add("vertical-align","top");
                                r.Cells.Add(ctextbox);
                                table.Rows.Add(r);
                                if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                {
                                    AddValueAttribute(item, tb, ctextbox, ex);
                                }
                                AddToolTip(tb, item, sbToolTipJs);
                                ScripJsonAttribute(tb, item, "0");
                                continue;
                            }
                        }
                    }
                    #endregion
                }

                if (item.EDataType != null&&item.EDataType.DataType== EDataType.TextBoxRegExEmail)
                {
                    #region DataType.EDataType.TextBoxRegExEmail

                    if (item.BaseValidateMvc == null)item.BaseValidateMvc=new BaseValidateMvc();
                    if (item.BaseValidateMvc.RegularExpression==null)
                     item.BaseValidateMvc.RegularExpression=new RegularExpressionAttribute(RegExEmail){ErrorMessage = @"Not real Email"};
                    var er = item; var pr = item.PropertyInfo;
                    using (var r = new TableRow())
                    {
                        using (var ctextbox = new TableCell())
                        {
                            var dd = pr.GetValue(obj, null);
                            using ( var tb = new TextBox
                            {
                                Enabled = item.IsNotReadOnly,
                                ID = er.PropertyName,
                                Text = dd == null ? "" : GetValue(dd, item).ToString()
                            })
                            {
                                AddControl(ctextbox, tb, er.DisplayName);
                                ctextbox.Style.Add("vertical-align","top");
                                r.Cells.Add(ctextbox);
                                table.Rows.Add(r);
                                if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                {
                                    AddValueAttribute(item, tb, ctextbox, ex);
                                }
                                AddToolTip(tb, item, sbToolTipJs);
                                ScripJsonAttribute(tb, item, "0");
                                continue;
                            }
                        }
                    }
                    #endregion
                }

                if (item.EDataType != null && item.EDataType.DataType == EDataType.TextBoxRegExpNumber)
                {
                    #region DataType.EDataType.TEDataType.TextBoxRegExpNumber

                    if (item.BaseValidateMvc == null) item.BaseValidateMvc = new BaseValidateMvc();
                    if (item.BaseValidateMvc.RegularExpression==null)
                    item.BaseValidateMvc.RegularExpression = new RegularExpressionAttribute(RegExNumber) { ErrorMessage = @"Not real Number" };
                    var er = item; var pr = item.PropertyInfo;
                    using (var r = new TableRow())
                    {
                        using (var ctextbox = new TableCell())
                        {
                            var dd = pr.GetValue(obj, null);
                            using ( var tb = new TextBox
                            {
                                Enabled = item.IsNotReadOnly,
                                ID = er.PropertyName,
                                Text = dd == null ? "" : GetValue(dd, item).ToString()
                            })
                            {
                                AddControl(ctextbox, tb, er.DisplayName);
                                ctextbox.Style.Add("vertical-align","top");
                                r.Cells.Add(ctextbox);
                                AddToolTip(tb, item, sbToolTipJs);
                                table.Rows.Add(r);
                                if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                {
                                    AddValueAttribute(item, tb, ctextbox, ex);
                                }
                                ScripJsonAttribute(tb, item, "0");
                                continue;
                            }
                        }
                    }
                    #endregion
                }

                if (item.EDataType != null && item.EDataType.DataType == EDataType.TextBoxRegExpDouble)
                {
                    #region EDataType.TextBoxRegExpDouble

                    if (item.BaseValidateMvc == null) item.BaseValidateMvc = new BaseValidateMvc();
                    if(item.BaseValidateMvc.RegularExpression==null)
                    item.BaseValidateMvc.RegularExpression = new RegularExpressionAttribute(RegExDouble) { ErrorMessage = @"Not real string as Double" };
                    var er = item; var pr = item.PropertyInfo;
                    using (var r = new TableRow())
                    {
                        using (var ctextbox = new TableCell())
                        {
                            var dd = pr.GetValue(obj, null);
                            using ( var tb = new TextBox
                            {
                                Enabled = item.IsNotReadOnly,
                                ID = er.PropertyName,
                                Text = dd == null ? "" : GetValue(dd, item).ToString()
                            })
                            {
                                AddControl(ctextbox, tb, er.DisplayName);
                                ctextbox.Style.Add("vertical-align","top");
                                r.Cells.Add(ctextbox);
                                table.Rows.Add(r);
                                if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                {
                                    AddValueAttribute(item, tb, ctextbox, ex);
                                }
                                AddToolTip(tb, item, sbToolTipJs);
                                ScripJsonAttribute(tb, item, "0");
                                continue;
                            }
                        }
                    }
                    #endregion
                }

                if (item.DataType != null && item.DataType.DataType == DataType.Currency)
                {
                    #region DataType.Currency
                    var er = item; var pr = item.PropertyInfo;
                    using (var r = new TableRow())
                    {
                        using (var ctextbox = new TableCell ())
                        {
                            ctextbox.Style.Add("vertical-align","top");
                            var dd = pr.GetValue(obj, null);
                            if (dd is Decimal)
                            {
                                using (var label = new Label { Text = ((decimal)dd).ToString("C") })
                                {
                                    AddControl(ctextbox, label, er.DisplayName);
                                    r.Cells.Add(ctextbox);
                                    table.Rows.Add(r);
                                    continue;
                                }
                            }
                            using (var tb = new TextBox
                            {
                                Enabled = item.IsNotReadOnly,
                                ID = er.PropertyName,
                                Text = dd == null ? "" : GetValue(dd, item).ToString()
                            })
                            {
                                AddControl(ctextbox, tb, er.DisplayName);
                                r.Cells.Add(ctextbox);
                                table.Rows.Add(r);
                                if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                {
                                    AddValueAttribute(item, tb, ctextbox, ex);
                                }
                                AddToolTip(tb, item, sbToolTipJs);
                                ScripJsonAttribute(tb, item, "0");
                                continue;
                            }
                        }
                    }
                    #endregion
                }
                if (item.DataType != null&&item.DataType.DataType == DataType.Password)
                {
                    #region DataType.Password
                    var er = item; var pr = item.PropertyInfo;
                    using ( var r = new TableRow())
                    {
                        using (var ctextbox = new TableCell())
                        {
                            ctextbox.Style.Add("vertical-align","top");
                            var dd = pr.GetValue(obj, null);
                            using ( var tb = new TextBox
                            {
                                TextMode = TextBoxMode.Password,
                                Enabled = item.IsNotReadOnly,
                                ID = er.PropertyName,
                                Text = dd == null ? "" : GetValue(dd, item).ToString()
                            })
                            {
                                AddControl(ctextbox, tb, er.DisplayName);

                                r.Cells.Add(ctextbox);
                                table.Rows.Add(r);
                                if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                {
                                    AddValueAttribute(item, tb, ctextbox, ex);
                                }
                                AddToolTip(tb, item, sbToolTipJs);
                                ScripJsonAttribute(tb, item, "0");
                                continue;
                            }
                        }
                    }
                   #endregion
                }
                if (item.DataType != null&&item.DataType.DataType == DataType.MultilineText)
                {
                    #region DataType.MultilineText
                    using (var r = new TableRow())
                    {
                        using (var ctextbox = new TableCell() )
                        {
                            ctextbox.Style.Add("vertical-align","top");
                            var dd = item.PropertyInfo.GetValue(obj, null);
                            using ( var tb = new TextBox
                            {
                                Enabled = item.IsNotReadOnly,
                                ID = item.PropertyName,
                                TextMode = TextBoxMode.MultiLine,
                                Text = dd == null ? "" : dd.ToString()
                            })
                            {
                                AddControl(ctextbox, tb, item.DisplayName);
                                r.Cells.Add(ctextbox);
                                table.Rows.Add(r);
                                if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                {
                                    AddValueAttribute(item, tb, ctextbox, ex);
                                }
                                AddToolTip(tb, item, sbToolTipJs);
                                ScripJsonAttribute(tb, item, "0");
                                continue;
                            }
                        }
                    }
#endregion
                }
                if (item.EDataType!=null&&item.EDataType.DataType== EDataType.TinyMceClassic)
                {
                    #region EDataType.TynyMceClassic
                    var er = item;
                    var pr = item.PropertyInfo;
                    using ( var t = new Table())
                    {
                        using (var r = new TableRow())
                        {
                            using ( var ctextbox = new TableCell())
                            {
                                var dd = pr.GetValue(obj, null);
                                using (var tb = new TextBox
                                {
                                    Text = dd == null ? "" : dd.ToString(),
                                    ID = er.PropertyName,
                                    TextMode = TextBoxMode.MultiLine
                                })
                                {
                                    AddControl(ctextbox, tb, er.DisplayName);
                                    ctextbox.Style.Add("vertical-align","top");
                                    r.Cells.Add(ctextbox);
                                        t.Rows.Add(r);
                                        using (var row = new TableRow())
                                        {
                                            using (var cel = new TableCell())
                                            {
                                                cel.Controls.Add(t);
                                                row.Cells.Add(cel);
                                                table.Rows.Add(row);
                                                if (true)
                                                {
                                                    using ( var res = new TableRow())
                                                    {
                                                        using (var cres = new TableCell())
                                                        {
                                                            using (var panel = new Panel { ID = er.PropertyName + "_res" })
                                                            {
                                                                if (dd != null) panel.Controls.Add(new Literal { Text = dd.ToString() });
                                                                cres.Controls.Add(panel);
                                                                res.Cells.Add(cres);
                                                                table.Rows.Add(res);
                                                            }
                                                        }
                                                    }
                                                    sb.Append(ResourceControl.updateModel.Replace("#name#", er.PropertyName));
                                                    if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                                    {
                                                        AddValueAttribute(item, tb, ctextbox, ex);
                                                    }
                                                    AddToolTip(tb, item, sbToolTipJs);
                                                    ScripJsonAttribute(tb, item, "1");
                                                    continue;
                                                    }
                                            }
                                        }
                                }
                            }
                        }
                    }
#endregion
                }
                if (item.EDataType!=null&&item.EDataType.DataType== EDataType.TinyMceModal)
                {
                    #region EDataType.TynyMceModal
                    var er = item;
                    using (var t = new Table())
                    {
                        table.BorderStyle = BorderStyle.None;
                        using (var rowbase = new TableRow())
                        {
                            using (var celbase = new TableCell())
                            {
                                t.Rows.Add(rowbase);
                                rowbase.Cells.Add(celbase);
                                using (var panelBase = new Panel())
                                {
                                    using (var dragDrop = new Panel())
                                    {
                                        using (var liter = new Literal { Text = string.Format("<span style=\"color: #FFFFFF;\"><strong>{0}</strong></span>", er.DisplayName) })
                                        {
                                            dragDrop.Controls.Add(liter);
                                            dragDrop.CssClass = "dragHeadDiv";
                                            dragDrop.Height = new Unit("20px");
                                            dragDrop.Width = new Unit("100%");
                                            dragDrop.BackColor = Color.FromArgb(255, 49, 89, 238);
                                            dragDrop.ID = er.PropertyName + "_darag";
                                            dragDrop.Style.Add("text-align", "center");

                                            panelBase.Controls.Add(dragDrop);

                                            panelBase.Style.Add("z-index", "10000");
                                            panelBase.Style.Add(HtmlTextWriterStyle.Display, "none");
                                            panelBase.BorderStyle = BorderStyle.Solid;
                                            panelBase.BorderWidth = 1;
                                            panelBase.CssClass = "editionModal";
                                            celbase.Controls.Add(panelBase);
                                            panelBase.ID = er.PropertyName + "_panel";
                                            panelBase.BackColor = Color.FromArgb(255, 192, 223, 249);
                                            modal.Add(t);
                                            using (var tt = new Table())
                                            {
                                                using (var cell = new TableCell())
                                                {
                                                    using ( var roww = new TableRow())
                                                    {
                                                        roww.Cells.Add(cell);
                                                        var dd = item.PropertyInfo.GetValue(obj, null);
                                                        using (  var tb = new TextBox
                                                        {
                                                            Text = dd == null ? string.Empty : Convert.ToString(dd),
                                                            ID = string.Format("{0}_edit", er.PropertyName),
                                                            TextMode = TextBoxMode.MultiLine
                                                        })
                                                        {
                                                            using ( var buttonrow = new TableRow())
                                                            {
                                                                using (var butt1 = new TableCell() )
                                                                {
                                                                    buttonrow.Cells.Add(butt1);

                                                                    using ( var b11 = new Button
                                                                    {
                                                                        Text = @"Cancel",
                                                                        CssClass = "buttonModalTinyMce",
                                                                        OnClientClick = string.Format("return cancelShowPanel('{0}')", er.PropertyName)
                                                                    })
                                                                    {
                                                                        using (var b12 = new Button
                                                                        {
                                                                            Text = @"Save",
                                                                            CssClass = "buttonModalTinyMce",
                                                                            OnClientClick =
                                                                                string.Format("return closePanel('{0}')", er.PropertyName)


                                                                        })
                                                                        {
                                                                            b11.Style.Add("margin-right", "30px");
                                                                            b11.Style.Add("margin-left", "30px");
                                                                            b12.Style.Add("margin-right", "30px");
                                                                            b12.Style.Add("margin-left", "30px");
                                                                            butt1.Controls.Add(b11);
                                                                            butt1.Controls.Add(b12);
                                                                            butt1.Style.Add("text-align","right");
                                                                           // butt1.HorizontalAlign = HorizontalAlign.Right;
                                                                            tt.Rows.Add(roww);
                                                                            tt.Rows.Add(buttonrow);
                                                                            cell.Controls.Add(tb);
                                                                            panelBase.Controls.Add(tt);
                                                                            using (var t1 = new Table())
                                                                            {
                                                                                using ( var r = new TableRow())
                                                                                {
                                                                                    using (var ctextbox = new TableCell())
                                                                                    {
                                                                                        var dd1 = item.PropertyInfo.GetValue(obj, null);
                                                                                        using (var tb1 = new TextBox
                                                                                        {
                                                                                            Text = dd1 == null ? "" : dd1.ToString(),
                                                                                            ID = er.PropertyName,
                                                                                            TextMode = TextBoxMode.MultiLine
                                                                                        })
                                                                                        {
                                                                                            AddControl(ctextbox, tb1, er.DisplayName);
                                                                                            using (var litt=new Literal { Text = @"<br/>" })
                                                                                            {
                                                                                                ctextbox.Controls.Add(litt);
                                                                                                ctextbox.Style.Add("vertical-align","top");
                                                                                                using (var bb = new Button
                                                                                                {
                                                                                                    Text = @"Edit",
                                                                                                    CssClass = "buttonModalTinyMce",
                                                                                                    OnClientClick =
                                                                                                        string.Format("return showPanel('{0}')", er.PropertyName)
                                                                                                })
                                                                                                {
                                                                                                    ctextbox.Controls.Add(bb);
                                                                                                    r.Cells.Add(ctextbox);
                                                                                                        t1.Rows.Add(r);
                                                                                                        using (var row = new TableRow())
                                                                                                        {
                                                                                                            using ( var cel = new TableCell())
                                                                                                            {
                                                                                                                cel.Controls.Add(t1);
                                                                                                                row.Cells.Add(cel);
                                                                                                                table.Rows.Add(row);


                                                                                                                using (var res = new TableRow())
                                                                                                                {
                                                                                                                    using (var cres = new TableCell() )
                                                                                                                    {
                                                                                                                        using ( var panel = new Panel { ID = er.PropertyName + "_res" })
                                                                                                                        {
                                                                                                                            if (dd != null) panel.Controls.Add(new Literal { Text = dd.ToString() });
                                                                                                                            cres.Controls.Add(panel);
                                                                                                                            res.Cells.Add(cres);
                                                                                                                            table.Rows.Add(res);
                                                                                                                            sb.Append(ResourceControl.updateModel.Replace("#name#", er.PropertyName + "_edit"));
                                                                                                                            if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                                                                                                            {
                                                                                                                                AddValueAttribute(item, tb1, ctextbox, ex);
                                                                                                                            }
                                                                                                                            AddToolTip(tb1, item, sbToolTipJs);
                                                                                                                            ScripJsonAttribute(tb1, item, "1");
                                                                                                                            continue;
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
#endregion
                }
               
               

                if (item.DataType != null&&item.DataType.DataType == DataType.DateTime)
                {
                    #region DataType.DateTime

                    var er = item;
                    using (var r = new TableRow())
                    {
                        using (var c = new TableCell())
                        {
                            var dd = item.PropertyInfo.GetValue(obj, null);
                            using (var panelBase = new Panel { Enabled = item.IsNotReadOnly })
                            {
                                panelBase.Controls.Add(new Literal
                            {
                                Text = ResourceControl.datePicker.
                                    Replace("#titl#", er.DisplayName).
                                    Replace("#json#", !item.IsNotAddJson ? "data-json=\"0\"" : "").
                                    Replace("#12#", er.PropertyName).
                                    Replace("#hh#", string.IsNullOrEmpty(item.ToolTip) ? "" : "title =\"" + item.ToolTip + "\"").
                                    Replace("#13#", "dd.MM.yyyy hh:mm:ss").
                                    Replace("#date#", ((DateTime)dd).ToString(DateTimeFormat))
                            });
                                c.Controls.Add(panelBase);
                                r.Cells.Add(c);
                                table.Rows.Add(r);
                                if (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo))
                                {
                                    using (var textb = new TextBox())
                                    {
                                        AddValueAttribute(item, textb, panelBase, ex);
                                    }
                                }
                                continue;
                            }
                        }
                    }
                    #endregion
                }

                if (item.DataType != null&&item.DataType.DataType == DataType.Date)
                {
                    #region DataType.Date
                    var er = item;
                    using ( var r = new TableRow())
                    {
                        using (var c = new TableCell { Enabled = item.IsNotReadOnly })
                        {
                            var dd = item.PropertyInfo.GetValue(obj, null);
                            using ( var panelBase = new Panel())
                            {
                                using ( var tb = new TextBox { ID = er.PropertyName, Text = ((DateTime)dd).ToString("dd.MM.yyyy") })
                                {
                                       tb.Attributes.Add("onclick", string.Format("showDatepicker2('{0}','{1}');return false;", er.PropertyName, DateTimeFormat));
                                        panelBase.Controls.Add(tb);
                                    using ( var panCal = new Panel { ID = er.PropertyName + "_calendar" })
                                    {
                                        panCal.Style.Add("position", "absolute");

                                        panelBase.Controls.Add(panCal);
                                        AddControl(c, panelBase, er.DisplayName);
                                        r.Cells.Add(c);
                                        table.Rows.Add(r);
                                        if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                        {
                                            AddValueAttribute(item, tb, panelBase, ex);
                                        }
                                        AddToolTip(tb, item, sbToolTipJs);
                                        ScripJsonAttribute(tb, item, "0");
                                        continue;
                                    }
                                }
                            }
                        }
                    }
#endregion
                }

                if (item.EDataType != null && item.EDataType.DataType == EDataType.Color)
                {

                    
                    #region EDataType.Color
                    var er = item;
                    var dd = item.PropertyInfo.GetValue(obj, null);

                    string strdd;
                    if(dd is Color)
                    {
                        strdd = ColorTranslator.ToHtml((Color)dd);
                    }
                    else
                    {
                        var s = dd as string; 
                        if(s != null)
                        {
                            strdd =s;
                        }
                        else
                        {
                            strdd = null;
                        }
                    }


                    if (string.IsNullOrEmpty(strdd))
                        strdd = "#fefef6";
                    
                    using (var r = new TableRow())
                    {
                       
                        using (var c = new TableCell { Enabled = item.IsNotReadOnly })
                        {
                          
                            using (var button = new Button
                                  {

                                      ID = er.PropertyName + "_btcolor",
                                      Text = @"..",
                                      CssClass = "buttonModalTinyMce",
                                      OnClientClick =
                                          string.Format("ClickColorDp('{0}');return false", er.PropertyName),

                                  })
                            {
                                using (var tb = new TextBox { ID = er.PropertyName, Text = strdd })
                                {
                                    AddControl(c, tb, er.DisplayName);
                                    c.Controls.Add(button);
                                    r.Cells.Add(c);
                                    using (var literal = new Literal { Text = string.Format(@"<div id='{0}_divc'  style='z-index: 10000; border: 1px solid #003399; position: absolute; display: none; background-color: #ECF1FF;'></div>", er.PropertyName) })
                                    {
                                        c.Controls.Add(literal);
                                        sbColor.AppendFormat("$('#{0}_divc').farbtastic('#{0}');", er.PropertyName);
                                        table.Rows.Add(r);
                                        if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                        {
                                            AddValueAttribute(item, tb, c, ex);
                                        }
                                        AddToolTip(tb, item, sbToolTipJs);
                                        ScripJsonAttribute(tb, item, "0");
                                        continue;
                                    }
                                }
                            }
                        }
                    }
#endregion
                }
                if (item.EDataType!=null&&item.EDataType.DataType== EDataType.DropDownAsBool)
                {
                    #region typeof(EDropDownAttribute)
                    var er = item;
                    using (var r = new TableRow())
                    {
                        using (var c = new TableCell())
                        {
                            using ( var con = new DropDownList { ID = er.PropertyName, Enabled = item.IsNotReadOnly })
                            {
                                var dd = item.PropertyInfo.GetValue(obj, null);

                                var ddd = Nullable.GetUnderlyingType(item.PropertyInfo.PropertyType);

                                if (ddd != null && ddd.UnderlyingSystemType == typeof(Boolean))
                                {
                                    con.Items.Add(new ListItem(er.EDataType.DisplayTextAsTrue, true.ToString()));
                                    con.Items.Add(new ListItem(er.EDataType.DisplayTextAsFalse, false.ToString()));
                                    con.Items.Add(new ListItem(er.EDataType.DisplayTextasEmpty, ""));


                                    var item1 = con.Items.FindByValue(dd == null ? string.Empty : dd.ToString());
                                    if (item1 != null)
                                    {
                                        item1.Selected = true;
                                    }
                                }

                                else if (ddd != null && (ddd.UnderlyingSystemType == typeof(Int32) || ddd.UnderlyingSystemType == typeof(Int16) ||
                                    ddd.UnderlyingSystemType == typeof(Int64) || ddd.UnderlyingSystemType == typeof(UInt32) || ddd.UnderlyingSystemType == typeof(UInt16)
                                    || ddd.UnderlyingSystemType == typeof(UInt64)))
                                {
                                    con.Items.Add(new ListItem(er.EDataType.DisplayTextAsTrue, "1"));
                                    con.Items.Add(new ListItem(er.EDataType.DisplayTextAsFalse, "0"));
                                    con.Items.Add(new ListItem(er.EDataType.DisplayTextasEmpty, ""));


                                    var item1 = con.Items.FindByValue(dd == null ? "" : dd.ToString());
                                    if (item1 != null)
                                    {
                                        item1.Selected = true;
                                    }
                                }

                                else if (dd is Boolean)
                                {
                                    con.Items.Add(new ListItem(er.EDataType.DisplayTextAsTrue, true.ToString()));
                                    con.Items.Add(new ListItem(er.EDataType.DisplayTextAsFalse, false.ToString()));
                                    var item1 = con.Items.FindByValue(dd.ToString());
                                    if (item1 != null)
                                    {
                                        item1.Selected = true;
                                    }
                                }
                                else if (dd is Int32 || dd is Int16 || dd is Int64 || dd is UInt32 || dd is UInt64 || dd is UInt16)
                                {
                                    con.Items.Add(new ListItem(er.EDataType.DisplayTextAsTrue, "1"));
                                    con.Items.Add(new ListItem(er.EDataType.DisplayTextAsFalse, "0"));
                                    var item1 = con.Items.FindByValue(dd.ToString());
                                    if (item1 != null)
                                    {
                                        item1.Selected = true;
                                    }
                                }

                                AddControl(c, con, er.DisplayName);
                                r.Cells.Add(c);
                                table.Rows.Add(r);
                                if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                {
                                    AddValueAttribute(item, con, c, ex);
                                }
                                AddToolTip(con, item, sbToolTipJs);
                                ScripJsonAttribute(con, item, "0");
                                continue;
                            }
                        }
                    }
#endregion
                }

                if (item.EDataType!=null&&item.EDataType.DataType== EDataType.CheckBox)
                {
                    #region EDataType.CheckBox
                    var er = item;
                    using (var r = new TableRow())
                    {
                        using (var c = new TableCell())
                        {
                            var dd = (bool)item.PropertyInfo.GetValue(obj, null);
                            using (var con = new TextBox { ID = er.PropertyName, Enabled = item.IsNotReadOnly })
                            {
                                con.Attributes.Add("value", "true");
                                AddToolTip(con, item, sbToolTipJs);
                                ScripJsonAttribute(con, item, "0");
                                if (dd)
                                    con.Attributes.Add("checked", "true");
                                using (var sw = new StringWriter())
                                {
                                    var tw = new HtmlTextWriter(sw);
                                    con.RenderControl(tw);
                                    var tt = sw.ToString().Replace("type=\"text\"", "type=\"checkbox\"");

                                    using (var literal2 = new Literal { Text = tt })
                                    {
                                        AddControl(c, literal2, er.DisplayName);
                                        using (var hf1 = new HiddenField { ID = er.PropertyName +"-cb", Value = "false" })
                                        {
                                            c.Controls.Add(hf1);
                                            r.Cells.Add(c);
                                            table.Rows.Add(r);
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
#endregion
                }

                if (item.EDataType!=null&&item.EDataType.DataType == EDataType.FileUpload)
                {
                    #region EDataType.FileUpload
                    var er = item;
                    using ( var r = new TableRow())
                    {
                        using (var c = new TableCell())
                        {
                            using (var con = new FileUpload { ID = er.PropertyName, Enabled = item.IsNotReadOnly })
                            {
                                AddToolTip(con, item, sbToolTipJs);
                                using (var lit1=new Literal { Text = @"<div style='text-align: center'>" })
                                {
                                    c.Controls.Add(lit1);
                                    using (var imag=new Image { ImageUrl = item.EDataType.UrlImageForFileUpload,AlternateText = "LoadImage"})
                                    {
                                        c.Controls.Add(imag);
                                        using (var lit3 = new Literal { Text = @"<br/>" })
                                        {
                                            c.Controls.Add(lit3);
                                            AddControl(c, con, er.DisplayName);
                                            using (var lit4=new Literal { Text = @"</div>" })
                                            {
                                                c.Controls.Add(lit4);
                                                r.Cells.Add(c);
                                                table.Rows.Add(r);
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
#endregion
                }
              

                if (item.DataType != null&&item.DataType.DataType == DataType.Html)
                {
                    #region DataType.Html
                    var er = item;
                    using ( var r = new TableRow())
                    {
                        using (var c = new TableCell())
                        {
                            var dd = (string)item.PropertyInfo.GetValue(obj, null);
                            using ( var con = new Literal { ID = er.PropertyName, Text = dd })
                            {
                                AddControl(c, null, er.DisplayName);
                                c.Controls.Add(con);
                                r.Cells.Add(c);
                                table.Rows.Add(r);
                                continue;
                            }
                        }
                    }
#endregion
                }

                if (item.EDataType != null && item.EDataType.DataType == EDataType.DropDownForEnum)
                {

                    #region EDataType.DropDownForEnum
                    var er = item;
                    using (var r = new TableRow())
                    {
                        using ( var c = new TableCell())
                        {
                           
                            var dd = obj.GetType().GetProperty(er.PropertyName).GetValue(obj, null);
                            using (var con = new DropDownList { ID = er.PropertyName, Enabled = item.IsNotReadOnly })
                            {
                                foreach (var val in Enum.GetValues(er.PropertyType))
                                {
                                    con.Items.Add(new ListItem(Enum.GetName(er.PropertyType, val)));
                                }
                                var itemddl = con.Items.FindByValue(Enum.GetName(er.PropertyType, dd));
                                if (itemddl != null)
                                    itemddl.Selected = true;
                                AddControl(c, null, er.DisplayName);
                                c.Controls.Add(con);
                                r.Cells.Add(c);
                                table.Rows.Add(r);
                                if (item.BaseValidateMvc != null || (ex != null && ex.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo)))
                                {
                                    AddValueAttribute(item, con, c, ex);
                                }
                                AddToolTip(con, item, sbToolTipJs);
                                ScripJsonAttribute(con, item, "0");
                            }
                        }
                    }

                    #endregion
                }

            }
            
            return table;
        }

       private static void AddValueAttribute<T>(EBaseAttribute item, T tb, Control container,Exception[] exception) where T : WebControl
      {

          if (exception != null)
          {
              if (exception.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo))
                  tb.Attributes.Add("class", "input-validation-error");
          }

          if (item.BaseValidateMvc != null)
          {
              var name = item.DisplayName;

              tb.Attributes.Add("data-val", "true");
              if (item.BaseValidateMvc.RegularExpression != null)
              {
                  tb.Attributes.Add("data-val-regex",string.Format(item.BaseValidateMvc.RegularExpression.ErrorMessage,name));
                  tb.Attributes.Add("data-val-regex-pattern", item.BaseValidateMvc.RegularExpression.Pattern);
              }
              if (item.BaseValidateMvc.RequiredAttribute != null)
              {
                  tb.Attributes.Add("data-val-required",string.Format(item.BaseValidateMvc.RequiredAttribute.ErrorMessage,name));
                  tb.Attributes.Add("data-val-required", string.Format(item.BaseValidateMvc.RequiredAttribute.ErrorMessage,name));
              }
              if (item.BaseValidateMvc.StringLengthAttribute != null)
              {
                  tb.Attributes.Add("data-val-length", string.Format(item.BaseValidateMvc.StringLengthAttribute.ErrorMessage,name));
                  if (item.BaseValidateMvc.StringLengthAttribute.MaximumLength != 0)
                      tb.Attributes.Add("data-val-length-max",
                                        item.BaseValidateMvc.StringLengthAttribute.MaximumLength.ToString(CultureInfo.InvariantCulture));

                  if (item.BaseValidateMvc.StringLengthAttribute.MinimumLength != 0)
                      tb.Attributes.Add("data-val-length-min",
                                        item.BaseValidateMvc.StringLengthAttribute.MinimumLength.ToString(CultureInfo.InvariantCulture));
              }
              if (item.BaseValidateMvc.RangeAttribute != null)
              {
                  tb.Attributes.Add("data-val-number", string.Format(item.BaseValidateMvc.RangeAttribute.ErrorMessage,name));
                  tb.Attributes.Add("data-val-range", string.Format(item.BaseValidateMvc.RangeAttribute.ErrorMessage,name));
                  tb.Attributes.Add("data-val-range-max", item.BaseValidateMvc.RangeAttribute.Maximum.ToString());
                  tb.Attributes.Add("data-val-range-min", item.BaseValidateMvc.RangeAttribute.Minimum.ToString());
              }
          }
           var sp = string.Format("<br/><span class=\"field-validation-valid\" data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", item.PropertyName);


          if (exception != null)
          {
              if (exception.ToList().OfType<ErrorValidate>().Any(a => a.PropertyInfo == item.PropertyInfo))
                  sp = string.Format("<span class=\"field-validation-error\" data-valmsg-replace=\"true\" data-valmsg-for=\"{0}\"><span class=\"\" for=\"{0}\" generated=\"true\">{1}</span></span>", item.PropertyName,
                                    string.Format(exception.ToList().OfType<ErrorValidate>().First(a => a.PropertyInfo == item.PropertyInfo).ErrorMessage,item.DisplayName));
           
           }

           using (var literal=new Literal { Text = sp })
           {
               container.Controls.Add(literal);
           }
         
      }


      private static  void ScripJsonAttribute(WebControl con,EBaseAttribute atr,string isTinyMce)
        {
            if(atr.IsNotAddJson)return;
            con.Attributes.Add("data-json",isTinyMce);
        }


     
  }
}
