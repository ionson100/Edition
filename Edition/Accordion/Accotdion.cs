using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Edition.Accordion
{
    /// <summary>
    /// Creating an accordion
    /// </summary>
    [ToolboxData("<{0}:Accordion runat=server></{0}:Accordion>")]
    internal class Accordion : WebControl, ITabsAccord
    {
        private string _strJs;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string JScript()
        {
            return _strJs;
        }

        List<ItemAccordion> _ia = new List<ItemAccordion>();

        /// <summary>
        ///List item is accordion
        /// </summary>
        public List<ItemAccordion> Items
        {
            get { return _ia; }
            set { _ia = value; }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            var accordion = new Panel { ID = ID + "accordion",CssClass = "editionDivItem" };
            foreach (var i in _ia)
            {
                accordion.Controls.Add(new LiteralControl
                                           {
                                               Text = String.Format(@"<h2> <a href='#'>{0}</a></h2>", i.HeaderText)
                                           });
                accordion.Controls.Add(i.Panel);
            }
            _strJs = ResourceControl.accordionCore.Replace("#acc#", accordion.ID);
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var htmlTextWriter = new HtmlTextWriter(stringWriter);
            accordion.RenderControl(htmlTextWriter);
            var ee = sb.ToString();
            output.Write(ee);
        }
    }
}