using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppTest.Code
{
    public class TestErrorException : Exception
    {
        public Action.Result result { get; set; } 

        public TestErrorException()
        {
            result = Action.Result.StopAndCloseBrowser;
        }

        public TestErrorException(string message, Action.Result result)
            : base(message)
        {
            this.result = result;
        }

        public TestErrorException(string message, Exception inner, Action.Result result)
            : base(message, inner)
        {
            this.result = result;
        }
    }
}
