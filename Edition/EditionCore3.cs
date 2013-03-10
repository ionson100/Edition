using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Edition.Attribute;

namespace Edition
{
    public partial class EditionControl
    {
        /// <summary>
        ///Getting the object of request
        /// </summary>
        /// <param name="httpContextBase"></param>
        /// <returns></returns>
        public static object GetObject(this  HttpContextBase httpContextBase)
          {

              var baseS=httpContextBase.Request;
              var unvalidForms = new FormCollection(baseS.Unvalidated().Form);
              var formf = baseS.Form;
              var formFile = baseS.Files;
              var strType = formf.Get(NameTypeField);
              var strAssebly = formf.Get(NameAcssemblyField);
              var obj = Activator.CreateInstance(strAssebly, strType).Unwrap();
              var rr = GetCustomAttributesEditionBaseAttribute(obj);
              if (rr != null)
              {
                  var eBaseAttributes = rr as EBaseAttribute[] ?? rr.ToArray(); 
                  foreach (var ba in eBaseAttributes)
                  {
                          try
                          {

                      
                          var pr = ba.PropertyInfo;
                          if (!ba.IsNotReadOnly) continue;
                          var str = ba.EDataType!=null&&(ba.EDataType.DataType== EDataType.TinyMceModal||ba.EDataType.DataType== EDataType.TinyMceClassic)
                                        ? unvalidForms.Get(ba.PropertyInfo.Name)
                                        : formf.Get(ba.PropertyInfo.Name);

                      
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
                              if (str == "true,false") str = "true";
                              var o = parse.Invoke(pr, new object[] { str });
                              pr.SetValue(obj, o, null);
                          }
                          else
                              pr.SetValue(obj, null, null);
                      }
                      catch (Exception ex)
                      {
                          var v = new ErrorValidate {ErrorMessage = ex.InnerException.Message, PropertyInfo = ba.PropertyInfo};
                         httpContextBase.AddError(v);
                      }
                  }

                  //////////////////////////////////////////////////////////
                  if (baseS.Files != null)
                  {
                     
                      foreach (var ba in eBaseAttributes)
                      {
                          try
                          {
                              var pr = obj.GetType().GetProperty(ba.PropertyInfo.Name);
                              var file = formFile.Get(ba.PropertyName);
                              if (pr.PropertyType == typeof(HttpPostedFileBase))
                              {
                                  if (file != null && file.ContentLength != 0)
                                  {
                                      pr.SetValue(obj, file, null);
                                  }
                              }

                          }
                          catch (Exception ex)
                          {

                              var v = new ErrorValidate { ErrorMessage = ex.Message, PropertyInfo = ba.PropertyInfo };
                              httpContextBase.AddError(v);
                          }
                         
                      }
                  }
              }
              return obj;
          }


        internal static void AddToolTip(WebControl con, EBaseAttribute items, StringBuilder sbToolTipJs)
        {
           if(string.IsNullOrEmpty(items.ToolTip))return;

           con.Attributes.Add("title", items.ToolTip);
            sbToolTipJs.AppendFormat(
                "$('#{0}').tooltip({{ position: \"center right\",offset: [-2, 10],effect: \"fade\",opacity: 0.7}});",
              items.PropertyName);
 
        }




        private static void AddControl(Control bases, Control add, string label)
        {
            using (var literal = new Literal { Text = string.Format(@"<Label class='editionlabel' >{0}</Label> <br/>", label) })
            {
                bases.Controls.Add(literal);
                if (add != null)
                {
                    bases.Controls.Add(add);
                }
        
            }


        }

       
        internal static IEnumerable<EBaseAttribute> GetCustomAttributesEditionBaseAttribute(object obj)
          {
              var listObj = new List<EBaseAttribute>();
              var pr = obj.GetType().GetProperties();
              foreach (var item in pr)
              {
                  if (item.GetCustomAttributes(typeof(EBaseAttribute), true).Any())
                  {

                      var ff = (EBaseAttribute)item.GetCustomAttributes(typeof(EBaseAttribute), true).First();
                      ff.PropertyInfo = item;

                      listObj.Add(ff);
                    
                      if (item.GetCustomAttributes(typeof(ScriptIgnoreAttribute), true).Any())
                      {
                          ff.IsNotAddJson = true;
                      }

                      if (item.GetCustomAttributes(typeof(EStringFormatAttribute), true).Any())
                      {
                          var val =
                              (EStringFormatAttribute)
                              item.GetCustomAttributes(typeof(EStringFormatAttribute), true).First();
                          ff.StringFormat = val.Format;
                          ff.Culture = val.Culture;
                      }





                      if(item.GetCustomAttributes(typeof(RegularExpressionAttribute),true).Any())
                      {
                          if(ff.BaseValidateMvc==null)ff.BaseValidateMvc=new BaseValidateMvc();
                          ff.BaseValidateMvc.RegularExpression =
                              (RegularExpressionAttribute)
                              item.GetCustomAttributes(typeof (RegularExpressionAttribute), true).First();
                      }
                      if (item.GetCustomAttributes(typeof(RequiredAttribute), true).Any())
                      {
                          if (ff.BaseValidateMvc == null) ff.BaseValidateMvc = new BaseValidateMvc();
                          ff.BaseValidateMvc.RequiredAttribute =
                              (RequiredAttribute)
                              item.GetCustomAttributes(typeof(RequiredAttribute), true).First();
                      }
                      if (item.GetCustomAttributes(typeof(RangeAttribute), true).Any())
                      {
                          if (ff.BaseValidateMvc == null) ff.BaseValidateMvc = new BaseValidateMvc();
                          ff.BaseValidateMvc.RangeAttribute =
                              (RangeAttribute)
                              item.GetCustomAttributes(typeof(RangeAttribute), true).First();
                      }

                      if (item.GetCustomAttributes(typeof(StringLengthAttribute), true).Any())
                      {
                          if (ff.BaseValidateMvc == null) ff.BaseValidateMvc = new BaseValidateMvc();
                          ff.BaseValidateMvc.StringLengthAttribute =
                              (StringLengthAttribute)
                              item.GetCustomAttributes(typeof(StringLengthAttribute), true).First();
                      }




                      if (item.GetCustomAttributes(typeof(DataTypeAttribute), true).Any())
                      {
                         
                          ff.DataType =
                              (DataTypeAttribute)
                              item.GetCustomAttributes(typeof(DataTypeAttribute), true).First();
                      }
                      if (item.GetCustomAttributes(typeof(EDataTypeAttribute), true).Any())
                      {
                        
                          ff.EDataType =
                              (EDataTypeAttribute)
                              item.GetCustomAttributes(typeof(EDataTypeAttribute), true).First();
                      }
                        if (item.GetCustomAttributes(typeof(DisplayNameAttribute), true).Any())
                      {
                        
                          ff.DisplayName =
                              ((DisplayNameAttribute)
                              item.GetCustomAttributes(typeof(DisplayNameAttribute), true).First()).DisplayName;
                      }

                        if (item.GetCustomAttributes(typeof(ReadOnlyAttribute), true).Any())
                        {

                            ff.IsNotReadOnly =
                                ((ReadOnlyAttribute)
                                 item.GetCustomAttributes(typeof (ReadOnlyAttribute), true).First()).IsReadOnly;
                        }

                    
                    
                  }
              }
              return listObj;
          }

       
        sealed class PageEditor : Page
          {

