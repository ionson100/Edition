using System;

namespace Edition.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false,Inherited = true)]
    public class EStringFormatAttribute : System.Attribute
    {
        public string Format { get; set; }
        public string Culture { get; set; }

        public EStringFormatAttribute(string format,string culture)
        {
            Format = format;
            Culture = culture;
        }
    }
}