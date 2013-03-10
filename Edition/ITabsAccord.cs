using System.Collections.Generic;
using Edition.Accordion;

namespace Edition
{
    internal  interface ITabsAccord
    {
         string JScript();
         List<ItemAccordion> Items { get; set; }
    }
}
