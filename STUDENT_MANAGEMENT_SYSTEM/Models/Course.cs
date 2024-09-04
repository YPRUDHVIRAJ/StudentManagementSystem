using System.Collections.Generic;

namespace StudentManagementSystem.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int DepartmentID { get; set; }
        public Department Department { get; set; } = new Department();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
