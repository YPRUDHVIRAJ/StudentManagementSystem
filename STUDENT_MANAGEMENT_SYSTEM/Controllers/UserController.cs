using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Context;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;
using StudentManagementSystem.ViewModel;
using StudentManagementSystem3.Models;
using System.Diagnostics;

namespace StudentManagementSystem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly FingerprintService _fingerprintService;

        public UserController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/Student
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(LoginRequestModel request)
        {
            // Validate the existence of related entities
            var user = await _context.UserInfo.FirstOrDefaultAsync(a => a.UserName == request.UserName &&
                a.Password == request.Password);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }
    }

   
}