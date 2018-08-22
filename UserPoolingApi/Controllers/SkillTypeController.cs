using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPoolingApi.EntityFramework;
using UserPoolingApi.Models;
using UserPoolingApi.ViewModels;

namespace UserPoolingApi.Controllers
{
    [Authorize]
    [Route("SkillType")]
    public class SkillTypeController : Controller
    {
        private readonly IMapper _map;
        private DataContext _context;

        public SkillTypeController(IMapper map, DataContext context)
        {
            _map = map;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllSkillType()
        {
            return Json(Ok(_map.Map<IEnumerable<SkillTypeViewModel>>(_context.SkillTypes)));
        }

        [HttpPost]
        public IActionResult CreateSkillType([FromBody] SkillTypeViewModel skillTypeVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(BadRequest());
            }

            if (_context.SkillTypes.Any(st => st.SkillTypeName == skillTypeVM.SkillTypeName))
            {
                return Json(StatusCode(409, "Skill type name, '" + skillTypeVM.SkillTypeName + "', is already exisiting in the database."));
            }

            var skilltype = _map.Map<SkillType>(skillTypeVM);
            _context.SkillTypes.Add(skilltype);
            _context.SaveChanges();

            return Json(Ok());
        }

        [HttpGet("{id}")]
        public IActionResult GetSkillTypeById(int id)
        {
            if (!_context.SkillTypes.Any(st => st.SkillTypeId == id))
            {
                return Json(NotFound());
            }
            var skilltype = _context.SkillTypes.FirstOrDefault(st => st.SkillTypeId == id);
            return Json(Ok(skilltype));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSkillType([FromBody] SkillTypeViewModel skillTypeVM, int id)
        {
            if (!ModelState.IsValid)
            {
                return Json(BadRequest());   
            }

            if (!_context.SkillTypes.Any(st => st.SkillTypeId == id))
            {
                return Json(StatusCode(404, "SkillType Id " + id + " does not exist in the database."));
            }

            var skilltype = _context.SkillTypes.FirstOrDefault(st => st.SkillTypeId == id);
            skilltype.SkillTypeName = skillTypeVM.SkillTypeName;
            _context.SkillTypes.Update(skilltype);
            _context.SaveChanges();
            return Json(Ok());
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSkillType(int id)
        {
            if (!_context.SkillTypes.Any(st => st.SkillTypeId == id))
            {
                return Json(StatusCode(404, "SkillType Id " + id + " does not exist in the database."));
            }

            var skilltype = _context.SkillTypes.FirstOrDefault(st => st.SkillTypeId == id);
            _context.SkillTypes.Remove(skilltype);
            return Json(NotFound());
        }
    }
}