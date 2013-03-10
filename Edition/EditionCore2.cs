using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace Edition
{
  public partial  class  EditionControl
    {
      /// <summary>
        /// The loader script on page JSON
      /// </summary>
      /// <param name="helper"></param>
        /// <param name="obj">Object whose JSON to load on the client</param>
      /// <returns></returns>
      public static MvcHtmlString Loader(this HtmlHelper helper, Object obj)
      {
          var ee = new JavaScriptSerializer {MaxJsonLength = 1000000000};
          var sb=new StringBuilder();
          sb.Append("<script type=\"text/ecmascript\" >");
          sb.Append(ResourceControl.loader.Replace("#str#", ee.Serialize(obj)));
          sb.Append("</script>");
         
          return MvcHtmlString.Create(sb.ToString());
      }
    }
}



