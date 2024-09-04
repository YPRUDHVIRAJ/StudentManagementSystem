namespace StudentManagementSystem.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int DepartmentID { get; set; }
        public int CourseID { get; set; }
        public int YearOfJoiningID { get; set; }

        public byte[] FingerprintData { get; set; } = new byte[0];
        // Navigation properties
        public Department Department { get; set; } = new Department();
        public Course Course { get; set; } = new Course();
        public YearOfJoining YearOfJoining { get; set; } = new YearOfJoining();
    }
}
