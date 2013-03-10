using System;

namespace Edition.Attribute
{
    /// <summary>
    /// The display type of control, and method validation
    /// </summary>
   
    public class EDataTypeAttribute :System.Attribute
    {
        /// <summary>
        /// text as false for DropDown as bool
        /// </summary>
        public string DisplayTextAsFalse { get;  set; }
        /// <summary>
        /// text as null for DropDown as bool(nulable)
        /// </summary>
        public string DisplayTextasEmpty { get;  set; }
        /// <summary>
        /// text as true for DropDown as bool
        /// </summary>
        public string DisplayTextAsTrue { get; set; }

        /// <summary>
        ///If it is a picture, the path to the image
        /// </summary>
        public String UrlImageForFileUpload { get; set; }
        /// <summary>
        /// Display type, and method validation
        /// </summary>
        public EDataType DataType { get; set; }


        /// <summary>
        /// aggregate for drop-down lists to inherit IDropDownCore
        /// </summary>
        public Type DropDownCore { get; set; }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="type">Display type, and method validation</param>
        public EDataTypeAttribute(EDataType type)
        {
            DataType = type;
        }
    }
}