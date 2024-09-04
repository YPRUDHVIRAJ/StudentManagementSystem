using System.Collections.Generic;

namespace StudentManagementSystem.Models
{
    public class YearOfJoining
    {
        public int YearOfJoiningID { get; set; }
        public int Year { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
