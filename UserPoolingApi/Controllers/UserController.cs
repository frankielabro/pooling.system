using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserPoolingApi.Services;
using UserPoolingApi.Enums;
using UserPoolingApi.Helper;
using UserPoolingApi.Models;
using UserPoolingApi.Models.Enums;
using UserPoolingApi.ViewModels;
using UserPoolingApi.EntityFramework;
using Newtonsoft.Json;

namespace UserPoolingApi.Controllers
{
    [Produces("application/json")]
    [Route("User")]
    [Authorize]
    public class UserController : Controller
    {
        private DataContext _context;
        private readonly SmtpHelper _send;
        private readonly IMapper _map;
        private readonly IGenerateCV _dataTable;
       // private readonly ILogWriter _logWriter;
        public static IConfiguration _configuration { get; set; }

        public UserController(IMapper map, DataContext context, SmtpHelper send, IConfiguration configuration, IGenerateCV dataTable)
        {
            _context = context;
            _send = send;
            _map = map;
            _configuration = configuration;
            //_logWriter = logWriter;
            _dataTable = dataTable;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _context.Users.Include("UserSkills").Include("PositionDesired").Include("UserSkills.Skill").Include("Survey");
                return Json(Ok(_map.Map<IEnumerable<DisplayUserViewModel>>(users)));
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("PaginateUser")]
        public IActionResult GetPage(int pageNumber = 1, int pageSize = 1, string searchFirstName = "")
        {
            PagedParamModel param = new PagedParamModel()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                searchValue = searchFirstName
            };
            Expression<Func<User, bool>> searchFunc = (s => s.FirstName.ToLower().Contains(searchFirstName.ToLower()));
            var users = _context.Users.Include("UserSkills").Include("UserSkills.Skill").ToPagedList(param, filter: !string.IsNullOrEmpty(searchFirstName) ? searchFunc : null, orderBy: null);
            var results = _map.Map<PageResultViewModel>(users);
            return Json(Ok(results));
        }

