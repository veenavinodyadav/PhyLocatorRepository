using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using PhysicianLocator.Models;

namespace PhysicianLocator.DAL
{
    public class CommonFunction
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CommonFunction));  //Declaring Log4Net


        static int i = 0;
        public string MakePassword(int pl)
        {
            string possibles = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            char[] passwords = new char[pl];
            Random rd = new Random();

            for (int i = 0; i < pl; i++)
            {
                passwords[i] = possibles[rd.Next(0, possibles.Length)];
            }
            return new string(passwords);
        }
        public string arr(string file)
        {
            string[] i = new string[30];
            //i++;
            return (file);
        }
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public string getTimeAgo(DateTime strDate)
        {
            string strTime = string.Empty;
            if (IsDate(Convert.ToString(strDate)))
            {
                TimeSpan t = DateTime.UtcNow - Convert.ToDateTime(strDate);
                double deltaSeconds = t.TotalSeconds;

                double deltaMinutes = deltaSeconds / 60.0f;
                int minutes;

                if (deltaSeconds < 5)
                {
                    return "Just now";
                }
                else if (deltaSeconds < 60)
                {
                    return Math.Floor(deltaSeconds) + " seconds ago";
                }
                else if (deltaSeconds < 120)
                {
                    return "A minute ago";
                }
                else if (deltaMinutes < 60)
                {
                    return Math.Floor(deltaMinutes) + " minutes ago";
                }
                else if (deltaMinutes < 120)
                {
                    return "An hour ago";
                }
                else if (deltaMinutes < (24 * 60))
                {
                    minutes = (int)Math.Floor(deltaMinutes / 60);
                    return minutes + " hours ago";
                }
                else if (deltaMinutes < (24 * 60 * 2))
                {
                    return "Yesterday";
                }
                else if (deltaMinutes < (24 * 60 * 7))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24));
                    return minutes + " days ago";
                }
                else if (deltaMinutes < (24 * 60 * 14))
                {
                    return "Last week";
                }
                else if (deltaMinutes < (24 * 60 * 31))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 7));
                    return minutes + " weeks ago";
                }
                else if (deltaMinutes < (24 * 60 * 61))
                {
                    return "Last month";
                }
                else if (deltaMinutes < (24 * 60 * 365.25))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 30));
                    return minutes + " months ago";
                }
                else if (deltaMinutes < (24 * 60 * 731))
                {
                    return "Last year";
                }

                minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 365));
                return minutes + " years ago";
            }
            else
            {
                return "";
            }
        }
        public bool IsDate(string o)
        {
            DateTime tmp;
            return DateTime.TryParse(o, out tmp);
        }
        public void  EmailLogInsert( string emailAddress,bool IsEmailSend,string tempcode)
        {
            EmailTemplateViewModel manage = new EmailTemplateViewModel();
            EmailLogViewModel model = new EmailLogViewModel();
            LocatorContext context = new LocatorContext();
            EmailTemplateViewModel emaillog_query = (from email in context.DBContext_emailtemplate where email.templateCode == tempcode select email).SingleOrDefault();

            try
            {
                var logrecord = new EmailLogViewModel()
                {
                    EmailLogFrom = emailAddress,
                    IsEmailSend = IsEmailSend,
                    templateid = emaillog_query.templateid,
                    SendOn = DateTime.Now,
                };
                context.DBContext_emaillog.Add(logrecord);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
        }
        public void EmailAttachmentInsert(string path, string tempcode)
        {
            EmailTemplateViewModel manage = new EmailTemplateViewModel();

            EmailLogViewModel model = new EmailLogViewModel();
            EmailTemplateAttachment attachment = new EmailTemplateAttachment();
            LocatorContext context = new LocatorContext();
            EmailTemplateViewModel emailattach_query = (from email in context.DBContext_emailtemplate where email.templateCode == tempcode select email).SingleOrDefault();
            int userid = context.DBContext_register.Max(item => item.UserId);
            attachment.CreatedBy = userid;

            try
            {
                var attachmentrecord = new EmailTemplateAttachment()
                {
                 
                    templateid = emailattach_query.templateid,
                    attachmentName=path,
                    IsActive=attachment.IsActive,
                    IsDeleted=attachment.IsDeleted,
                    CreatedBy=attachment.CreatedBy,
                    CreatedOn=DateTime.Now,
                    LastModifiedBy=attachment.LastModifiedBy,
                    LastModifiedOn= DateTime.Now,
                };
                context.DBContext_emailattachment.Add(attachmentrecord);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
        }

        public bool SendActivationEmail(string email, string subject, string body, string bcc, string cc, string path,string templatecode)
        {
            try
            {
                var smtpClient = new SmtpClient();
                var msg = new MailMessage();
                msg.To.Add(email);
                if (path != null )
                { 
                    Attachment attachment;
                    attachment = new Attachment(path);
                    msg.Attachments.Add(attachment);
                    EmailAttachmentInsert(path, templatecode);
                  
                }
                if (bcc != null)
                {
                    msg.Bcc.Add(bcc);
                }
                if (cc != null)
                {
                    msg.Bcc.Add(bcc);
                }


                msg.Subject = subject;
                msg.Body = body;
                smtpClient.Send(msg);
                bool Ismailsend = true;
                return Ismailsend;


            }
            catch (SmtpFailedRecipientsException ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
        }
    }
}