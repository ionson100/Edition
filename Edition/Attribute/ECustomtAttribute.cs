namespace Edition.Attribute
{
    /// <summary>
    /// Custom control
    /// </summary>
    public class ECustomtAttribute : EBaseAttribute
    {
        /// <summary>
        /// url path for custom control
        /// </summary>
        public string UrlForControl { get; set; }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="urlForConttrol">url </param>
        /// <param name="sortIndex"> position on the page</param>
        /// <param name="displayNameItem">page</param>
        public ECustomtAttribute(string displayName, string urlForConttrol, int sortIndex, string displayNameItem)
        {
            UrlForControl = urlForConttrol;
            DisplayName = displayName;
            SortIndex = sortIndex;
            DisplayNameItem = displayNameItem;
            TypeParent = GetType();
        }
    }
}