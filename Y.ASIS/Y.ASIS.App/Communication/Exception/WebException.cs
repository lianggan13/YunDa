using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.App.Communication
{
    class WebException : Exception
    {
        public string Url { get; set; }

        public new string Message { get; set; }

        public WebException(string url)
        {
            Url = url;
        }

        public WebException(string url, string message)
        {
            Url = url;
            Message = message;
        }
    }
}
