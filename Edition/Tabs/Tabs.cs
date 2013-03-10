using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Edition.Accordion;

namespace Edition.Tabs
{
   


    /// <summary>
    /// 
    /// </summary>
    
    [ToolboxData("<{0}:Tabs runat=server></{0}:Tabs>")]
    public class Tabs : WebControl, ITabsAccord
    {
        private string _strJs;
        List<ItemAccordion> _base = new List<ItemAccordion>();
        protected override void RenderContents(HtmlTextWriter output)
        {
            var tabs = new Panel { ID = ID + "tabs",CssClass = "editionDivItem"};
           
            tabs.Controls.Add(new Literal { Text = "<ul>" });
            var i = 0;
            foreach (var item in _base)
            {
                tabs.Controls.Add(new Literal { Text = string.Format(" <li><a href=\"#tabs-{0}\">{1}</a></li>", (++i), item.HeaderText) });
            }
            tabs.Controls.Add(new Literal { Text = "</ul>" });
            i = 0;
            foreach (var item in _base)
            {
                item.Panel.ID = "tabs-" + (++i);
                tabs.Controls.Add(item.Panel);
            }

            _strJs = ResourceControl.tabsCore.Replace("#acc#", tabs.ID); 
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var htmlTextWriter = new HtmlTextWriter(stringWriter);
            tabs.RenderControl(htmlTextWriter);
            var ee = sb.ToString();
            output.Write(ee);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string JScript()
        {
            return _strJs;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ItemAccordion> Items
        {
            get { return _base; }
            set { _base = value; }
        }
    }
}
