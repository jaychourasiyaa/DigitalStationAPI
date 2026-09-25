using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalStation.Contract.Enums;

namespace DigitalStation.Contract.Interfaces
{
    public interface IEmailServices
    {
        public Task<bool> SendEmail(string recipientEmail, EmailType? emailType,string ? name = "User");
        public Task<string?> VerifyOtp(string recipientEmail, string otp);
    }
}
