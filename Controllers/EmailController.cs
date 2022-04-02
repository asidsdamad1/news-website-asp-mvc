using System.Net;
using System.Net.Mail;
using System.Web.Helpers;
using System.Web.Mvc;
using NewsWebsite.ViewModel;

namespace NewsWebsite.Controllers
{
    public class EmailController : Controller
    {
        public ActionResult SendMail()
        {
            return View();
            
        }
        [HttpPost]
        // GET
        public ActionResult SendMail(string useremail)
        {
            MailMessage mm = new MailMessage("emailfortest612@gmail.com", useremail);
            mm.Subject = "THANK YOU FOR SUBCRIBING";
            mm.Body = "We will send you news soon";
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("emailfortest612@gmail.com", "asidsdamad1");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = nc;
            smtp.Send(mm);

            ViewBag.msg = "Email have been sent successfully...";
            return View();
        }
    }
}