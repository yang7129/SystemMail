using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc; 
using System.Text;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;
namespace TestEmail.Controllers
{
    public class DefaultController : Controller
    { 
        string reMsg = "";
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, string TEST)
        {
            if (file !=null)
            {
                //儲存
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/FileUploads"), fileName);
                    file.SaveAs(path);

                    byte[] data = System.IO.File.ReadAllBytes(path); 
                    //發送EMAIL
                    MailService.ReturnResult ReturnResult = new MailService.ReturnResult();
                    MailService.MailServiceClient MailService = new MailService.MailServiceClient();
                    MailService.InputMail InputMail = new MailService.InputMail();
                    try
                    { 
                        InputMail.addressFrom = "aa@aa.aa.aa";
                        InputMail.addressNameFrom = "aa";
                        InputMail.addressTo = "aa@aa.aa.aa";
                        InputMail.addressNameTo = "lex";
                        InputMail.subject = "TEST信件";
                        InputMail.body = "這是測試內<img src='" + fileName + "'>容";
                        InputMail.filebyte = Convert.ToBase64String(data);
                        InputMail.fileName = fileName;
                        ReturnResult = MailService.Mail(InputMail);
                        ViewBag.Send = ReturnResult.ReturnMsgNo + "|" + ReturnResult.ReturnMsg;
                    }
                    catch (Exception Ex)
                    {
                        ViewBag.Send = "Err" + Ex.ToString();
                    }
                    finally { MailService.Close(); } 
                }  
            } 
            return View();
        }  
    }
}
