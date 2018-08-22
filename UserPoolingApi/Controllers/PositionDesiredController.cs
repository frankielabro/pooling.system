using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPoolingApi.Services;
using UserPoolingApi.Models;
using UserPoolingApi.ViewModels;
using UserPoolingApi.EntityFramework;

namespace UserPoolingApi.Controllers
{
    [Authorize]
    [Route("PositionDesired")]
    [Produces("application/json")]
    public class PositionDesiredController : Controller
    {
        private readonly IMapper _map;
        private readonly DataContext _context;

        public PositionDesiredController(DataContext context, IMapper map) {
            _context = context;
            _map = map;
            
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllPositionDesired() {
            return Json(Ok(_map.Map<IEnumerable<PositionDesiredViewModel>>(_context.PositionDesired)));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetPositionDesiredById(int id)
        {
            try
            {
                if (!_context.PositionDesired.Any(p => p.PositionDesiredId == id)) {

                    return Json(NotFound("Position Desired with an id " + id +" does not exist."));
                }

                var position = _context.PositionDesired.FirstOrDefault(p => p.PositionDesiredId == id);
                return Json(Ok(position));
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPost]
        public IActionResult CreatePositionDesired([FromBody] PositionDesiredViewModel positionDesiredVM) {

            try
            {
                if (!ModelState.IsValid) {
                    return Json(BadRequest());
                }

                if (_context.PositionDesired.Any(p => p.PositionName == positionDesiredVM.PositionName)) {
                    return Json(StatusCode(409,"Position desired name, '"+positionDesiredVM.PositionName +"', is already existing in the database."));
                }
                var positionDesired = _map.Map<PositionDesired>(positionDesiredVM);
                _context.Add(positionDesired);
                _context.SaveChanges();
                return Json(Ok());
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePositionDesired([FromBody] PositionDesiredViewModel positionDesiredVM, int id) {
            try
            {
                if (!ModelState.IsValid) {
                    return Json(BadRequest());
                }

                if (_context.PositionDesired.Any(p => p.PositionName == positionDesiredVM.PositionName)) {
                    return Json(StatusCode(409, "Position desired name, '" + positionDesiredVM.PositionName + "', is already existing in the database."));
                }
                var position = _context.PositionDesired.FirstOrDefault(p => p.PositionDesiredId == id);
                position.PositionName = positionDesiredVM.PositionName;
                _context.Update(position);
                _context.SaveChanges();
                return Json(Ok());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePositionDesired(int id) {

            if (!_context.PositionDesired.Any(p => p.PositionDesiredId == id)) {

                return Json(NotFound("Position Desired with an id " + id + " does not exist."));
            }

            var position = _context.PositionDesired.FirstOrDefault(p => p.PositionDesiredId == id);
            _context.PositionDesired.Remove(position);
            _context.SaveChanges();
            return Json(NoContent());
        }

         


    }
}