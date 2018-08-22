using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserPoolingApi.EntityFramework;
using UserPoolingApi.Models;
using UserPoolingApi.ViewModels;
using UserPoolingApi.Helper;
using Microsoft.Extensions.Configuration;

namespace UserPoolingApi.Controllers
{
    [Produces("application/json")]
    [Route("Test")]
    public class TestController : Controller
    {
        private DataContext _context;
        private IMapper _map;
        private readonly SmtpHelper _send;
        public static IConfiguration _configuration { get; set; }

        public TestController(DataContext context, IMapper map, SmtpHelper send, IConfiguration configuration)
        {
            _context = context;
            _map = map;
            _send = send;
            _configuration = configuration;
        }

        [HttpPost("Submit")]//Inform Fritz about this
        public async Task<IActionResult> SubmitAnswers([FromBody] SubmitAnswerViewModel submitAnswerVM)
        {
            //if the user has already taken a specified exam.
            if (_context.UserTests.Any(ut => (ut.UserId == submitAnswerVM.UserId && ut.TestId == submitAnswerVM.TestId && ut.IsSubmit == 1)))
            {
                var data2 = new { emailStatus = 1, isTaken = 1 };
                return Json(data2);
            }
            double score = 0;
            int negative = 0;

            if (submitAnswerVM.TestId == 4)
            {
                foreach (var userAnswer in submitAnswerVM.Answers)
                {
                    Question question = _context.Questions.FirstOrDefault(q => q.QuestionId == userAnswer.QuestionId);
                    Choice choice = _context.Choices.FirstOrDefault(c => c.ChoiceId == userAnswer.ChoiceId);
                    if (choice.Value == 1) score++;
                    else if (choice.Value == -1) negative++;
                    else continue;
                }

                if (negative > 1) score = -1; //meaning he failed the test.
            }
            else
            {
                foreach (var userAnswer in submitAnswerVM.Answers)
                {
                    Question question = _context.Questions.FirstOrDefault(q => q.QuestionId == userAnswer.QuestionId);
                    if (userAnswer.ChoiceId == question.CorrectAnswer) score++;
                    else continue;
                }
            }

            int scoreForDB = Convert.ToInt32(score);

            //UPDATE THE TABLE HERE
            var _usertest = _context.UserTests.FirstOrDefault(ut => ut.UserId == submitAnswerVM.UserId && ut.TestId == submitAnswerVM.TestId);
            _map.Map(submitAnswerVM, _usertest);

            _usertest.Score = scoreForDB;
            _usertest.IsTaken = 1;
            _usertest.IsSubmit = 1;
            _usertest.DateTaken = DateTime.Now.AddHours(8);
            
            _context.Update(_usertest);
            _context.SaveChanges();

            //send a confirmation email to admin
            try
            {
                var admins = _context.Admins;
                var user = _context.Users.FirstOrDefault(u => u.UserId == submitAnswerVM.UserId);
                var test = _context.Test.FirstOrDefault(t => t.TestId == submitAnswerVM.TestId);
                var testtype = _context.TestTypes.FirstOrDefault(tt => tt.TestTypeId == test.TestTypeId);

                var content = "";


                if (submitAnswerVM.TestId == 4)
                {
                    string passFail = "";
                    if (score > 6) passFail = "passed";
                    else passFail = "failed";

                    content = user.Email + " has " + passFail + " the exam on " + testtype.Description + " with a score of " + score + " on " + DateTime.Now.AddHours(8);
                }
                else
                {
                    var CountQuestions = _context.Questions.Count(q => q.TestId == submitAnswerVM.TestId);
                    double scorePercentages = (score / CountQuestions) * 100;

                    content = user.Email + " has taken an exam on " + testtype.Description + " with a score of " + scorePercentages + "% on " + DateTime.Now.AddHours(8);
                }

                if (admins.Count() > 0)
                {
                    foreach (var a in admins)
                    {
                        var adminEmail = a.Email;
                        await _send.SendEmailAsync("Admin", _configuration.GetSection("AdminEmailCredentials:SenderEmail").Value, adminEmail,
                        content, "Applicant Online Examination");
                    }
                }
            }
            catch (Exception)
            {
                return Json(StatusCode(500, "Error in sending email to admins"));
            }
            
            return Ok();
           
        }

        [HttpPost]
        public IActionResult GetTest([FromBody] GetTestViewModel getTestVM)
        {
            //new implementation

            if (getTestVM.email == null)
            {
                return BadRequest("Email is null");
            }

            var user = new User();

            //if there is no existing user with the following email then ask for full name
            if (!_context.Users.Any(u => u.Email == getTestVM.email))
            {
                user.IsOutsider = 1;
                user.Email = getTestVM.email;
                user.PositionDesiredId = 1;
                user.SubmittedDate = DateTime.Now.AddHours(8);
                _context.Add(user);
                _context.SaveChanges();

            }

            user = _context.Users.FirstOrDefault(u => u.Email == getTestVM.email);

            //user exist
            //if the user already took the test 
            if (_context.UserTests.Any(ut => ut.UserId == user.UserId && ut.TestId == getTestVM.testId))
            {
                return Json(new {emailStatus = 1, isTaken =1}); 
            }

            //if the user has not yet taken the test, return the questions

            if (!_context.Test.Any(t => t.TestId == getTestVM.testId))
            {
                return NotFound("No test available yet for that kind of test.");
            }
            try
            {
                var _usertest = _map.Map<UserTest>(getTestVM);
                _usertest.DateTaken = DateTime.Now.AddHours(8);
                _usertest.UserId = user.UserId;
                _usertest.IsTaken = 1;
                _usertest.TestId = getTestVM.testId;
                _context.Add(_usertest);
                _context.SaveChanges();


                Test test = _context.Test
                .Include("Question")
                .Include("Question.Choices")
                .FirstOrDefault(t => t.TestId == getTestVM.testId);
                
                TestViewModel testVM = _map.Map<TestViewModel>(test);
                testVM.UserId = user.UserId;
                testVM.emailStatus = 1;
                testVM.isTaken = 0;
                return Json(testVM);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { ex.Message });
            }
        }

        [HttpGet("GetTestTypes")]
        public IActionResult GetTestTypes()
        {
            var testTypes = _context.TestTypes;
            return Json(testTypes);
        }

        


    }
}