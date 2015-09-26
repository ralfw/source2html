using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace source2html
{
    public class NancyPortal : Nancy.NancyModule
    {
        public NancyPortal()
        {
			Get ["/"] = p => "hello " + DateTime.Now.ToString ();
        }
    }
}