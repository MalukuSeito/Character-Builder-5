using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_Forms
{
    class LogEntry
    {
        public string message;
        public Exception e;
        public DateTime time;

        public LogEntry(string message, Exception e)
        {
            time = DateTime.Now;
            this.message = message;
            this.e = e;
        }
        public override string ToString()
        {
            return time.ToString() + ": " + message;
        }
    }
}
