using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace MyCardSystemMail
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "MailService"。
    // 注意: 若要啟動 WCF 測試用戶端以便測試此服務，請在 [方案總管] 中選取 MailService.svc 或 MailService.svc.cs，然後開始偵錯。
    public class MailService : @base, IMailService
    { 
        public ReturnResult Mail(InputMail InputMail)
        {
            LogTxt(JsonConvert.SerializeObject(InputMail));
            ReturnResult ReturnResult = new ReturnResult();             
            
            SmtpClient SmtpClient = new SmtpClient();
            SmtpClient.Host = Properties.Settings.Default.serverHost; //InputMail.serverHost;
            MailMessage MailMessage = new MailMessage();
            MailAddress MailAddress = new MailAddress(InputMail.addressFrom, (string.IsNullOrEmpty(InputMail.addressNameFrom) ? InputMail.addressFrom : InputMail.addressNameFrom));
            MailMessage.From = MailAddress;
            MailMessage.To.Add(new MailAddress(InputMail.addressTo, (string.IsNullOrEmpty(InputMail.addressNameTo) ? InputMail.addressTo : InputMail.addressNameTo)));
            MailMessage.Subject = InputMail.subject;
            MailMessage.Body = InputMail.body;
            MailMessage.IsBodyHtml = true;


            if (string.IsNullOrEmpty(InputMail.fileName).Equals(false) && string.IsNullOrEmpty(InputMail.filebyte).Equals(false))
            {
                string directory = "SendFile"; 
                DeleteFile("*", 10, directory);
                System.IO.FileStream _FileStream = new System.IO.FileStream(HttpContext.Current.Server.MapPath("~/" + directory + "/" + InputMail.fileName), System.IO.FileMode.Create, System.IO.FileAccess.Write);
                byte[] filebyte = Convert.FromBase64String(InputMail.filebyte);
                _FileStream.Write(filebyte, 0, filebyte.Length);
                _FileStream.Close();
                Attachment data = new Attachment(HttpContext.Current.Server.MapPath("~/" + directory + "/" + InputMail.fileName), MediaTypeNames.Application.Octet);
                MailMessage.Attachments.Add(data); 
            }
            try
            {
                SmtpClient.Send(MailMessage);
                ReturnResult.ReturnMsgNo = 1;
                ReturnResult.ReturnMsg = "發送成功";
                ReturnResult.ErrorCode = "SYSM0001";
            }
            catch (Exception Ex)
            {
                ReturnResult.ReturnMsgNo = -999;
                ReturnResult.ReturnMsg = Ex.ToString();
                ReturnResult.ErrorCode = "SYSM0002";
            }
            finally {
                MailMessage.Dispose(); 
                SmtpClient.Dispose();
            }
            LogTxt(JsonConvert.SerializeObject(ReturnResult));
            return ReturnResult;
        }
        private void DeleteFile(string FilePattern, int retainDay, string directory)
        {
            try
            {
                Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory));
                String[] FileCollection;
                if (string.IsNullOrEmpty(FilePattern))
                    FileCollection = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory));
                else
                    FileCollection = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory), FilePattern);

                for (int i = 0; i < FileCollection.Length; i++)
                {
                    FileInfo theFileInfo = new FileInfo(FileCollection[i]);
                    TimeSpan TIS = DateTime.Now.Subtract(theFileInfo.LastWriteTime);
                    if (TIS.TotalDays >= retainDay)
                        File.Delete(theFileInfo.FullName);
                }
            }
            catch (Exception)
            {
            }

        }

        public void LogTxt(string Msg)
        {
            if (!string.IsNullOrEmpty(Msg))
                LogTxt(Msg, "log", 10);
        }
        /// <summary> 
        /// 紀錄資料純放文字檔 <para></para>  
        /// 可自訂存放天數的txt檔案 
        /// </summary> 
        /// <param name="Msg">訊息</param> 
        /// <param name="directory">自訂資料夾(選填)</param> 
        /// <param name="retainDay">存放天數(選填)</param>  
        public void LogTxt(string Msg, string directory = "log", int retainDay = 10)
        {
            DeleteFile("*.txt", retainDay, directory);

            if (!string.IsNullOrEmpty(Msg))
            {
                Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory));
                try
                {
                    System.IO.File.AppendAllText(string.Format(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory) + "/Log-{0}.txt", DateTime.Now.ToString("yyyy-MM-dd")), string.Concat(new object[] { Msg, Environment.NewLine }));
                }
                catch (Exception Ex)
                {
                    System.IO.File.AppendAllText(string.Format(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory) + "/Log-{0}.txt", DateTime.Now.ToString("yyyy-MM-dd")), string.Concat(new object[] { "=============Error===============" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine + Ex.ToString() + Environment.NewLine + "=============Error===============", Environment.NewLine }));
                }
            }
        }

    }
}

