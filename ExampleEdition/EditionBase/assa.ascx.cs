using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Edition;

namespace ExampleEdition.EditionBase
{
    public partial class assa : System.Web.UI.UserControl, IEdition
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public object SetValue
        {

            set
            {
                for (var i = 0; i < 10; i++)
                {
                    mazai.Items.Add(new ListItem("Text" + i, i.ToString(CultureInfo.InvariantCulture)));
                }

                var item = mazai.Items.FindByValue(value.ToString());
                if (item == null) return;
                mazai.ClearSelection();
                item.Selected = true;
            }
        }
    }
}