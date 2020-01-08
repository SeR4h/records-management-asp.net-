using RecordsTransferMgtSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.Helpers;
using System.Web.Mail;
using System.Net.Mail.SmtpClient;
using System.Net.Mail;
using System.Net;

namespace RecordsTransferMgtSystem.Controllers
{

    
    public class HomeController : Controller
    {
        DBConnect db = new DBConnect();
        
        public ActionResult emailsend()
        {
            MailMessage objMail = new MailMessage();
            objMail.From = "yourname@yourdomain.com" ;
            objMail.To = "recipientname@somedomain.com";
            objMail.Cc = "name1@anotherdomain.com" ;
            objMail.Bcc = "name2@anotherdomain.com";
            objMail.BodyFormat = MailFormat.Text;
            objMail.Priority = MailPriority.High ;
            objMail.Subject = "My first ASP.NET email";
            objMail.Body = "Your transfer request was rejected :)";
            SmtpMail.Send(objMail);

        }
       
      
        // GET: Home
        public ActionResult Index()
        {          
            return View();
        }
        [HttpPost]
        public ActionResult Index(string firstname, string lastname, string emailaddress, string username, string password2, string jobtitle)
        {
            ViewBag.Msg = db.addUser(firstname,lastname,emailaddress,username,password2,jobtitle);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login() {
            return View();           
        }
               
        [HttpPost]
        public ActionResult LoginAction(string username, string Password,Userlog model)        {
           string success=db.getLogin(username, Password);         

           ViewBag.Msg = success;

           if (success == "station")
           {
               return RedirectToAction("getRecord", "Home");
           }
           else if (success == "records")
           {

               return RedirectToAction("", "Home");
           }
           else if (success == "admin")
           {

               return RedirectToAction("act", "Home");
           }
           else if (success == "registry")
           {

               return RedirectToAction("Records", "Home");
           }
           else
           {
               return RedirectToAction("Login", "Home");
           }
        
        }
           public ActionResult act()
           {
               return View(db.getUsers()); 
           }
           public ActionResult logOut() {
               return RedirectToAction("Login", "Home");
           }
           public ActionResult recordsHome() {
               return View();
           }
           public ActionResult getRecord() {
               return View();
           }
           [HttpPost]
           public ActionResult getRecorde(FileTransfer files)
           {
               files.dateOfTransfer = files.dateOfTransfer.Date;
              int projectid = db.getRecords(files);
              TempData["msg"] = projectid;
              return RedirectToAction("FileTransferdetails", "Home", new {id= projectid});
           }
           public ActionResult FileTransferdetails(int id)           {
           
               return View(db.listProjectDetail(id));
           }
            
        [HttpPost]
           public ActionResult getArrayData(projectFile fileslist)
           {
               db.addFiles(fileslist);

               return RedirectToAction("getRecord", "Home");
           }
        [HttpPost]
        public ActionResult AddfileTransfer(projectFile model)
        {

            if (model != null)
            {
                return Json("Success");
            }
            else
            {
                return Json("An Error Has occoured");
            }
        }
        /*
        public ActionResult AddfileTransfer(string myJsonString)
           {
              // projectFile file = new projectFile();

               List<projectFile> files = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<projectFile>>(myJsonString);
            

               foreach (projectFile item in files)
               {
                   string no = "SR" + item.SrNo;
                   item.SrNo = no;
                   db.addFiles(item);
                  
               } 

               return RedirectToAction("getRecord", "Home");
           }
       */
        [HttpPost]
           public ActionResult EditProject(FileTransfer t)
           {
               db.updateProject(t);
        
              return RedirectToAction("FileTransferdetails", "Home", new {id= t.projectID});  
           
           }
      [HttpGet]
        public ActionResult DeleteProject(int id) {
            db.DeleteProj(id);
            return RedirectToAction("getRecord", "Home");

        }
      public ActionResult Records()
      {
          return View();

      }
      public ActionResult viewProjects()
      {
          return View(db.getProjects());
      }
        [HttpGet]
      public ActionResult viewProjFiles(int id)
      {
          return View(db.getProjectfiles(id));
      }
        
        public ActionResult approveProj(int id)
        {
            ViewBag.Msg = db.ApproveProjects(id);
            return RedirectToAction("viewProjects", "Home");
        }
        public ActionResult approvedProjects()
        {
            return View(db.getApprovedProjects());
        }
        public ActionResult rejectProj(int id)
        {
            ViewBag.Msg = db.RejectProjects(id);
            return RedirectToAction("viewProjects", "Home");
        }
        public ActionResult RejectedProjects()
        {
            return View(db.getRejectedProjects());
        }
        [HttpGet]
        public ActionResult Edit(int id) {

            var std = db.getUsers().FirstOrDefault();
            return View(std);
        
        }
        [HttpPost]
        public ActionResult EditUsers(Userlog p) {
            db.updateUser(p);
            return RedirectToAction("act", "Home");  
        }
        [HttpGet]
        public ActionResult DeleteUsers(int id)
        {
            db.DeleteUser(id);
            return RedirectToAction("act", "Home"); 
        }
        
        public ActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("sarahalezandor@gmail.com", "Sarah");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "Your Email Password here";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    var mess = new System.Net.Mail.MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    };  
                        smtp.Send(mess);                    
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            } 
            return View();
        }
        /*
        [HttpPost]
        public void sendemail()
        {
            try
            {
                //parameters to send email
                string ToEmail, FromOrSenderEmail = "sarahalezandor@gmail.com", SubJect, Body, cc, Bcc;

                //Reading values from form collection (Querystring) and assigning values to parameters
                ToEmail = Request["ToEmail"].ToString();
                SubJect = Request["EmailSubject"].ToString();
                Body = Request["EMailBody"].ToString();
                cc = Request["EmailCC"].ToString();
                Bcc = Request["EmailBCC"].ToString();
                //Configuring webMail class to send emails
                WebMail.SmtpServer = "smtp.gmail.com"; //gmail smtp server
                WebMail.SmtpPort = 587; //gmail port to send emails
                WebMail.SmtpUseDefaultCredentials = true;
                WebMail.EnableSsl = true; //sending emails with secure protocol
                WebMail.UserName = FromOrSenderEmail;//EmailId used to send emails from application
                WebMail.Password = "al3zand0r";
                WebMail.From = FromOrSenderEmail; //email sender email address.

                //Sending email
                WebMail.Send(to: ToEmail, subject: SubJect, body: Body, cc: cc, bcc: Bcc, isBodyHtml: true);

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }*/
      
    }

}