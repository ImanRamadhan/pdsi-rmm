using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kairos.Library.Notification.Email
{
    public class SmtpClientEmail
    {
        public SmtpClientEmail()
        {
            ListAttachmentByteArray = new Dictionary<string, byte[]>();
            ListAttachmentFileName = new List<string>();
        }

        private Dictionary<string, byte[]> ListAttachmentByteArray { get; set; }
        private List<string> ListAttachmentFileName { get; set; }

        public void AddAttachment(string name, byte[] attachment)
        {
            ListAttachmentByteArray.Add(name, attachment);
        }

        public void AddAttachment(string attachmentFileName)
        {
            ListAttachmentFileName.Add(attachmentFileName);
        }

        public void SendMail(string FromAddr, string ToAddr, string MailSubject, string MailBody)
        {
            //Create instance of main mail message class.
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

            //Configure mail mesage
            mailMessage.From = new System.Net.Mail.MailAddress(FromAddr);

            //Set additional addresses
            string[] ToList = ToAddr.Split(',');
            foreach (string ToMail in ToList)
            {
                if ((!string.IsNullOrEmpty(ToMail.Trim())))
                {
                    mailMessage.To.Add(new System.Net.Mail.MailAddress(ToMail));
                }
            }

            //Text/HTML
            mailMessage.IsBodyHtml = true;

            //Set the subjet and body text
            mailMessage.Subject = MailSubject;
            mailMessage.Body = MailBody;

            //Add one to many attachments
            //mailMessage.Attachments.Add(new System.Net.Mail.Attachment("c:\temp.txt"));

            foreach (var attachment in ListAttachmentByteArray)
            {
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(new System.IO.MemoryStream(attachment.Value), attachment.Key));
            }

            foreach (var attachment in ListAttachmentFileName)
            {
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(attachment));
            }

            //Create an instance of the SmtpClient class for sending the email
            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
            //smtpClient.Host = "192.168.3.81";
            //smtpClient.Port = 25;
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new System.Net.NetworkCredential("zulkarnain.ismail@kairos-it.com", "kairosmail");
            //smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            ////smtpClient.EnableSsl = true;


            //Use a Try/Catch block to trap sending errors
            //Especially useful when looping through multiple sends
            //Try
            //Catch smtpExc As System.Net.Mail.SmtpException
            //Log error information on which email failed.
            //Catch ex As Exception
            //Log general errors
            //End Try
            smtpClient.Send(mailMessage);
        }
    }
}
