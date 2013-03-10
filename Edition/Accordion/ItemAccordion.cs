using System.Web.UI;
using System.Web.UI.WebControls;

namespace Edition.Accordion
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemAccordion
    {
        private readonly Panel _panel = new Panel();

        /// <summary>
        /// 
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ControlCollection Control
        {
            get { return _panel.Controls; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Panel Panel
        {
            get { return _panel; }
        }
    }
}
