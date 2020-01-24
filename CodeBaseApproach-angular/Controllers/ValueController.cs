using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBaseApproach_angular.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeBaseApproach_angular.Controllers
{
    public class ValueController : Controller
    {
        private readonly DataContext _context;
        public ValueController(DataContext context)
        {
            _context = context;
        }
        ////// GET: /<controller>/
        [HttpGet]
        [Route("/[controller]")]
        public async Task<IActionResult> getValues()
        {
            var values = await _context.ValuesAngular.ToListAsync();
            return Ok(values);
        }

        [HttpGet]
        [Route("/[controller]/{id}")]
        public async Task<IActionResult> getValues(int id)
        {
            var value = await _context.ValuesAngular.FirstOrDefaultAsync(item=>item.Id==id);
            return Ok(value);
        }
    }
}
