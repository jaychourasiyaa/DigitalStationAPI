using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalStation.Contract.Models;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Provider.Database;
using DigitalStation.Contract.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using DigitalStation.Contract.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using CloudinaryDotNet;
using Azure;

namespace DigitalStation.Provider.Services
{

    public class EmailServices : IEmailServices
    {
        private readonly dbContext _dbContext;
        private readonly EmailConfiguration emailConfiguration;
        private readonly AuthServices authServices;
        public EmailServices(dbContext _dbContext, EmailConfiguration emailConfiguration, AuthServices authServices)
        {
            this._dbContext = _dbContext;
            this.emailConfiguration = emailConfiguration;
            this.authServices = authServices;
        }


        public async Task<bool> SendEmail(string recipientEmail, EmailType? emailType, string? name)
        {
            try
            {
                if (emailType == EmailType.Registration || emailType == EmailType.ForgetPassword || emailType == EmailType.OrderConfirmation)
                {
                    if (emailType == EmailType.ForgetPassword)
                    {
                        var login = await _dbContext.Logins.AnyAsync(x => x.Username == recipientEmail);
                        if (!login)
                        {
                            throw new Exception(ResponseCode.AccountNotExists);
                        }
                    }
                    var otp_Code = new Random().Next(100000, 999999).ToString();
                    var newotp = new Otp
                    {
                        Email = recipientEmail,
                        Code = otp_Code,
                        ExpirationTime = DateTime.UtcNow.AddMinutes(5).AddHours(5).AddMinutes(30)
                    };
                    var subject = "Signup OTP for your Digital Station account";
                    var body = $@"
    <p>Dear {name},</p>
    <br>
    <p>
        You can use the following one-time password (OTP) to sign up to your Digital Station Account account:
    </p>
    <br>
    <p><strong>OTP:</strong> {otp_Code}</p>
    <br>
    <p><strong>Note:</strong> OTP is valid for next 5 minutes</p>
    <br>
    <p>
        Please visit our website at <a href='https://digitalstation.vercel.app/'>websitelink</a>
    </p>
    <br>
    <p>
        Thank you for choosing Digital Station. We are thrilled to welcome you as a valued member of our community and are committed to offering you a seamless experience.
    </p>
    <br>
    <p>
        Please feel free to contact our support team.
    </p>
    <br>
    <p>
    <p>
        Warm regards,<br>
        Digital Station Team
    </p>";
                    if (emailType == EmailType.ForgetPassword)
                    {
                        subject = "Password reset OTP for your Digital Station account";
                        body = $@"
    <p>Dear {name},</p>
    <br>
    <p>
        You can use the following one-time password (OTP) to change your Digital Station Account Password:
    </p>
    <br>
    <p><strong>OTP:</strong> {otp_Code}</p>
    <br>
    <p><strong>Note:</strong> OTP is valid for next 5 minutes</p>
    <br>
    <p>
        In case of any other queries, Please feel free to contact our support team.
    </p>
    <br>
    <p>
    <p>
        Warm regards,<br>
        Digital Station Account Team
    </p>";
                    }
                    if (emailType == EmailType.OrderConfirmation)
                    {
                        subject = $"Order Confirmed #{name}";
                        body = $@"
   <p>Dear User,</p>
<br>
<p>
    Your order has been successfully placed!  
    You can view the details of your order on our website:  
    <a href='https://digitalstation.vercel.app/'>websitelink</a>
</p>
<br>
<p>
    To successfully receive your order from the delivery person, please share the following:
</p>
<ul>
    <li><strong>OTP:</strong> {otp_Code}</li>
    <li><strong>Order ID:</strong> {name}</li>
</ul>
<br>
<p><strong>Note:</strong> Both OTP and Order ID must be shared with the delivery person to complete the delivery.</p>
<br>
<p>
    Thank you for trusting us and choosing Digital Station. We look forward to serving you again!
</p>
<br>
<p>
    Warm regards,<br>
    Digital Station Team
</p>

   ";
                    }
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress("Digital Station", emailConfiguration.Username));
                    email.To.Add(new MailboxAddress("", recipientEmail));
                    email.Subject = subject;

                    // Create the body of the email
                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = body, // Set the HTML body
                        TextBody = body // Set the plain text body
                    };
                    email.Body = bodyBuilder.ToMessageBody();

                    // Send the email
                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                        await client.AuthenticateAsync(emailConfiguration.Username, emailConfiguration.Password);
                        await client.SendAsync(email);
                        await client.DisconnectAsync(true);
                    }
                    var oldOtps = _dbContext.Otps.Where(x => x.Email == recipientEmail);
                    _dbContext.Otps.RemoveRange(oldOtps);
                    await _dbContext.Otps.AddAsync(newotp);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }
        public async Task<string?> VerifyOtp(string recipientEmail, string otp_code)
        {
            try
            {
                var otp = await _dbContext.Otps.Where(o => o.Email == recipientEmail && o.Code == otp_code).FirstOrDefaultAsync();
                if (otp == null || otp.ExpirationTime <= DateTime.Now)
                {
                    return null;
                }
                _dbContext.Otps.Remove(otp);
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Gmail == recipientEmail);
                string token = "";
                if (user != null)
                {
                    token = authServices.GenerateToken(user.Id, user.Name, user.UserRole.ToString(), user.Gmail);
                }
                await _dbContext.SaveChangesAsync();
                return token;
            }
            catch
            {
                throw;
            }
        }
    }
}
