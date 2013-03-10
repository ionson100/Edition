using System;
using System.ComponentModel.DataAnnotations;

namespace Edition.Attribute
{
    /// <summary>
    /// Base Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false,Inherited = true)]
    public abstract class EBaseAttribute : System.Attribute, IComparable<EBaseAttribute>
    {
        private bool _isNotReadOnly;
        internal bool IsNotReadOnly
        {
            get
            {
                return !_isNotReadOnly && PropertyInfo.CanWrite;
            }
            set { _isNotReadOnly = value; }
        }

        internal DataTypeAttribute DataType { get; set; }
        internal EDataTypeAttribute EDataType { get; set; }
        internal string PropertyName
        {
            get { return PropertyInfo.Name; }
        }
        internal Type PropertyType
        {
            get { return PropertyInfo.PropertyType; }
        }

        internal BaseValidateMvc BaseValidateMvc { get; set; }
        internal Type TypeParent;
        internal bool IsNotAddJson { get; set; }
        internal string StringFormat { get; set; }
        internal string Culture { get; set; }
        internal System.Reflection.PropertyInfo PropertyInfo { get; set; }
       

        /// <summary>
        /// ToolTip text
        /// </summary>
        public string ToolTip { get; set; }

        private string _displayName; 
        /// <summary>
        /// Visible text
        /// </summary>
        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(_displayName) ? PropertyName : _displayName;
            }
            set { _displayName = value; }
        }

        /// <summary>
        /// Sorting in item
        /// </summary>
        public int SortIndex { get; set; }
        /// <summary>
        /// Name item
        /// </summary>
        public string DisplayNameItem { get; set; }
      

        public int CompareTo(EBaseAttribute other)
        {
            if (SortIndex > other.SortIndex)
            {
                return -1;
            }
            return SortIndex == other.SortIndex ? 0 : 1;
        }

    }

   /// <summary>
   /// 
   /// </summary>
   internal  class BaseValidateMvc
    {
        public RegularExpressionAttribute RegularExpression { get; set; }
        public RequiredAttribute RequiredAttribute { get; set; }
        public RangeAttribute RangeAttribute { get; set; }
        public StringLengthAttribute StringLengthAttribute { get; set; }
      
    }
    /// <summary>
   /// Selecting the type items
    /// </summary>
    public enum EDataType
    {
        /// <summary>
        /// Module colors
        /// </summary>
        Color=1,
        /// <summary>
        /// DropDown as bool
        /// </summary>
        DropDownAsBool = 3,
        /// <summary>
        /// DropDown for enum
        /// </summary>
        DropDownForEnum = 4,
        /// <summary>
        ///DropDown simple
        /// </summary>
        DropDownCore=14,
        /// <summary>
        ///  TinyMce as panel
        /// </summary>
        TinyMceClassic=5,
        /// <summary>
        ///  TinyMce as modal panel
        /// </summary>
        TinyMceModal=6,
        /// <summary>
        /// File Upload panel
        /// </summary>
        FileUpload=7,
        /// <summary>
        /// Hidden Fiel
        /// </summary>
        HiddenFiel=8,
        /// <summary>
        /// Check box one
        /// </summary>
        CheckBox=9,
        /// <summary>
        /// imput for email
        /// </summary>
        TextBoxRegExEmail=10,
        /// <summary>
        /// imput for number
        /// </summary>
        TextBoxRegExpNumber=11,
        /// <summary>
        /// imput for as double
        /// </summary>
        TextBoxRegExpDouble = 12
    }
}