using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;

namespace Kairos.Library.Notification.Email
{
    public class ExchangeServerEmail
    {
        private string credentialEmailUser;
        private string credentialEmailPassword;
        private string credentialDomain;
        private ExchangeVersion serviceVersion;
        private string serviceUrl;
        private int serviceTimeOut;

        public ExchangeServerEmail(string _credentialEmailUser,
                                    string _credentialEmailPassword,
                                    string _credentialDomain,
                                    ExchangeVersion _serviceVersion,
                                    string _serviceUrl)
        {
            credentialEmailUser = _credentialEmailUser;
            credentialEmailPassword = _credentialEmailPassword;
            credentialDomain = _credentialDomain;
            serviceVersion = _serviceVersion;
            serviceUrl = _serviceUrl;
            serviceTimeOut = 100000;
            ListAttachmentByteArray = new Dictionary<string, byte[]>();
            //ListAttachmentFileName = new Dictionary<string, string>();
            ListAttachmentFileName = new List<string>();
        }

        public ExchangeServerEmail(string _credentialEmailUser,
                                    string _credentialEmailPassword,
                                    string _credentialDomain,
                                    ExchangeVersion _serviceVersion,
                                    string _serviceUrl,
                                    int _serviceTimeOut)
        {
            credentialEmailUser = _credentialEmailUser;
            credentialEmailPassword = _credentialEmailPassword;
            credentialDomain = _credentialDomain;
            serviceVersion = _serviceVersion;
            serviceUrl = _serviceUrl;
            serviceTimeOut = _serviceTimeOut;
            ListAttachmentByteArray = new Dictionary<string, byte[]>();
            //ListAttachmentFileName = new Dictionary<string, string>();
            ListAttachmentFileName = new List<string>();
        }

        //public enum FileAttachmentType
        //{
        //    None,
        //    FileName,
        //    ByteArray
        //}

        //private List<byte[]> ListAttachmentByteArray { get; set; }
        //private List<string> ListAttachmentFileName { get; set; }

        private Dictionary<string, byte[]> ListAttachmentByteArray { get; set; }
        //private Dictionary<string, string> ListAttachmentFileName { get; set; }
        private List<string> ListAttachmentFileName { get; set; }

        public void AddAttachment(string name, byte[] attachment)
        {
            ListAttachmentByteArray.Add(name, attachment);
        }

        //public void AddAttachment(string name, string attachment)
        //{
        //    ListAttachmentFileName.Add(name, attachment);
        //}

        public void AddAttachment(string attachmentFileName)
        {
            ListAttachmentFileName.Add(attachmentFileName);
        }

        public void SendMail(string FromAddr, string ToAddr, string MailSubject, string MailBody)
        {
            WebCredentials webCredential = new WebCredentials(credentialEmailUser, credentialEmailPassword, credentialDomain);

            ExchangeService service = new ExchangeService(serviceVersion); // new ExchangeService(ExchangeVersion.Exchange2010);
            service.Credentials = webCredential;
            service.Url = new Uri(serviceUrl); // new Uri("https://webmail.carigali.co.id/EWS/Exchange.asmx");
            //service.AutodiscoverUrl("ofas@carigali.co.id");
            service.Timeout = serviceTimeOut;

            EmailMessage message = new EmailMessage(service);
            message.Sender = FromAddr;
            foreach(var item in ToAddr.Split(';'))
            {
                message.ToRecipients.Add(item);
            }
            message.Subject = MailSubject;
            message.Body = MailBody;

            foreach (var attachment in ListAttachmentByteArray)
            {
                message.Attachments.AddFileAttachment(attachment.Key, attachment.Value);
            }

            foreach (var attachment in ListAttachmentFileName)
            {
                //message.Attachments.AddFileAttachment(attachment.Key, attachment.Value);
                message.Attachments.AddFileAttachment(attachment);
            }

            try
            {
                message.Send();
            }
            catch (Exception err)
            { }
        }
    }
}