        [HttpGet]
        [Route("TestResult")]
        public IActionResult GetTestResults()
        {
            try
            {
                //var users = _context.UserTests.Include("Test").Include("Test.TestType").Include("User");

                //var usersMapped = _map.Map<IEnumerable<DisplayUserTestViewModel>>(users);

                //foreach (var user in usersMapped)
                //{
                //    var testtype = _context.TestTypes.FirstOrDefault(tt => tt.TestTypeId == user.TestId);
                //    user.TestType = testtype.Description;
                //}

                //return Json(usersMapped);

                var users = _context.Users.Include("UserTest").Include("UserTest.Test.TestType");
                var usersMapped = _map.Map<IEnumerable<UserUserTestViewModel>>(users);
               
                return Json(usersMapped);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { ex.Message });
            }
        }

        [HttpGet]
        [Route("DownloadFile/{userId}")]
        public async Task<IActionResult> DLFile(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            string fileName = user.UploadedCV;
            string path = Path.Combine("C:\\UploadFiles", userId.ToString());
            if (!Directory.Exists(path))
            {
                return Json(NotFound("No files found."));
            }
            string file = Path.Combine(path, fileName);
            string contentType = FileHelper.GetContentType(file);
            var memory = await FileHelper.CreateMemoryStream(file);
            return File(memory, contentType, Path.GetFileName(file));
        }

        [HttpGet("Status")]
        public IActionResult GetStatus()
        {
            List<UserStatusModel> status = ((StatusEnum[])Enum.GetValues(typeof(StatusEnum))).Select(c => new UserStatusModel() { Value = (int)c, StatusName = c.ToString().ToSentenceCase() }).ToList();
            return Json(Ok(status));
        }

        [AllowAnonymous]
        [HttpGet("Position")]
        public IActionResult GetPosition()
        {
            var positions = _context.PositionDesired;
            return Json(positions);
        }

        //Update Status
        [HttpGet("Status/{id}/{status}")]
        public IActionResult UpdateStatus(int id, string status)
        {
            status = status.ToPascalCase();
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return Json(BadRequest("User with ID " + id + " is not found"));
            }
            user.Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), status);
            user.StatusDate = DateTime.Now.AddHours(8);
            _context.Users.Update(user);
            _context.SaveChanges();
            return Json(NoContent());
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            //Investigate: Joining Tables
            var user = _context.Users.Include("UserSkills").Include("UserSkills.Skill").Include("UserSkills.Skill.SkillType").FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return Json(NotFound("User with Id " + id + " is not found"));
            }

            return Json(Ok(_map.Map<DisplayUserViewModel>(user)));
            //return Json(Ok(user));

        }


        [HttpGet("VerifyEmail/{email}")]
        [AllowAnonymous]
        public IActionResult CheckEmail(string email)
        {
            var is_email_exist = _context.Users.Any(u => u.Email == email && u.IsOutsider == 0);
            if (is_email_exist)
            {
                return Json(StatusCode(409));
            }
            return Json(Ok());
        }


        [HttpPost]/*The action below will be triggered after pressing the Yes button at the Confimation Page*/
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync([FromBody] UserViewModel userVM)
        {
            try
            {
                if (!ModelState.IsValid) return Json(userVM);

                // email validation
                if (_context.Users.Any(u => u.Email == userVM.Email && u.IsOutsider == 0))
                    return Json(StatusCode(409, "Email already exists in the database"));

                //Fixing the first name and last name casing. 
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                userVM.FirstName = userVM.FirstName.ToLower();
                userVM.FirstName = textInfo.ToTitleCase(userVM.FirstName.Trim());
                userVM.LastName = userVM.LastName.ToLower();
                userVM.LastName = textInfo.ToTitleCase(userVM.LastName.Trim());

                User user;
                //if Outsider is equal to 1, which means he has taken the test but not yet registered.
                if (_context.Users.Any(u => u.Email == userVM.Email && u.IsOutsider == 1))
                {
                    user = _context.Users.FirstOrDefault(u => u.Email == userVM.Email);
                    user.IsOutsider = 0;
                    userVM.UserId = user.UserId;
                    _map.Map(userVM, user);
                }
                else
                {
                    user = _map.Map<User>(userVM);
                    user.SubmittedDate = DateTime.Now.AddHours(8);
                    _context.Add(user);
                }

                _context.SaveChanges();
                foreach (var customskill in user.CustomSkills)
                {
                    customskill.DateAdded = DateTime.Now.AddHours(8);
                }
                //Creation of physical directory in the the unit.
                var dir = _configuration.GetSection("Directory:ForLocalGeneratedCV").Value;
                var path = Path.Combine(dir, user.UserId.ToString(), user.FirstName + "_" + user.LastName.TrimStart().Substring(0, 1) + "_CV.pdf");
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                user.CVTemplate = path;
                _context.Update(user);
                _context.SaveChanges();

                var UserId = user.UserId;
                _dataTable.CreatePDF(userVM, UserId);

                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                await _send.SendEmailAsync(userVM.FirstName, _configuration.GetSection("AdminEmailCredentials:SenderEmail").Value, userVM.Email,
                    "Thank you for submitting your profile to Dev Partners. We will review your CV and contact you for current openings.", "Application received.");
                
                var admins = _context.Admins;
                var content = user.FirstName + " " + user.LastName + " has submitted application to the Talent Pool on " + user.SubmittedDate;
                if (admins.Count() > 0)
                {
                    foreach (var a in admins)
                    {
                        var adminEmail = a.Email;
                        await _send.SendEmailAsync("Admin", _configuration.GetSection("AdminEmailCredentials:SenderEmail").Value, adminEmail,
                        content, "New application received on the Pooling System.");
                    }
                }

                return Json(StatusCode(201, "Profile info is successfully sent to the database"));
            }
            //THE FOLLOWING CODE WILL GIVE YOU MORE ACCURATE DESCRIPTION ABOUT THE ERROR IN THE INTERNAL SIDE.
            catch (Exception ex)
            {
                LogWrite(ex.ToString());
                return Json(new { ex.Message });
            }
        }

        [HttpPut("UploadFile")]/*The action below will be triggered after pressing the Yes button at the Confimation Page*/
        [AllowAnonymous]
        public IActionResult CreateAsync2(UserViewModel2 userVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    //return BadRequest(ModelState);
                    return Json(StatusCode(400));
                }

                var user = _context.Users.FirstOrDefault(u => u.Email == userVM.Email);
                user.UploadedCV = userVM.UploadedCV.FileName;
                //Create the CV
                // Creating a folder where the uploaded file will be stored.
                if (userVM.UploadedCV == null || userVM.UploadedCV.Length == 0)
                    return Json(Content("file not selected"));

                var dir = _configuration.GetSection("Directory6:ForLocalUploadedCV").Value;

                var path = Path.Combine(dir,
                            user.UserId.ToString(), userVM.UploadedCV.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    userVM.UploadedCV.CopyTo(stream);
                }

                _context.Update(user);
                _context.SaveChanges();

                return Json(StatusCode(201));
            }
            //THE FOLLOWING CODE WILL GIVE YOU MORE ACCURATE DESCRIPTION ABOUT THE ERROR IN THE INTERNAL SIDE.
            catch (Exception ex)
            {
                LogWrite(ex.ToString());
                return Json(new {ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("Survey")]
        public IActionResult Survey([FromBody] SurveyViewModel surveyVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(StatusCode(400));
            }
            if (!_context.Users.Any(u => u.Email == surveyVM.Email))
            {
                return Json(NotFound("Email not found"));
            }
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == surveyVM.Email);
                surveyVM.UserId = user.UserId;
                Survey survey;
                survey = _map.Map<Survey>(surveyVM);
                _context.Add(survey);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogWrite(ex.ToString());
                return Json(new { ex.Message });
            }
            return Json(Ok());
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserViewModel userVM)
        {
            try
            {
                var user = _context.Users.Include("UserSkills").FirstOrDefault(u => u.UserId == id);
                if (user == null)
                {
                    return Json(NotFound("User ID with " + id + " is not found"));
                }
                if (userVM.Email != null)
                {
                    if (_context.Users.Any(u => u.Email == userVM.Email))
                    {
                        if (_context.Users.FirstOrDefault(u => u.UserId == id).Email == userVM.Email)
                        {
                        }
                        else
                        {
                            return Json(StatusCode(409, "Email already exist."));
                        }
                    }
                }
                else
                {
                    userVM.Email = _context.Users.FirstOrDefault(u => u.UserId == id).Email;
                }
                var oldPath = user.CVTemplate;
                var dir = _configuration.GetSection("Directory:ForLocalGeneratedCV").Value;
                var newPath = dir + "\\" + id + "\\" + userVM.FirstName + "_" + userVM.LastName + "_CV.pdf";
                newPath = newPath.Replace(@"\\", @"\");
                newPath = newPath.Replace(@":\", @":\\");

                if (oldPath != newPath)
                {
                    System.IO.File.Move(oldPath, newPath);
                    user.CVTemplate = newPath;
                    _context.Update(user);
                    _context.SaveChanges();
                }
                userVM.UserId = id;
                _map.Map(userVM, user);
                _context.SaveChanges();

                return Json(new NoContentResult());
            }
            catch (Exception ex)
            {
                LogWrite(ex.Message);
                return Json(BadRequest(ex));
            }
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == id);
                if (user == null)
                {
                    return Json(NotFound("User with ID " + id + " is not found."));
                }
                user.IsActive = 1;
                _context.Update(user);
                _context.SaveChanges();
                return Json(new NoContentResult());
            }
            catch (Exception ex)
            {
                LogWrite(ex.Message);
                throw;
            }
        }

        [HttpPut("editUserSkill")]
        public IActionResult EditUserSkill([FromBody] EditUserSkillViewModel editUserSkillVM)
        {
            if (!_context.Users.Any(u => u.UserId == editUserSkillVM.UserId))
            {
                return Json(NotFound("UserId not found."));
            }

            if (!_context.UserSkills.Any(us => us.UserId == editUserSkillVM.UserId))
            {
                return Json(NotFound("UserSkillsId not found."));
            }

            var userskills = _context.UserSkills.First(us => us.UserId == editUserSkillVM.UserId);

            _map.Map(editUserSkillVM, userskills);
            _context.SaveChanges();

            return Json(Ok());
        }

        [HttpPost("addUserSkill")]
        public IActionResult addUserSkill([FromBody] EditUserSkillViewModel editUserSkillVM)
        {

            if (!ModelState.IsValid)
            {
                return Json(BadRequest("ModelState invalid"));
            }

            if (!_context.Users.Any(u => u.UserId == editUserSkillVM.UserId))
            {
                return Json(NotFound("UserId not found."));
            }


            UserSkills userSkill;

            userSkill = _map.Map<UserSkills>(editUserSkillVM);

            _context.Add(userSkill);
            _context.SaveChanges();

            return Json(Ok());
        }

        [HttpPut("deleteUser/{id}")]
        public IActionResult deleteUser(int userId)
        {
            if (!_context.Users.Any(u => u.UserId == userId))
            {
                return Json(NotFound("userId not found"));
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            user.IsActive = 1;

            _context.Update(user);
            _context.SaveChanges();

            return Json(StatusCode(204));
        }

        [HttpPut("restoreUser/{id}")]
        public IActionResult restoreUser(int userId)
        {
            if (!_context.Users.Any(u => u.UserId == userId))
            {
                return Json(NotFound("userId not found"));
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            user.IsActive = 0;

            _context.Update(user);
            _context.SaveChanges();

            return Json(StatusCode(204));
        }

        [AllowAnonymous]
        [HttpGet("getDetails/{email}")]
        public IActionResult getUserForOnlineTest(string email)
        {
            if (!_context.Users.Any(u => u.Email == email))
            {
                var _emailStatus = 0;
                var _data = new { emailStatus = _emailStatus };
                return Json(_data);
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            var fullname = user.FirstName + " " + user.LastName;
            var positiondesired = user.PositionDesiredId;

            var data = new { EmailStatus = 1, FullName = fullname, PositionDesired = positiondesired };

            return Json(data);
        }

        [HttpPost("CheckEmail")]//inform Fritz about this that this has been changed to Core Values Test
        public IActionResult PostCheckEmail(UserOnlineTestViewModel userVM)
        {
            //return Ok("something");
            if (!ModelState.IsValid)
            {
                return BadRequest(userVM);
            }
            User user;
            //If the user does exist in the User Table
            if (_context.Users.Any(u => u.Email == userVM.UserEmail && u.IsOutsider == 0))
            {
                user = _context.Users.FirstOrDefault(u => u.Email == userVM.UserEmail);
                //If the user does exist in the UserTest with TestId = 6 (CORE VALUES TEST) then, return the score
                if (_context.UserTests.Any(ut => ut.UserId == user.UserId && ut.TestId == 6))
                {
                    var usertest = _context.UserTests.FirstOrDefault(ut => ut.UserId == user.UserId && ut.TestId == 6);
                    var result = new { emailStatus = 1, IsTaken = 1, CoreValuesTestScore = usertest.Score }; //inform Fritz about this changes in return
                    return Json(result);
                }
            }
            //if the User does not exist in the database-table User.
            else
            {
                try
                {
                    user = _map.Map<User>(userVM);
                    user.IsOutsider = 1;
                    _context.Add(user);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { ex.Message });
                }
            }
            //If the user exist or has just been added, but has not yet taken the Core Values Test;
            try
            {
                Test test2;
                test2 = _context.Test
                .Include("Question")
                .Include("Question.Choices")
                .FirstOrDefault(t => t.TestTypeId == 4); //TestTypeId = 4 -> Core Values Test
                TestViewModel testVM = _map.Map<TestViewModel>(test2);
                testVM.UserId = user.UserId;
                testVM.emailStatus = 1;
                testVM.isTaken = 0;
                return Json(testVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Statistics")]
        public IActionResult GetStatistics()
        {
            int technicalpassingscore = 10;
            var users = _context.Users.ToList();
            var usersSurvey = _context.Survey.ToList();
            var usersTest = _context.UserTests.ToList();

            string[,] statArray = new string[5,2];

            StatisticsViewModel statisticVM = new StatisticsViewModel
            {
                NumberOfApplicants = _context.Users.Count(u => u.IsOutsider == 0),
                
                HomeBased = usersSurvey.Count(s => s.PreferredWorkplace == "Home Based"),
                OfficeBased = usersSurvey.Count(s => s.PreferredWorkplace == "Office Based"),
                
                FacebookAds = usersSurvey.Count(s => s.ReachedBy == "Facebook Ads"),
                FacebookGroup = usersSurvey.Count(s => s.ReachedBy.Contains("Facebook Group")),
                LinkedIn = usersSurvey.Count(s => s.ReachedBy == "Linkedln"),
                Referral = usersSurvey.Count(s => s.ReachedBy == "Referral"),
                
                FrontendDeveloper = users.Count(u => u.PositionDesiredId == 1 && u.IsOutsider == 0),
                BackendDeveloper = users.Count(u => u.PositionDesiredId == 2),
                DatabaseDeveloper = users.Count(u => u.PositionDesiredId == 3),
                MobileDeveloper = users.Count(u => u.PositionDesiredId == 4),
                FullStackWebDeveloper = users.Count(u => u.PositionDesiredId == 5),
                UXUIDesigner = users.Count(u => u.PositionDesiredId == 6),
                VirtualAssistant = users.Count(u => u.PositionDesiredId == 7),

                SeniorFrontendDeveloper = users.Count(u => u.PositionDesiredId == 11),
                SeniorBackendDeveloper = users.Count(u => u.PositionDesiredId == 12),
                SeniorDatabaseDeveloper = users.Count(u => u.PositionDesiredId == 13),
                SeniorFullStackDeveloper = users.Count(u => u.PositionDesiredId == 14),
                SeniorMobileDeveloper = users.Count(u => u.PositionDesiredId == 15),
                
                LeadFrontendDeveloper = users.Count(u => u.PositionDesiredId == 16),
                LeadBackendDeveloper = users.Count(u => u.PositionDesiredId == 17),
                LeadDatabaseDeveloper = users.Count(u => u.PositionDesiredId == 18),
                LeadFullStackDeveloper = users.Count(u => u.PositionDesiredId == 19),
                LeadMobileDeveloper = users.Count(u => u.PositionDesiredId == 20),

                
                FrontEndTest = usersTest.Count(ut => ut.TestId == 1),
                FrontEndTestPassed = usersTest.Count(ut => ut.TestId == 1 && ut.Score >= technicalpassingscore),
                FrontEndTestFailed = usersTest.Count(ut => ut.TestId == 1 && ut.Score < technicalpassingscore),
                
                BackendTest = usersTest.Count(ut => ut.TestId == 2),
                BackendTestPassed = usersTest.Count(ut => ut.TestId == 2 && ut.Score >= technicalpassingscore),
                BackendTestFailed = usersTest.Count(ut => ut.TestId == 2 && ut.Score < technicalpassingscore),
                
                DatabaseTest = usersTest.Count(ut => ut.TestId == 3),
                DatabaseTestPassed = usersTest.Count(ut => ut.TestId == 3 && ut.Score >= technicalpassingscore),
                DatabaseTestFailed = usersTest.Count(ut => ut.TestId == 3 && ut.Score < technicalpassingscore),
                
                CoreValuesTest = usersTest.Count(ut => ut.TestId == 4),
                CoreValuesTestPassed = usersTest.Count(ut => ut.TestId == 4 && ut.Score >= 7),
                CoreValuesTestFailed = usersTest.Count(ut => ut.TestId == 4 && ut.Score < 7),
                
                EnglishTest = usersTest.Count(ut => ut.TestId == 5),
                EnglishTestPassed = usersTest.Count(ut => ut.TestId == 5 && ut.Score >= technicalpassingscore),
                EnglishTestFailed = usersTest.Count(ut => ut.TestId == 5 && ut.Score < technicalpassingscore)
                
            };


            statisticVM.Others = statisticVM.NumberOfApplicants - (
                statisticVM.FacebookAds + statisticVM.FacebookGroup + statisticVM.LinkedIn + statisticVM.Referral);

            //Passing Rates
            statisticVM.FrontEndTestPassingRate = statisticVM.FrontEndTestPassed / statisticVM.FrontEndTest * 100;
            statisticVM.BackendTestPassingRate = statisticVM.BackendTestPassed / statisticVM.BackendTest * 100;
            statisticVM.DatabaseTestPassingRate = statisticVM.DatabaseTestPassed / statisticVM.DatabaseTest * 100;
            statisticVM.CoreValuesTestPassingRate = statisticVM.CoreValuesTestPassed / statisticVM.CoreValuesTest * 100;
            statisticVM.EnglishTestPassingRate = statisticVM.EnglishTestPassed / statisticVM.EnglishTest * 100;

            var results = new List<Result>()
                {
                    new Result { description = "Number of Applicants", value = statisticVM.NumberOfApplicants, group = 0 },

                    new Result { description = "Home Based", value = statisticVM.HomeBased, group = 1 },
                    new Result { description = "Office Based", value = statisticVM.OfficeBased, group = 1 },

                    new Result { description = "Facebook Ads", value = statisticVM.FacebookAds, group = 2 },
                    new Result { description = "Facebook Group", value = statisticVM.FacebookGroup, group = 2 },
                    new Result { description = "LinkedIn", value = statisticVM.LinkedIn, group = 2 },
                    new Result { description = "Referral", value = statisticVM.Referral, group = 2 },
                    new Result { description = "Others", value = statisticVM.Others, group = 2 },

                    new Result { description = "Frontend Developer", value = statisticVM.FrontendDeveloper, group = 3 },
                    new Result { description = "Backend Developer", value = statisticVM.BackendDeveloper, group = 3 },
                    new Result { description = "Database Developer", value = statisticVM.DatabaseDeveloper, group = 3 },
                    new Result { description = "Mobile Developer", value = statisticVM.MobileDeveloper, group = 3 },
                    new Result { description = "Full Stack Web Developer", value = statisticVM.FullStackWebDeveloper, group = 3 },
                    new Result { description = "UX/UI Designer", value = statisticVM.UXUIDesigner, group = 3 },
                    new Result { description = "Virtual Assistant", value = statisticVM.VirtualAssistant, group = 3 },

                    new Result { description = "Senior Frontend Developer", value = statisticVM.SeniorFrontendDeveloper, group = 3 },
                    new Result { description = "Senior Backend Developer", value = statisticVM.SeniorBackendDeveloper, group = 3 },
                    new Result { description = "Senior Database Developer", value = statisticVM.SeniorDatabaseDeveloper, group = 3 },
                    new Result { description = "Senior Full Stack Developer", value = statisticVM.SeniorFullStackDeveloper, group = 3 },
                    new Result { description = "Senior Mobile Developer", value = statisticVM.SeniorMobileDeveloper, group = 3 },

                    new Result { description = "Lead Frontend Developer", value = statisticVM.LeadFrontendDeveloper, group = 3 },
                    new Result { description = "Lead Backend Developer", value = statisticVM.LeadBackendDeveloper, group = 3 },
                    new Result { description = "Lead Database Developer", value = statisticVM.LeadDatabaseDeveloper, group = 3 },
                    new Result { description = "Lead Full Stack Developer", value = statisticVM.LeadFullStackDeveloper, group = 3 },
                    new Result { description = "Lead Mobile Developer", value = statisticVM.LeadMobileDeveloper, group = 3 },

                    new Result { description = "Front End Test", value = statisticVM.FrontEndTest, group = 6 },
                    new Result { description = "Front End Test Passed", value = statisticVM.FrontEndTestPassed, group = 6 },
                    new Result { description = "Front End Test Failed", value = statisticVM.FrontEndTestFailed, group = 6 },
                    new Result { description = "Front End Test Passing Rate", value = statisticVM.FrontEndTestPassingRate, group = 6 },

                    new Result { description = "Backend Test", value = statisticVM.BackendTest, group = 7 },
                    new Result { description = "Backend Test Passed", value = statisticVM.BackendTestPassed, group = 7 },
                    new Result { description = "Backend Test Failed", value = statisticVM.BackendTestFailed, group = 7 },
                    new Result { description = "Backend Test Passing Rate", value = statisticVM.BackendTestPassingRate, group = 7 },

                    new Result { description = "Database Test", value = statisticVM.DatabaseTest, group = 8 },
                    new Result { description = "Database Test Passed", value = statisticVM.DatabaseTestPassed, group = 8 },
                    new Result { description = "Database Test Failed", value = statisticVM.DatabaseTestFailed, group = 8 },
                    new Result { description = "Database Test Passing Rate", value = statisticVM.DatabaseTestPassingRate, group = 8 },

                    new Result { description = "Core Values Test", value = statisticVM.CoreValuesTest, group = 9 },
                    new Result { description = "Core Values TestPassed", value = statisticVM.CoreValuesTestPassed, group = 9 },
                    new Result { description = "Core Values TestFailed", value = statisticVM.CoreValuesTestFailed, group = 9 },
                    new Result { description = "Core Values TestPassing Rate", value = statisticVM.CoreValuesTestPassingRate, group = 9 },

                    new Result { description = "English Test", value = statisticVM.EnglishTest, group = 10 },
                    new Result { description = "English Test Passed", value = statisticVM.EnglishTestPassed, group = 10 },
                    new Result { description = "English Test Failed", value = statisticVM.EnglishTestFailed, group = 10 },
                    new Result { description = "English Test Passing Rate", value = statisticVM.EnglishTestPassingRate, group = 10 }
                };
            
            return Json(new { survey = results });
            
            
        }

        public void LogWrite(string logMessage)
        {

            var dir = _configuration.GetSection("Directory7:ForErrorLogs").Value;

            try
            {
                using (StreamWriter w = System.IO.File.AppendText(dir + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

        public class Result
        {
            public string description { get; set; }
            public double value { get; set; }
            public int group { get; set; }

        }

    }
}