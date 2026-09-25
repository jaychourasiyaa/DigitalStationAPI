using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalStation.Contract.Utility
{
    public class EmailConfiguration
    {
        public string SmtpServer { get; set; } = null!;
        public int SmtpPort { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
