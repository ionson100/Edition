using System;

namespace Edition
{
    public interface CodeCompressor
    {
        /// <summary>
        /// Compresses the given String
        /// </summary>
        /// <param name="toBeCompressed">String to be compressed.</param>
        void Compress(ref String toBeCompressed);

        /// <summary>
        /// Compresses sourcePath and writes the compressed Stream
        /// into the destinationPath. A new file is created if file
        /// does not exist.
        /// </summary>
        /// <param name="sourcePath">The source file path to compress</param>
        /// <param name="destinationPath">The destination file path
        /// to write the compressed Stream.</param>
        void Compress(String sourcePath, String destinationPath);

        /// <summary>
        /// If <code>true</code> comments will be removed.
        /// </summary>
        bool RemoveComments { set; }

        /// <summary>
        /// If <code>true/code> lines will be trimmed.
        /// </summary>
        bool TrimLines { set; }

        /// <summary>
        /// If <code>true</code> CRLF characters will be removed.
        /// </summary>
        bool RemoveCRLF { set; }

        /// <summary>
        /// If <code>true</code> additional compression will be done.
        /// </summary>
        bool RemoveEverthingElse { set; }
    }
}