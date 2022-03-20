using MailKit.Net.Smtp;
using MimeKit;

namespace MicroServices_DB.LogicClass
{
    public static class EmailSender
    {
        public static void Send(string token, string adressToSend)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("CommandAPI", ProgrammValues.SmtpEmail));
            mailMessage.To.Add(new MailboxAddress("User", adressToSend));
            mailMessage.Subject = "Token from API";
            mailMessage.Body = new TextPart("plain")
            {
                Text = "Hello, here is your token - " + token
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 465, true);
                smtpClient.Authenticate(ProgrammValues.SmtpEmail, ProgrammValues.SmtpPass);
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}
