using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UserPoolingApi.Services;
using UserPoolingApi.Helper;
using UserPoolingApi.Models;
using UserPoolingApi.ViewModels;
using UserPoolingApi.EntityFramework;

namespace UserPoolingApi.Controllers
{
    [Produces("application/json")]
    [Route("Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private DataContext _context;
        private readonly SmtpHelper _send;
        private readonly IMapper _map;
        private IConfiguration _configuration { get; set; }

        public AdminController(DataContext context, SmtpHelper send, IConfiguration configuration, IMapper map) {
            _context = context;
            _send = send;
            _configuration = configuration;
            _map = map;
        }

        [AllowAnonymous]
        [HttpGet("getConfig")]
        public IActionResult getConfig()
        {
            var config2 = _configuration.GetConnectionString("DefaultConnection");
            return Json(config2);
        }
        
        [HttpPost("Send")]
        public async Task<IActionResult> SendListToClient([FromBody] SendListViewModel sendListVM)
        {
            if (sendListVM.SendList != null)
            {
                string[] userId = null;
                userId = sendListVM.SendList.Split(",");
                string content = "";
                string table;
                table =
                      "                    <table cellspacing=\"0\" cellpadding=\"10\" border=\"0\" align=\"left\" >"
                    + "                       <tbody>"
                    + "                          <tr>";
                //http: //13.75.89.123:8081/generatedCV/UserId_327.pdf
                for (int i = 0; i < userId.Length; i++)
                {
                    //Check if the user exist
                    var is_user_exist = _context.Users.Any(u => u.UserId == Int32.Parse(userId[i]));
                    if (!is_user_exist)
                    {
                        return Json(StatusCode(404));
                    }
                    /*Create an object to represent a user in the database == userId */


                    var user = _context.Users.FirstOrDefault(u => u.UserId == Int32.Parse(userId[i]));
                    var dir = _configuration.GetSection("Directory2:ForGeneratedCV").Value;
                    var path = dir + user.UserId + "/"+  user.FirstName+"_"+user.LastName+"_CV.pdf";

                    table += "<tr><td>" + (i + 1) + ". " + user.FirstName + " " + user.LastName + "</td><td>"
                        + "<a href=\"" + path + "\">" + "Link to CV" + " </a></td></tr>";
                }
                //END OF THE TABLE FOR EMPLOYEES, ADD THIS
                table += "</tr>"
                    + "<tr><td>Best regards.</td></tr>"
                    + "<tr><td>Dev Partners</td></tr>"
                    + "</tbody>"
                    + "</table>";
                content = "<p>Good day! Please find below the CV(s) of candidate(s) we are presenting for your consideration:</p>";
                content += table;

                string subject = "List of proposed people for " + sendListVM.CompanyName + " project.";
                string senderEmail = _configuration.GetSection("AdminEmailCredentials:SenderEmail").Value;

                //send the content here
                await _send.SendEmailAsync(sendListVM.ReceiverName, senderEmail, sendListVM.ReceiverEmail, content, subject);
            }
            return Json(Ok());
        }

        [HttpPost("Message")]
        public IActionResult CreateMessage([FromBody] MessageTemplateCreateViewModel messageTemplateVM ) {

            try
            {
                if (_context.Messages.Any(m => m.Position == messageTemplateVM.Position))
                {
                    return Json(StatusCode(409));
                }
                var messageTemplate = _map.Map<Message>(messageTemplateVM);
                _context.Add(messageTemplate);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
            return Json(Ok());
            //return Json(messageTemplateVM);
        }

        [HttpGet("Message/{id}")]
        public IActionResult GetMessage(int id) {
            try
            {
                
                if (!_context.Messages.Any(m => m.Position == id))
                {
                    return Json(BadRequest("Message with PositionID " + id + " is not found"));
                }

                var message = _context.Messages.FirstOrDefault(m => m.Position == id);

                return Json(message);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [HttpPut("Message/{id}")]
        public IActionResult UpdateMessage([FromBody] MessageTemplateCreateViewModel messageTemplateVM, int id)
        {
            try
            {
                if (!_context.Messages.Any(m => m.Position == id))
                {
                    return Json(BadRequest("Message with PositionID " + id + " is not found"));
                }

                var message = _context.Messages.FirstOrDefault(m => m.Position == id);
                message.Subject = messageTemplateVM.Subject;
                message.Body = messageTemplateVM.Body;
                message.Position = messageTemplateVM.Position;
                _context.Messages.Update(message);
                _context.SaveChanges();
                return Json(StatusCode(204, "Message is updated."));
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [HttpDelete("Message/{id}")]
        public IActionResult DeleteMessage(int id)
        {
            try
            {
                if (!_context.Messages.Any(m => m.Position == id))
                {
                    return Json(BadRequest("Message with PositionID " + id + " is not found"));
                }

                var message = _context.Messages.FirstOrDefault(m => m.Position == id);

                _context.Messages.Remove(message);
                _context.SaveChanges();

                return Json(new NoContentResult());
            }
            catch (Exception ex)
            {
                return Json(BadRequest(ex));
            }
        }

        [HttpPost("SendInvitation/{id}")]
        public async Task<IActionResult> SendInvitation(int id) {

            try
            {
                if(!_context.Users.Any(u => u.UserId == id))
                {
                    return Json(NotFound());
                }

                var user = _context.Users.FirstOrDefault(u => u.UserId == id);
                string senderEmail = _configuration.GetSection("AdminEmailCredentials:SenderEmail").Value;

                if (!_context.Messages.Any(m => m.Position == user.PositionDesiredId))
                {
                    return Json("False");
                }

                var message = _context.Messages.FirstOrDefault(m => m.Position == user.PositionDesiredId);

                //var message2 = _context.Messages.Where(m => m.Position == user.PositionDesiredId);
                //message2 = message2.FirstOrDefault(m => m.MessageType == 2); //where two (2) means invitation to take an exam.

                var content = message.Body;
                var subject = message.Subject;
                await _send.SendEmailAsync(user.FirstName, senderEmail, user.Email, content, subject);
            }
            catch (Exception)
            {

                throw;
            }

            return Json(Ok());
        }


        [HttpPost("SendInvitationByBulk")]
        public async Task<IActionResult> SendInvitationbyBulk(SendInvitationByBulkViewModel sendByBulkVM)
        {
            if (sendByBulkVM.ids == null) {
                return Json(BadRequest("Ids is null."));
            }

            string[] userIds = null;

            Console.WriteLine(sendByBulkVM.ids);
            if (sendByBulkVM.ids.Contains(",")) userIds = sendByBulkVM.ids.Split(",");
            
            else
            {
                userIds = new string[1];
                userIds[0] = sendByBulkVM.ids;
            }

            var path = "";

            // Creating a folder where the uploaded file will be stored.
            if (sendByBulkVM.UploadedFile == null || sendByBulkVM.UploadedFile.Length == 0)
            {
            }
            else
            {
                var dateTime = DateTime.Now.ToString("yyyy_MM_ddTHH_mm_ss");
                path = Path.Combine("C:\\Email", dateTime.ToString(), sendByBulkVM.UploadedFile.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    sendByBulkVM.UploadedFile.CopyTo(stream);
                }
            }
            
            try
            {
                for (int i = 0; i < userIds.Length; i++)
                {
                    var is_user_exist = _context.Users.Any(u => u.UserId == Int32.Parse(userIds[i]));
                    if (!is_user_exist)
                    {
                        return Json(StatusCode(404));
                    }

                    var user = _context.Users.FirstOrDefault(u => u.UserId == Int32.Parse(userIds[i]));
                    string senderEmail = _configuration.GetSection("AdminEmailCredentials:SenderEmail").Value;
                    var content = sendByBulkVM.Content;

                    if (sendByBulkVM.UploadedFile == null || sendByBulkVM.UploadedFile.Length == 0)
                    {

                    }
                    else
                    {
                        var dir = _configuration.GetSection("Directory3:ForEmail").Value;
                        var path2 = dir + DateTime.Now.ToString("yyyy_MM_ddTHH_mm_ss") +"//"+ sendByBulkVM.UploadedFile.FileName;
                        Console.WriteLine(path2);
                        content += "<br>Attached herewith is a document: " + "<a href=\"" + path2 + "\">" + "link" + " </a></p>";
                    }
                        
                    
                    
                    var subject = sendByBulkVM.Subject;
                    await _send.SendEmailAsync(user.FirstName, senderEmail, user.Email, content, subject);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Json(Ok());
        }


        [HttpPost("SendTest/{id}")]
        public async Task<IActionResult> SendTest(int id)
        {

            try
            {
                if (!_context.Users.Any(u => u.UserId == id))
                {
                    return Json(NotFound());
                }

                var user = _context.Users.FirstOrDefault(u => u.UserId == id);
                string senderEmail = _configuration.GetSection("AdminEmailCredentials:SenderEmail").Value;

                if (!_context.Messages.Any(m => m.Position == user.PositionDesiredId))
                {
                    return Json("False");
                }
                
                var content = "Link to test: _____________";
                var subject = "You are invited to take an online test.";

                await _send.SendEmailAsync(user.FirstName, senderEmail, user.Email, content, subject);
            }
            catch (Exception)
            {

                throw;
            }

            return Json(Ok());
        }


        [HttpPost("SendTestByBulk")]
        public async Task<IActionResult> SendTestbyBulk([FromBody] SendInvitationByBulkViewModel sendByBulkVM)
        {
            string[] userIds = null;
            userIds = sendByBulkVM.ids.Split(",");

            
            try
            {
                for (int i = 0; i < userIds.Length; i++)
                {
                    var is_user_exist = _context.Users.Any(u => u.UserId == Int32.Parse(userIds[i]));
                    if (!is_user_exist)
                    {
                        return Json(StatusCode(404));
                    }

                    var user = _context.Users.FirstOrDefault(u => u.UserId == Int32.Parse(userIds[i]));
                    string senderEmail = _configuration.GetSection("AdminEmailCredentials:SenderEmail").Value;
                    
                    var content = "";
                    var subject = "You are invited to take an online test.";

                    
                     
                    await _send.SendEmailAsync(user.FirstName, senderEmail, user.Email, content, subject);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Json(Ok());
        }

    }
}