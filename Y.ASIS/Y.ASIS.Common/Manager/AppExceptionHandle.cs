using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.Common.Manager
{
    public class AppExceptionHandle
    {
        public void Handle(Exception e)
        {
            Console.WriteLine(e.Message + "\r\n" + e.StackTrace);
        }
    }
}
