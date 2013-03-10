using System;
using System.Text.RegularExpressions;

namespace Edition
{
    internal class FileName
    {
        private readonly String _name;
        private readonly String _extension;
        private readonly String _path;

        public FileName(String path)
        {
            /* match the filename and extension */
            var match = Regex.Match(path, "(.*)(\\.[^.]+)$", RegexOptions.Compiled | RegexOptions.ECMAScript);
            _name = match.Groups[1].ToString();
            _extension = match.Groups[2].ToString();
            _path = path;
        }

        public String Name
        {
            get
            {
                return _name;
            }
        }

        public String Extension
        {
            get
            {
                return _extension;
            }
        }

        public String Path
        {
            get
            {
                return _path;
            }
        }
    }

}
