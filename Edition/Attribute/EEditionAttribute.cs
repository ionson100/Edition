namespace Edition.Attribute
{
    /// <summary>
    /// The main attribute that specifies the location of the item to edit
    /// </summary>
    public class EEditionAttribute : EBaseAttribute
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="sortIndex">Position on the page</param>
        /// <param name="displayNameItem">text display</param>
        public EEditionAttribute( int sortIndex, string displayNameItem)
        {
            SortIndex = sortIndex;
            DisplayNameItem = displayNameItem;
            TypeParent = GetType();
        }
    }
}