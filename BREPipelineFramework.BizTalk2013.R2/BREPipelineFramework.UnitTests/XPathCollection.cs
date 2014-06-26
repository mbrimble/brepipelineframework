using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.UnitTests
{
    public class XPathCollection
    {
        public XPathCollection()
        {
            this.XPathQueryList = new Dictionary<string, string>();
        }

        public Dictionary<string, string> XPathQueryList;
    }
}
