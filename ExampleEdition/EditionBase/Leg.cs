using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Edition;
using Edition.Attribute;

namespace ExampleEdition.EditionBase
{
    public enum Ion100
    {
        Aaaaaaa = 0,
        Bbbbbbb = 1,
        Sssssss = 2,
        Ddddddd = 3
    }

    [ETypeShow(Edition.TypeShow.Tabs)]//Показывать табсом или аекордеоном

    public class Simpleleg : BaseHeel
    {
        private const string Bases1 = "Description";
        private const string Bases2 = "Photography";

        [EDataType(EDataType.TinyMceModal)]
        [EEdition(1, Bases1, DisplayName = "Description")]
        public virtual string Deckription { get; set; }

        [ScriptIgnore]
        [EDataType(EDataType.TinyMceClassic)]
        [EEdition(1, "Description2", DisplayName = "Simple Description")]
        public virtual string DescriptSimple { get; set; }

        private byte[] _fotoCore;
        public Byte[] FotoCore
        {
            get { return _fotoCore; }
            set
            {
                if (value != null)
                    _fotoCore = value;
            }
        }


        public Byte[] FBytes;

        [ScriptIgnore]
        [EDataType(EDataType.FileUpload, UrlImageForFileUpload = "/Assa.ashx")]
        [EEdition(1, Bases2, DisplayName = "Photography on memory")]
        public virtual HttpPostedFileBase Foto
        {
            set
            {
                if (value != null)
                    FotoCore = Edition.Utils.HttpPostedFileToBytes(value);
            }
            get { return null; }
        }
        [AllowHtml]
        [EDataType(EDataType.TinyMceModal)]
        [EEdition(1, "Simple tale", DisplayName = "Uau I have conducted summer")]
        public virtual string Deckription22 { get; set; }

        private string _simpleLabel = "hello <br /> world";

        [ReadOnly(true)]
        [DataType(DataType.Html)]
        [EEdition(2, "Simple tale", DisplayName = "Simple label")]
        public virtual string SimpleLabel
        {
            get { return _simpleLabel; }
            set { _simpleLabel = value; }
        }

        private string _simpleLiteral = "<p><span style=\"color: #ff0000;\"><strong>Простой текст</strong></span></p>";

        [ScriptIgnoreAttribute]
        [DataType(DataType.Html)]
        [EEdition(20, "Simple tale")]
        public virtual string SimpleLiteral
        {
            get { return _simpleLiteral; }
            set { _simpleLiteral = value; }
        }

        [EHiddenField()]
        public bool IsValidate
        {
            get
           ;
            set
           ;
        }

    }

    public class BaseHeel
    {

        private const string Bases = "Base characteristic";

        [EDataType(EDataType.DropDownForEnum)]
        [EEdition(61, Bases, ToolTip = "Text Description")]
        public Ion100 EnumSimple { get; set; }

        [Required(ErrorMessage = "Text Description Error")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
                   ErrorMessage = "Недействительный электронный адрес.{0}")]
        [EEdition(3, Bases, DisplayName = "Mail:", ToolTip = "fsdfsdf dfsdf sdfsdf")]
        public virtual string Email_3 { get; set; }

        [EHiddenField]
        public virtual int IdSimpleleg { get; set; }

        [DataType(DataType.MultilineText)]
        [EEdition(41, Bases, DisplayName = "AddIn", ToolTip = "Text Description")]
        public virtual string Colokwium { get; set; }
        [Range(10, 100,
            ErrorMessage = "Price must be between 0.01 and 100.00")]


        [EEdition(2, Bases, DisplayName = "Price", ToolTip = "Text Description")]
        public virtual decimal Price { get; set; }

        [EDataType(EDataType.Color)]
        [EEdition(22, Bases, DisplayName = "ColorAsSting", ToolTip = "Text Description")]
        public virtual Color Colorbase { get; set; }

        [ReadOnly(true)]
        [EDataType(EDataType.Color)]
        [EEdition(23, Bases, DisplayName = "ColorCore", ToolTip = "Text Description")]
        public virtual string Colorbase2 { get; set; }


        [EEdition(7, Bases, DisplayName = "Count", ToolTip = "Text Description")]
        public virtual int Count { get; set; }

        [ScriptIgnore]
        [DataType(DataType.DateTime)]
        [EEdition(21, Bases, DisplayName = "DateTime", ToolTip = "Text Description")]
        public virtual DateTime Datein { get; set; }

        [DataType(DataType.Date)]
        [EEdition(30, Bases, DisplayName = "Date", ToolTip = "Text Description")]
        public virtual DateTime Datein2 { get; set; }

        [ECustomt("Простая связка", "~/EditionBase/assa.ascx", 70, Bases)]
        public virtual int mazai { get; set; }

        [EDataType(EDataType.DropDownAsBool, DisplayTextAsTrue = "Yes", DisplayTextAsFalse = "No", DisplayTextasEmpty = "Not know")]
        [EEdition(65, Bases, DisplayName = "Simple bool", ToolTip = "Text Description")]
        public virtual bool? IsHow { get; set; }

        [EDataType(EDataType.DropDownAsBool, DisplayTextAsTrue = "Yes", DisplayTextAsFalse = "No", DisplayTextasEmpty = "Not know")]
        [EEdition(80, Bases, DisplayName = "SimpleIntAsBool", ToolTip = "Text Description")]
        public virtual int? IsHow2 { get; set; }


        [EDataType(EDataType.CheckBox)]
        [EEdition(12,
            Bases,
            DisplayName = "CheckBox",
            ToolTip = "Text Description")]
        public virtual Boolean Ssss { get; set; }


        [EDataType(EDataType.DropDownCore, DropDownCore = typeof(DropDownSimple))]
        [EEdition(79, Bases, ToolTip = "Text Description")]
        public int DropDownCore { get; set; }
    }
    public class DropDownSimple : IDropDownCore
    {
        public List<ListItem> ListItems
        {
            get
            {
                var res = new List<ListItem>();
                for (int i = 0; i < 10; i++)
                {
                    res.Add(new ListItem("List-" + i, i.ToString()));
                }
                return res;

            }
        }
    }
}