              public PageEditor()
              {
                  EnableEventValidation = false;
                  ClientIDMode = ClientIDMode.Static;
              }

              public override void VerifyRenderingInServerForm(Control control)
              {
              }
          }


        /// <summary>
        /// Checking the objec 
        /// </summary>
        /// <param name="basecon">expansion</param>
        /// <param name="obj">The object to validate</param>
        /// <returns></returns>
        public static bool ValidateModel(this HttpContextBase basecon,object obj)
        {
            if (obj == null) return false;
            var pr = obj.GetType().GetProperties();
            foreach (var propertyInfo in pr)
            {
                var d = propertyInfo.GetCustomAttributes(typeof(EBaseAttribute), true);
                if (d.Length == 0) continue;

                var value = propertyInfo.GetValue(obj, null);

                if (propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true).Any())
                {
                    var re = propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true);
                    foreach (ValidationAttribute o in re)
                    {
                        if (!o.IsValid(value))
                        {
                            var e = new ErrorValidate
                                                  {
                                                      ErrorMessage = o.ErrorMessage,
                                                      PropertyInfo = propertyInfo
                                                  };

                            basecon.AddError(e);
                        }
                    }
                  
                }
                if (propertyInfo.GetCustomAttributes(typeof(EDataTypeAttribute), true).Any())
                {
                    var ff = (EDataTypeAttribute)propertyInfo.GetCustomAttributes(typeof(EDataTypeAttribute), true).First();
                    if(ff.DataType== EDataType.TextBoxRegExEmail)
                    {
                        var o = new RegularExpressionAttribute(RegExEmail) { ErrorMessage = @"Not real Email" };
                        if (!o.IsValid(value))
                        {
                            var e = new ErrorValidate
                            {
                                ErrorMessage = o.ErrorMessage,
                            
                                PropertyInfo = propertyInfo
                            };

                            basecon.AddError(e);
                            continue;

                        }
                    }
                    if (ff.DataType == EDataType.TextBoxRegExpDouble)
                    {
                        var o = new RegularExpressionAttribute(RegExDouble) { ErrorMessage = @"Not real Double" };
                        if (!o.IsValid(value))
                        {
                            var e = new ErrorValidate
                            {
                                ErrorMessage = o.ErrorMessage,
                                PropertyInfo = propertyInfo
                            };

                            basecon.AddError(e);
                            continue;

                        }
                    }
                    if (ff.DataType == EDataType.TextBoxRegExpNumber)
                    {
                        var o = new RegularExpressionAttribute(RegExNumber) { ErrorMessage = @"Not real Number" };
                        if (!o.IsValid(value))
                        {
                            var e = new ErrorValidate
                            {
                                ErrorMessage = o.ErrorMessage,
                                PropertyInfo = propertyInfo
                            };

                            basecon.AddError(e);
                        }
                    }

                }
               
            }
           
            return basecon.AllErrors==null||basecon.AllErrors.Length==0;
        }

        internal class ErrorValidate:Exception
        {
            
            public string ErrorMessage { get; set; }
          
            public PropertyInfo PropertyInfo { get; set; }
        }

       
    


       
        internal static object GetValue(object target, EBaseAttribute host)
          {
              if (string.IsNullOrEmpty(host.StringFormat)) return target;

              var p1 = host.PropertyType.GetMethod("ToString", new[] { typeof(string) });
              var p2 = host.PropertyType.GetMethod("ToString", new[] { typeof(string), typeof(IFormatProvider) });
              if (p1 != null)
              {
                  return string.IsNullOrEmpty(host.Culture) ? p1.Invoke(target, new object[] { host.StringFormat }) :
                      p2.Invoke(target, new object[] { host.StringFormat, CultureInfo.CreateSpecificCulture(host.Culture) });
              }


              return target;
          }
    }
}
