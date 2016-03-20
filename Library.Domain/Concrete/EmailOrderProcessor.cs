using System.Net.Mail;
using System.Net;
using System.Text;
using Library.Domain.Abstract;
using Library.Domain.Entities;

namespace Library.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "library@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"d:\library_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                .AppendLine("New order submit")
                .AppendLine("----")
                .AppendLine("Items:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Book.PriceLoss * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity,
                        line.Book.Name, subtotal);
                }

                body.AppendFormat("Total: {0:c}", cart.CostForLost())
                    .AppendLine("---")
                    .AppendLine("Order:")
                    .AppendLine(shippingDetails.FirstName)
                    .AppendLine(shippingDetails.SecondName)
                    .AppendLine(shippingDetails.Contry)
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.Adress)
                    .AppendLine(shippingDetails.Index)
                    .AppendLine("---")
                    .AppendFormat("Wrapper: {0}", shippingDetails.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress, // Куда
                    emailSettings.MailToAddress, //Откуда
                    "New order submit", //Субьект
                    body.ToString()); //Тело

                if (emailSettings.WriteAsFile)
                {
                    
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
