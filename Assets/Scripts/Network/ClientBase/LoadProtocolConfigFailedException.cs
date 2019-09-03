using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBase
{
    public class LoadProtocolConfigFailedException : Exception
    {
        public LoadProtocolConfigFailedException(string name) : base(name)
        {
            //xxxxx
        }
    }
}
