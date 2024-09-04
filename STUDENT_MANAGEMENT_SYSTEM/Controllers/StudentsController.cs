using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Context;
using StudentManagementSystem.Models;
using FingerprintPro.ServerSdk;
using FingerprintPro.ServerSdk.Api;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly FingerprintService _fingerprintService;

        public StudentController(ApplicationDbContext context, FingerprintService fingerprintService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _fingerprintService = fingerprintService ?? throw new ArgumentNullException(nameof(fingerprintService));
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .Include(s => s.YearOfJoining)
                .ToListAsync();
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .Include(s => s.YearOfJoining)
                .FirstOrDefaultAsync(s => s.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // GET: api/Student/departments
        [HttpGet("departments")]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        // GET: api/Student/courses/{departmentId}
        [HttpGet("courses/{departmentId}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByDepartment(int departmentId)
        {
            var courses = await _context.Courses
                .Where(c => c.DepartmentID == departmentId)
                .ToListAsync();

            if (!courses.Any())
            {
                return NotFound();
            }

            return courses;
        }

        // GET: api/Student/yearsofjoining
        [HttpGet("yearsofjoining")]
        public async Task<ActionResult<IEnumerable<YearOfJoining>>> GetYearsOfJoining()
        {
            return await _context.YearsOfJoining.ToListAsync();
        }

        // POST: api/Student
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            // Validate the existence of related entities
            var department = await _context.Departments.FindAsync(student.DepartmentID);
            if (department == null)
            {
                ModelState.AddModelError("DepartmentID", "Invalid DepartmentID.");
                return BadRequest(ModelState);
            }

            var course = await _context.Courses.FindAsync(student.CourseID);
            if (course == null)
            {
                ModelState.AddModelError("CourseID", "Invalid CourseID.");
                return BadRequest(ModelState);
            }

            var yearOfJoining = await _context.YearsOfJoining.FindAsync(student.YearOfJoiningID);
            if (yearOfJoining == null)
            {
                ModelState.AddModelError("YearOfJoiningID", "Invalid YearOfJoiningID.");
                return BadRequest(ModelState);
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentID }, student);
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest();
            }

            // Validate the existence of related entities
            var department = await _context.Departments.FindAsync(student.DepartmentID);
            if (department == null)
            {
                ModelState.AddModelError("DepartmentID", "Invalid DepartmentID.");
                return BadRequest(ModelState);
            }

            var course = await _context.Courses.FindAsync(student.CourseID);
            if (course == null)
            {
                ModelState.AddModelError("CourseID", "Invalid CourseID.");
                return BadRequest(ModelState);
            }

            var yearOfJoining = await _context.YearsOfJoining.FindAsync(student.YearOfJoiningID);
            if (yearOfJoining == null)
            {
                ModelState.AddModelError("YearOfJoiningID", "Invalid YearOfJoiningID.");
                return BadRequest(ModelState);
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Student/add
        [HttpPost("add")]
        public async Task<IActionResult> AddStudentWithFingerprint([FromBody] Student student)
        {
            // Capture fingerprint
            student.FingerprintData = _fingerprintService.CaptureFingerprint();

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }

        // POST: api/Student/validate
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateFingerprint([FromBody] byte[] fingerprintData)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.FingerprintData.SequenceEqual(fingerprintData));

            if (student == null)
                return NotFound();

            return Ok(student);
        }

        // POST: api/Student/department
        [HttpPost("department")]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            if (department == null || string.IsNullOrEmpty(department.DepartmentName))
            {
                return BadRequest("Invalid department data.");
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartments), new { id = department.DepartmentID }, department);
        }

        [HttpPost("course")]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (course == null)
            {
                return BadRequest("Course data is null.");
            }

            // Validate the DepartmentID
            var departmentExists = await _context.Departments.AnyAsync(d => d.DepartmentID == course.DepartmentID);
            if (!departmentExists)
            {
                return BadRequest("Invalid DepartmentID.");
            }

            _context.Courses.Add(course);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error while saving course.");
            }

            return CreatedAtAction(nameof(GetCoursesByDepartment), new { id = course.CourseID }, course);
        }

        // POST: api/Student/yearofjoining
        [HttpPost("yearofjoining")]
        public async Task<ActionResult<YearOfJoining>> PostYearOfJoining(YearOfJoining yearOfJoining)
        {
            if (yearOfJoining == null || yearOfJoining.Year <= 0)
            {
                return BadRequest("Invalid YearOfJoining data.");
            }

            _context.YearsOfJoining.Add(yearOfJoining);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error while saving YearOfJoining.");
            }

            return CreatedAtAction(nameof(GetYearsOfJoining), new { id = yearOfJoining.YearOfJoiningID }, yearOfJoining);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}
