using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.Server.MainThread.Exceptions
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
