using System;
using System.ComponentModel.DataAnnotations;
namespace StudentAPI
{
    public class StudentDetails
    {
        public int StudentId { get; set; }
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(25)]
        public string LastName { get; set; }
        [Required]
        public string Dob { get; set; }
        [StringLength(2000)]
        public string Address { get; set; }
        public string Course { get; set; }
        
        public long PHnumber { get; set; }
        [Required]
         public string EnrolDate { get; set; }
    } 
}
