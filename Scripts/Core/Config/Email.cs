using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using Server.MainConfiguration;

namespace Server.Misc
{
    public class Email
    {
        /* In order to support emailing, fill in EmailServer and FromAddress:
        * Example:
        *  public static readonly string EmailServer = "mail.domain.com";
        *  public static readonly string FromAddress = "GOW@domain.com";
        * 
        * If you want to add crash reporting emailing, fill in CrashAddresses:
        * Example:
        *  public static readonly string CrashAddresses = "first@email.here,second@email.here,third@email.here";
        * 
        * If you want to add speech log page emailing, fill in SpeechLogPageAddresses:
        * Example:
        *  public static readonly string SpeechLogPageAddresses = "first@email.here,second@email.here,third@email.here";
        */
        public static readonly string EmailServer = MainConfig.MainEmailServer;
        public static readonly int EmailPort = MainConfig.MainEmailPort;
        public static readonly string FromAddress = MainConfig.MainFromAddress;
        public static readonly string CrashAddresses = MainConfig.MainCrashAddresses;
        public static readonly string SpeechLogPageAddresses = MainConfig.MainSpeechLogPageAddresses;
        private static readonly Regex _pattern = new Regex(@"^[a-z0-9.+_-]+@([a-z0-9-]+\.)+[a-z]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static SmtpClient _Client;
        public static bool IsValid(string address)
        {
            if (address == null || address.Length > 320)
                return false;

            return _pattern.IsMatch(address);
        }

        public static void Configure()
        {
            if (EmailServer != null)
                _Client = new SmtpClient(EmailServer, EmailPort); 
        }

        public static bool Send(MailMessage message)
        {
            try
            {
                // .NET relies on the MTA to generate Message-ID header. Not all MTAs will add this header.  
   
 				DateTime now = DateTime.UtcNow;  
 				string messageID = String.Format("<{0}.{1}@{2}>", now.ToString("yyyyMMdd"), now.ToString("HHmmssff"), EmailServer );  
 				message.Headers.Add("Message-ID", messageID );

                message.Headers.Add("X-Mailer", "GOW"); 
  

                lock (_Client)
                {
                    _Client.Send(message);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static void AsyncSend(MailMessage message)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(SendCallback), message);
        }

        private static void SendCallback(object state)
        {
            MailMessage message = (MailMessage)state;

            if (Send(message))
                Console.WriteLine("Sent e-mail '{0}' to '{1}'.", message.Subject, message.To);
            else
                Console.WriteLine("Failure sending e-mail '{0}' to '{1}'.", message.Subject, message.To);
        }
    }
}