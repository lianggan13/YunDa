using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmServer.Exceptions
{
    class ContentCastException : Exception
    {
        public string Content { get; set; }

        public ContentCastException(string content)
        {
            Content = content;
        }
    }
}
