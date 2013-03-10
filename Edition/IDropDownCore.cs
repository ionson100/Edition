using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Edition
{
   /// <summary>
    /// To initialize the drop-down list
   /// </summary>
   public  interface IDropDownCore
    {
        /// <summary>
        /// Getting a list item is to insert into the control
        /// </summary>
        List<ListItem> ListItems { get; }
    }
}
