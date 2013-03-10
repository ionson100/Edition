using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Edition;
using ExampleEdition.EditionBase;

namespace ExampleEdition.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(int? id)
        {
            ViewBag.Message = "";
            int ID;
            if (id == null || !int.TryParse(id.ToString(), out ID))
            {
                ID = 0;
            }
            ViewData["idd"] = ID;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string json)
        {
            int ID;
            var ee1 = RouteData.Values["id"];
            if (ee1 == null || !int.TryParse(ee1.ToString(), out ID))
            {
                ID = 0;
            }
            ViewData["idd"] = ID;

            if (Request.IsAjaxRequest())
            {

                var strings = Request.Form.GetValues(0);
                if (strings != null)
                {
                    var ddd = strings[0];
                    var ee = GetSerializer<Simpleleg>(234567).Deserialize<Simpleleg>(ddd);
                    var d = GetSerializer<Simpleleg>().Serialize(ee);
                    if (HttpContext.ValidateModel(ee))
                    {
                        Session["simpleleg"] = ee;
                    }
                    return Content(d, "text/xml");
                }
            }



            var ss = HttpContext.GetObject();
            var ty = HttpContext.ValidateModel(ss);
            if (ty)
            {
                Session["simpleleg"] = ss;
            }
            return View();

        }


        public ActionResult About()
        {
            return View();
        }
        public static JavaScriptSerializer GetSerializer<T>() where T : new()
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new ExtendedJavaScriptConverter<T>() });
            return serializer;
        }
        public static JavaScriptSerializer GetSerializer<T>(int maxJsonLength) where T : new()
        {
            var serializer = new JavaScriptSerializer { MaxJsonLength = maxJsonLength };
            serializer.RegisterConverters(new[] { new ExtendedJavaScriptConverter<T>() });
            return serializer;
        }
    }
    public class ExtendedJavaScriptConverter<T> : JavaScriptConverter where T : new()
    {
        private static readonly string DateFormat = Edition.EditionControl.DateTimeFormat;

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new[] { typeof(T) }; }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                           JavaScriptSerializer serializer)
        {
            try
            {
                var p = new T();

                var props = typeof(T).GetProperties();

                foreach (var key in dictionary.Keys)
                {
                    var prop = props.Where(t => t.Name == key).FirstOrDefault();
                    if (prop == null) continue;
                    if (prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(p, dictionary[key], null);
                        continue;
                    }

                    if (prop.PropertyType == typeof(Color))
                    {
                        prop.SetValue(p, ColorTranslator.FromHtml(dictionary[key].ToString()), null);
                        continue;
                    }

                    if (prop.PropertyType.BaseType == typeof(Enum))
                    {
                        prop.SetValue(p, Enum.Parse(prop.PropertyType, dictionary[key].ToString()), null);
                        continue;
                    }
                    var parse = prop.PropertyType.GetMethod("Parse", new[] { typeof(String) }) ??
                                prop.PropertyType.GetProperty("Value").PropertyType.GetMethod("Parse", new[] { typeof(String) });

                    var dd = prop.PropertyType.BaseType == typeof(ValueType) && prop.PropertyType.IsGenericType;
                    if (dd && parse != null)
                    {
                        prop.SetValue(p, string.IsNullOrEmpty(dictionary[key].ToString()) ? null : parse.Invoke(prop, new[] { dictionary[key] }), null);
                        continue;
                    }

                    if (parse != null)
                    {
                        var o = parse.Invoke(prop, new[] { dictionary[key] });
                        prop.SetValue(p, o, null);
                    }
                    else
                        prop.SetValue(p, null, null);
                }

                return p;
            }
            catch (Exception)
            {

                return null;
            }


        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var p = (T)obj;
            IDictionary<string, object> serialized = new Dictionary<string, object>();

            foreach (var pi in typeof(T).GetProperties())
            {

                if (pi.GetCustomAttributes(typeof(ScriptIgnoreAttribute), true).Any()) continue;

                if (pi.PropertyType == typeof(Boolean) || pi.PropertyType == typeof(Boolean?))
                {
                    serialized[pi.Name] = pi.GetValue(p, null) == null ? "" : ((Boolean)pi.GetValue(p, null)).ToString();
                    continue;
                }

                if (pi.PropertyType == typeof(DateTime))
                {
                    serialized[pi.Name] = ((DateTime)pi.GetValue(p, null)).ToString(DateFormat);
                    continue;
                }
                if (pi.PropertyType == typeof(Color))
                {
                    var dd = pi.GetValue(p, null);
                    serialized[pi.Name] = ColorTranslator.ToHtml((Color)dd);
                    continue;
                }
                if (pi.PropertyType.BaseType == typeof(Enum))
                {
                    var dd = pi.GetValue(p, null);
                    serialized[pi.Name] = Enum.GetName(pi.PropertyType, dd);
                    continue;
                }
                serialized[pi.Name] = pi.GetValue(p, null);
            }

            return serialized;
        }

    }
}
