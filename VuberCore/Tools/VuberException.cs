using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VuberCore.Tools
{
    public class VuberException : Exception
    {
        public VuberException()
        {
        }

        public VuberException(string message)
            : base(message)
        {
        }

        public VuberException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
