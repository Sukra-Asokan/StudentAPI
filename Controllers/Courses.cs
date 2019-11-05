using System.ComponentModel.DataAnnotations;

namespace StudentAPI.Controllers
{
    public class Courses
    {
        [Required]
        public string CourseName { get; set; }
        public string Sub1 { get; set; }
        public string Sub2 { get; set; }
        public string Sub3 { get; set; }

    }
}
