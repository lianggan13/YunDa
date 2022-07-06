using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmServer.Exceptions
{
    class ParameterDataFormatOrValueException : Exception
    {
        public string ParameterName { get; set; }

        public object ParameterValue { get; set; }

        public ParameterDataFormatOrValueException(string name, object value)
        {
            ParameterName = name;
            ParameterValue = value;
        }
    }
}
