using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Edition
{
    internal class JSCompressor : CodeCompressor
    {
        /// <summary>
        /// remove C-style comments and multi-line comments.
        /// </summary>
        private bool _removeComments = true;

        /// <summary>
        /// trim lines and remove multiple blank lines.
        /// </summary>
        private bool _removeAndTrimBlankLines = true;

        /// <summary>
        /// remove all CRLF characters.
        /// </summary>
        private bool _removeCarriageReturns = true;

        /// <summary>
        /// skim the rest of the code.
        /// </summary>
        private bool _removeEverthingElse = true;

        /// <summary>
        /// Matches /* c-style comments. 
        /// */
        /// </summary>
        private readonly Regex _regCStyleComment;

        /// <summary>
        /// Matches //line comments.
        /// </summary>
        private readonly Regex _regLineComment;

        /// <summary>
        /// Matches any white space including CRLF at the end of line.
        /// </summary>
        private readonly Regex _regSpaceLeft;

        /// <summary>
        /// Matches any whitespace at the beginning of the line.
        /// </summary>
        private readonly Regex _regSpaceRight;

        /// <summary>
        /// Matches any space-tab combination.
        /// </summary>
        private readonly Regex _regWhiteSpaceExceptCrlf;

        /// <summary>
        /// Quotes and regular expressions.
        /// </summary>
        private readonly Regex _regSpecialElement;

        /// <summary>
        /// Matches opening curly brace "{".
        /// </summary>
        private readonly Regex _regLeftCurlyBrace;

        /// <summary>
        /// Matches closing curly brace "}".
        /// </summary>
        private readonly Regex _regRightCurlyBrace;

        /// <summary>
        /// Matches a comma surrounded by whitespace characters.
        /// </summary>
        private readonly Regex _regComma;

        /// <summary>
        /// Matches a semi-column surrounded by whitespace characters.
        /// </summary>
        private readonly Regex _regSemiColumn;

        /// <summary>
        /// Matches CRLF characters.
        /// </summary>
        private readonly Regex _regNewLine;

        private readonly Regex _regCarriageAfterKeyword;

        /// <summary>
        /// Hashtable to store the captured special elements.
        /// </summary>
        private readonly Hashtable _htCaptureFields;

        /// <summary>
        /// Hashtable to store pre-compiled regular expressions for special elements.
        /// </summary>
        private readonly Hashtable _htRegSpecialElement;

        /// <summary>
        /// The total number of special elements captured.
        /// </summary>
        private int _specialItemCount;

        /// <summary>
        /// If <code>true</code> comments will be removed.
        /// Default value is <code>true</code>.
        /// </summary>
        public bool RemoveComments
        {
            set
            {
                _removeComments = value;
            }
        }

        /// <summary>
        /// If <code>true</code> lines will be trimmed and multiple blank
        /// new lines will be removed.
        /// Default value is <code>true</code>.
        /// </summary>
        public bool TrimLines
        {
            set
            {
                _removeAndTrimBlankLines = value;
            }
        }

        /// <summary>
        /// If <code>true</code> all CRLF characters will be removed.
        /// Default value is <code>true</code>.
        /// </summary>
        public bool RemoveCRLF
        {
            set
            {
                _removeCarriageReturns = value;
            }
        }

        /// <summary>
        /// If <code>true</code> some additional compression will be done.
        /// Default value is <code>true</code>.
        /// </summary>
        public bool RemoveEverthingElse
        {
            set
            {
                _removeEverthingElse = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JSCompressor()
        {
            /* initialize members */

            _regCStyleComment = new Regex("/\\*.*?\\*/", RegexOptions.Compiled | RegexOptions.Singleline);
            _regLineComment = new Regex("//.*\r\n", RegexOptions.Compiled | RegexOptions.ECMAScript);
            _regSpaceLeft = new Regex("^\\s*", RegexOptions.Compiled | RegexOptions.Multiline);
            _regSpaceRight = new Regex("\\s*\\r\\n", RegexOptions.Compiled | RegexOptions.ECMAScript);
            _regWhiteSpaceExceptCrlf = new Regex("[ \\t]+", RegexOptions.Compiled | RegexOptions.ECMAScript);
            _regSpecialElement = new Regex(
                "\"[^\"\\r\\n]*\"|'[^'\\r\\n]*'|/[^/\\*](?<![/\\S]/.)([^/\\\\\\r\\n]|\\\\.)*/(?=[ig]{0,2}[^\\S])",
                RegexOptions.Compiled | RegexOptions.Multiline);
            _regLeftCurlyBrace = new Regex("\\s*{\\s*", RegexOptions.Compiled | RegexOptions.ECMAScript);
            _regRightCurlyBrace = new Regex("\\s*}\\s*", RegexOptions.Compiled | RegexOptions.ECMAScript);
            _regComma = new Regex("\\s*,\\s*", RegexOptions.Compiled | RegexOptions.ECMAScript);
            _regSemiColumn = new Regex("\\s*;\\s*", RegexOptions.Compiled | RegexOptions.ECMAScript);
            _regNewLine = new Regex("\\r\\n", RegexOptions.Compiled | RegexOptions.ECMAScript);

            _regCarriageAfterKeyword = new Regex(
                "\\r\\n(?<=\\b(abstract|boolean|break|byte|case|catch|char|class|const|continue|default|delete|do|double|else|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|void|while|with)\\r\\n)",
                RegexOptions.Compiled | RegexOptions.ECMAScript);

            _htCaptureFields = new Hashtable();
            _htRegSpecialElement = new Hashtable();

            _specialItemCount = 0;
        }

        /// <summary>
        /// Compresses the given String.
        /// </summary>
        /// <param name="toBeCompressed">The String to be compressed.</param>
        public void Compress(ref String toBeCompressed)
        {
            /*clean the hasthable*/
            _htCaptureFields.Clear();
            _htRegSpecialElement.Clear();
            _specialItemCount = 0;



            /* mark special elements */
            MarkQuotesAndRegExps(ref toBeCompressed);

            if (_removeComments)
            {
                /* remove line comments */
                RemoveLineComments(ref toBeCompressed);
                /* remove C Style comments */
                RemoveCStyleComments(ref toBeCompressed);
            }

            if (_removeAndTrimBlankLines)
            {
                /* trim left */
                TrimLinesLeft(ref toBeCompressed);
                /* trim right */
                TrimLinesRight(ref toBeCompressed);
            }

            if (_removeEverthingElse)
            {
                /* { */
                ReplaceLeftCurlyBrace(ref toBeCompressed);
                /* } */
                ReplaceRightCurlyBrace(ref toBeCompressed);
                /* , */
                ReplaceComma(ref toBeCompressed);
                /* ; */
                ReplaceSemiColumn(ref toBeCompressed);
            }

            if (_removeCarriageReturns)
            {
                /* 
                 * else[CRLF]
                 * return
                 */
                ReplaceCarriageAfterKeyword(ref toBeCompressed);
                /* clear all CRLF's */
                ReplaceNewLine(ref toBeCompressed);
            }

            /* restore the formerly stored elements. */
            RestoreQuotesAndRegExps(ref toBeCompressed);

            StringBuilder buffer = new StringBuilder();

            buffer.Append(
                // This part is for my API, so commented out for the CodeProject version.
                //				"/* Copyright   : 2003-2005 (C) Volkan Ozcelik (volkan@sarmal.com)\r\n"+
                //				" * Terms of use: This file (s@rdalya API) is distributed under CC license.\r\n"+
                //				" *               see http://ww.sarmal.com/sardalya/Terms.aspx for details.\r\n"+
                //				" *//*Code Compressed with JS Code Compressor v.1.0.3 - http://www.sarmal.com/*/"
                " /*Code Compressed with JS Code Compressor v.1.0.3 - http://www.sarmal.com/*/"
                );

            buffer.Append(toBeCompressed);
            toBeCompressed = buffer.ToString();
        }
        public const int Codepage = 1254;
        /// <summary>
        /// Reads the text file on sourcePath, and creates a file with compressed
        /// content on the destinationPath.
        /// </summary>
        /// <param name="sourcePath">The fully qualified path to the file to read.</param>
        /// <param name="destinationPath">The fully qualified path to the file to write.</param>
        public void Compress(String sourcePath, String destinationPath)
        {
            /* 
             * Localization is always an issue if you are non-English. 
             * System.Text.Encoding class helps sort out this problem.
             */
            var locale = Encoding.GetEncoding(Codepage);

            var sr = new StreamReader(sourcePath, locale);

            String strCompress = sr.ReadToEnd();
            sr.Close();

            Compress(ref strCompress);

            var sw = new StreamWriter(destinationPath, false, locale);

            sw.Write(strCompress);
            sw.Close();
        }

        /// <summary>
        /// Replaces the stored special elements back to their places.
        /// </summary>
        /// <param name="input">The input String to process.</param>
        private void RestoreQuotesAndRegExps(ref String input)
        {
            int captureCount = _htCaptureFields.Count;
            for (int i = 0; i < captureCount; i++)
            {
                input = ((Regex)_htRegSpecialElement[i]).Replace(input, (String)_htCaptureFields[i]);
            }
        }

        /// <summary>
        /// Quotes and regular expressions should be untouched and unprocessed at all times.
        /// So we mark and store them beforehand in a private Hashtable for later use.
        /// </summary>
        /// <param name="input">The input String to process. It should be a single line.</param>
        private void MarkQuotesAndRegExps(ref String input)
        {
            MatchCollection matches = _regSpecialElement.Matches(input);

            int count = matches.Count;
            Match currentMatch;

            /* store strings and regular expressions */
            for (int i = 0; i < count; i++)
            {
                currentMatch = matches[i];
                _htCaptureFields.Add(_specialItemCount, currentMatch.Value);
                /* we added one more special item to our Hashtable */
                _specialItemCount++;
            }

            /* replace strings and regular expressions */
            for (int i = 0; i < count; i++)
            {
                /* 
                 * compile and add the Regex to the hashtable
                 * so that it executes faster at the Restore phase.
                 *
                 * A trade off between Regex compilation speed and 
                 * memory. 
                 */
                _htRegSpecialElement.Add(i, new Regex("____SPECIAL_ELEMENT____" + (i) + "____",
                                                     RegexOptions.ECMAScript | RegexOptions.Compiled));

                input = _regSpecialElement.Replace(input, "____SPECIAL_ELEMENT____" + (i) + "____", 1);
            }
        }

        /// <summary>
        /// Removes any multi-line single line /* c style comments */
        /// </summary>
        /// <param name="input">The input String to replace.</param>
        private void RemoveCStyleComments(ref String input)
        {
            input = _regCStyleComment.Replace(input, "");
        }

        /// <summary>
        /// Removes all \\line comments.
        /// </summary>
        /// <param name="input">The input String to replace.</param>
        private void RemoveLineComments(ref String input)
        {
            input = _regLineComment.Replace(input, "");
        }

        /// <summary>
        /// Replaces any duplicate space-tab combinations with a single space.
        /// </summary>
        /// <param name="input">The input String to replace.</param>
        private void ReplaceDuplicateWhiteSpace(ref String input)
        {
            input = _regWhiteSpaceExceptCrlf.Replace(input, " ");
        }

        /// <summary>
        /// Trims all the trailing whitespace characters in a line with "".
        /// </summary>
        /// <param name="input">The input String to replace.</param>
        private void TrimLinesLeft(ref String input)
        {
            input = _regSpaceLeft.Replace(input, "");
        }

        /// <summary>
        /// Trims all whitespace after the end of the line, and the proceeding CRLF characters
        /// with a single CRLF.
        /// </summary>
        /// <param name="input">The input String to replace.</param>
        private void TrimLinesRight(ref String input)
        {
            input = _regSpaceRight.Replace(input, "\r\n");
        }

        /// <summary>
        /// Replaces any whitespace before and after "{" characters with "".
        /// </summary>
        /// <param name="input">The input String to replace.</param>
        private void ReplaceLeftCurlyBrace(ref String input)
        {
            input = _regLeftCurlyBrace.Replace(input, "{");
        }

        /// <summary>
        /// Replaces any whitespace before and after "}" characters with "".
        /// </summary>
        /// <param name="input">The input String to replace.</param>
        private void ReplaceRightCurlyBrace(ref String input)
        {
            input = _regRightCurlyBrace.Replace(input, "}");
        }

        private void ReplaceCarriageAfterKeyword(ref String input)
        {
            input = _regCarriageAfterKeyword.Replace(input, " ");
        }

        /// <summary>
        /// Replaces any whitespace before and after "," characters with "".
        /// </summary>
        /// <param name="input">The input String to replace.</param>
        private void ReplaceComma(ref String input)
        {
            input = _regComma.Replace(input, ",");
        }

        /// <summary>
        /// Replaces any whitespace before and after ";" characters with "".
        /// </summary>
        /// <param name="input">The input String to replace.</param>
        private void ReplaceSemiColumn(ref String input)
        {
            input = _regSemiColumn.Replace(input, ";");
        }

        /// <summary>
        /// Replaces all CRLF characters in the input to "".
        /// </summary>
        /// <param name="input">The input String to replace.</param>
        private void ReplaceNewLine(ref String input)
        {
            input = _regNewLine.Replace(input, "");
        }
    }
}