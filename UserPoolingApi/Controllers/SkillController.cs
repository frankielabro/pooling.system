using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserPoolingApi.Services;
using UserPoolingApi.Models;
using UserPoolingApi.ViewModels;
using UserPoolingApi.EntityFramework;

namespace UserPoolingApi.Controllers
{
    [Produces("application/json")]
    [Route("Skill")]
    public class SkillController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _map;

        public SkillController(DataContext context, IMapper map)
        {
            _context = context;
            this._map = map;
        }

        [HttpGet]
        public IActionResult getAllSkills()
        {
            var skills = _context.Skills.Include("SkillType");
            return  Ok(_map.Map<IEnumerable<SkillViewModel>>(skills));
            
        }  

        [HttpGet("{id}")]
        public IActionResult getById(int id)
        {
            try
            {
                var skill = _context.Skills.Include("SkillType").FirstOrDefault(s => s.SkillId == id);
                if (skill == null)
                {
                    return Json(NotFound("Skill ID with " + id + " is not found."));
                }

                return Json(Ok(_map.Map<SkillViewModel>(skill)));
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] SkillViewModel skillVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(BadRequest());
                }

                var is_skill_exist = _context.Skills.Any(s => s.SkillName == skillVM.SkillName);

                if (is_skill_exist)
                {
                    return Json(StatusCode(409));
                }

                //added by F
                var newSkill = _map.Map<Skill>(skillVM);

                _context.Add(newSkill);
                _context.SaveChanges();

                return Json(NoContent());
            }
            catch (Exception ex)
            {
                return Json(BadRequest(ex));
            }            
        }

        [HttpPost("VerifySkill/{skillName}")]
        public IActionResult VerifySkill(string skillName)
        {
            if (_context.Skills.Any(s => s.SkillName == skillName))
            {
                return Json(StatusCode(409));
            }
            return Json(Ok());
        }
       
        [HttpPut("{id}")]
        public IActionResult UpdateSkillName(int id, [FromBody] SkillViewModel skillVM)
        {
            try
            {
                var skill = _context.Skills.Include("SkillType").FirstOrDefault(s => s.SkillId == id);
                if (skill == null)
                {
                    return Json(NotFound("Skill ID with " + id + " is not found"));
                }

                var is_skill_exist = _context.Skills.Any(s => s.SkillName == skillVM.SkillName);

                if (is_skill_exist)
                {
                    return Json(StatusCode(409));
                }

                skillVM.SkillId = id;
                _map.Map(skillVM, skill);
                _context.SaveChanges();

                return Json(new NoContentResult());
            }
            catch (Exception ex)
            {
                return Json(BadRequest(ex));
            }            
        }

        [HttpPut("{id}/{skillTypeId}")]
        public IActionResult UpdateSkillType(int id, int skillTypeId, [FromBody] UpdateSkillTypeViewModel updateSkillVM)
        {
            try
            {
                var skill = _context.Skills.Include("SkillType").FirstOrDefault(s => s.SkillId == id);
                if (skill == null)
                {
                    return Json(NotFound("Skill ID with " + id + " is not found"));
                }

                updateSkillVM.SkillId = id;
                _map.Map(updateSkillVM, skill);
                _context.SaveChanges();

                return Json(new NoContentResult());
            }
            catch (Exception ex)
            {
                return Json(BadRequest(ex));
            }
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var skill = _context.Skills.FirstOrDefault(s => s.SkillId == id);

                if (skill == null)
                {
                    return Json(NotFound("SKill ID with "+ id +" is not found"));
                }

                _context.Skills.Remove(skill);
                _context.SaveChanges();

                return Json(new NoContentResult());
            }
            catch (Exception ex)
            {
                return Json(BadRequest(ex));
            }
        }
    }
}