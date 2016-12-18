using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;

namespace PREP.Functions
{

    public static class Mail
    {

        /// <summary>
        /// List of From Mail of Amdocs
        /// </summary>
        private static IDictionary<string, NetworkCredential> FromMailList = new Dictionary<string, NetworkCredential>() {
                            { "_PRR", new NetworkCredential("GRPRRDBMAIL@amdocs.com", "Amdocs3") { }},
                        };

        /// <summary>
        /// defind defult of Amdocs_Mail and Port
        /// </summary>
        private static string Amdocs_Mail = ConfigurationManager.AppSettings["Amdocs_Mail"];
        private static int Amdocs_Port = Int32.Parse(ConfigurationManager.AppSettings["Amdocs_Port"]);



        #region Public functions
        /// <summary>
        /// Send Mail 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="FromMail"></param>
        /// <returns></returns>
        public static string SendMail(this MailMessage message, string FromMail, bool saveMail)
        {
            string FilePath = "";
            try
            {
                NetworkCredential networkCredential = FromMailList[FromMail];
                message.From = new MailAddress(networkCredential.UserName, FromMail);

                using (SmtpClient smtpClient = new SmtpClient(Amdocs_Mail, Amdocs_Port))
                {
                    // send mail
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = networkCredential;
                    smtpClient.Send(message);

                    // save mail as image
                    if (saveMail)
                    {
                        FilePath = message.SaveMailMessage(HttpContext.Current.Server.MapPath(VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["PublicationMailsFolder"])));
                    }
                }
                return FilePath;
            }
            catch (Exception ex)
            {
                return FilePath;
            }
        }

        /// <summary>
        /// Save Mail in Specific File
        /// Converts a MailMessage to an EML file stream.
        /// return file path 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="directoryPath"></param>
        public static string SaveMailMessage(this MailMessage msg, string directoryPath)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    var id = Guid.NewGuid();

                    var tempFolder = Path.Combine(Path.GetTempPath(), Assembly.GetExecutingAssembly().GetName().Name);

                    tempFolder = Path.Combine(tempFolder, "MailMessageToEMLTemp");

                    // create a temp folder to hold just this .eml file so that we can find it easily.
                    tempFolder = Path.Combine(tempFolder, id.ToString());

                    if (!Directory.Exists(tempFolder))
                    {
                        Directory.CreateDirectory(tempFolder);
                    }

                    client.UseDefaultCredentials = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    client.PickupDirectoryLocation = tempFolder;
                    client.Send(msg);

                    // tempFolder should contain 1 eml file

                    var filePath = Directory.GetFiles(tempFolder).Single();
                    var FileName = Path.GetFileName(filePath);
                    string destFile = System.IO.Path.Combine(directoryPath, FileName);

                    // To copy a folder's contents to a new location:
                    // Create a new target folder, if necessary.
                    if (!System.IO.Directory.Exists(directoryPath))
                    {
                        System.IO.Directory.CreateDirectory(directoryPath);
                    }

                    // To copy a file to another location and 
                    // overwrite the destination file if it already exists.
                    System.IO.File.Copy(@filePath, @destFile, true);
                    return FileName;
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        #endregion




    }
}