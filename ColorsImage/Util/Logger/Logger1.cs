using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorsImage.Util.Logger
{
    public class Logger1 : ILogger
    {
        private Action<string> _logAction;

        public Logger1(Action<string> logAction)
        {
            _logAction = logAction;
        }

        public void Log(string message)
        {
            _logAction(message);
        }
    }
}
