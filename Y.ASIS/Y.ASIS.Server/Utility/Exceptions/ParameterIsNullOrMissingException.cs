using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.Server.MainThread.Exceptions
{
    class ParameterIsNullOrMissingException : Exception
    {
        public string ParameterName { get; set; }

        public ParameterIsNullOrMissingException(string name)
        {
            ParameterName = name;
        }
    }
}
