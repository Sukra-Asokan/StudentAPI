using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StudentAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private static List<StudentDetails> stud = new List<StudentDetails>();
        private static List<Courses> Cour = new List<Courses>();    

        // GET api/student
        [HttpGet("api/stud")]

        public IActionResult GetStudentDetails()
        {
            return Ok(stud);
        }
        // GET api/studentList

        [HttpGet("api/stud/list")]

        public IActionResult GetStudentList()
        {
            var student = from s in stud
                          select new { s.FirstName, s.LastName };
            return Ok(student);
        }
        // GET api/course

        [HttpGet("api/course")]

        public IActionResult GetCourseDetails()
        {
            return Ok(Cour);
        }

        //GET api/course_details
        [HttpGet("api/course/details")]
        public IActionResult GetCourseListDetails()
        {
            var courselist = from C in Cour
                             join S in stud on C.CourseName equals S.Course into cs
                             from docIfNull in cs.DefaultIfEmpty()
                             group docIfNull by C.CourseName into g
                             select new {CourseName=g.Key, Count= g.Count(x => x != null)};
            return Ok(courselist);
        }


        // GET api/student/id
        [HttpGet("api/stud/{id}")]
        public IActionResult GetStudentDetailsById(int id)
        {
            foreach (var entity in stud)
            {
                if (entity.StudentId==id)
                {
                    return Ok(entity);
                }
                
            }
            return NotFound();
        }

        // GET api/course/id
        [HttpGet("api/course/{C_name}")]
        public IActionResult GetCourseDetailsById(string C_name)
        {
            foreach (var entity in Cour)
            {
                if (entity.CourseName == C_name)
                {
                    return Ok(entity);
                }

            }
            return NotFound();
        }



        //POST api/student
        [HttpPost("api/stud")]
        public IActionResult CreateStudentDetails (StudentDetails student)
        {
            int id;
            bool flag = false;
            DateTime dob, Erol;

            if (DateTime.TryParseExact(student.Dob, new[] { "dd-MMM-yyyy" }, null, DateTimeStyles.None, out dob))
            {
                String.Format("{0:dd-mmm-yyyy}", dob);
                if (dob > DateTime.Now)
                {
                    return Conflict("enter a valid date");
                }
            }
            else
                return Conflict();
            if (DateTime.TryParseExact(student.EnrolDate, new[] { "dd-MMM-yyyy" }, null, DateTimeStyles.None, out Erol))
            {
                String.Format("{0:dd-mmm-yyyy}", Erol);
                if (Erol > DateTime.Now)
                {
                    return Conflict("enter a valid date");
                }
            }
            else
                return Conflict();
            
            var laststud = stud.OrderByDescending(x => x.StudentId).FirstOrDefault();
         
            if (laststud == null)
                id = 1;
            else id = laststud.StudentId + 1;
            foreach (var course in Cour)
            {
                if (student.Course ==course.CourseName)
                {
                    flag = true;
                } 
            }
            if(flag==false)
                {
                    return Conflict("Course is Not is list");
                }
            
            else
                {
                    var studToBeAdded = new StudentDetails
                {

                    StudentId = id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Dob = student.Dob,
                    Address = student.Address,
                    Course = student.Course,
                    PHnumber = student.PHnumber,
                    EnrolDate = student.EnrolDate
                };
                stud.Add(studToBeAdded);
                return Ok(studToBeAdded.StudentId);
            }
        }

        // POST api/course
        [HttpPost("api/course")]
        public IActionResult CreateCourseDetails(Courses course)
        {
            var courseToBeAdded = new Courses
            {
                
                CourseName = course.CourseName,
                Sub1 = course.Sub1,
                Sub2 = course.Sub2,
                Sub3 = course.Sub3
            };
            Cour.Add(courseToBeAdded);
            return Ok(courseToBeAdded.CourseName);

        }


        // PUT api/student
        [HttpPut("api/stud/{id}")]
        public IActionResult EdidtStudentDetails(int id,StudentDetails student)
        {
          
                bool flag = false;
            DateTime dob, Erol;

            if (DateTime.TryParseExact(student.Dob, new[] { "dd-MMM-yyyy" }, null, DateTimeStyles.None, out dob))
            {
                String.Format("{0:dd-mmm-yyyy}", dob);
                if (dob > DateTime.Now)
                {
                    return Conflict("enter a valid date");
                }
            }
            else
                return Conflict();
            if (DateTime.TryParseExact(student.EnrolDate, new[] { "dd-MMM-yyyy" }, null, DateTimeStyles.None, out Erol))
            {
                String.Format("{0:dd-mmm-yyyy}", Erol);
                if (Erol > DateTime.Now)
                {
                    return Conflict("enter a valid date");
                }
            }
            else
                return Conflict();
            foreach (var course in Cour)
                {
                    if (student.Course == course.CourseName)
                    {
                        flag = true;
                    }
                }
                if (flag == false)
                {
                    return Conflict("Course is Not is list");
                }
            foreach (var entity in stud)
            {
                if (Convert.ToDateTime(student.Dob) > DateTime.Now)
                {
                    return Conflict("enter a valid date");
                }
                if (Convert.ToDateTime(student.EnrolDate) > DateTime.Now)
                {
                    return Conflict("enter a valid date");
                }
                else if (entity.StudentId == id)
                {

                    entity.FirstName = student.FirstName;
                    entity.LastName = student.LastName;
                    entity.Dob = student.Dob;
                    entity.Address = student.Address;
                    entity.Course = student.Course;
                    entity.PHnumber = student.PHnumber;
                    entity.EnrolDate = student.EnrolDate;
                    return Ok(entity.StudentId);
 
                }
            }
            return NotFound();
            
        }
        // PUT api/course
        [HttpPut("api/course")]
        public IActionResult EditCourseDetails(Courses course)
        {
            foreach (var entity in Cour)
            {
                if (entity.CourseName == course.CourseName)
                {
                    entity.CourseName = course.CourseName;
                    entity.Sub1 = course.Sub1;
                    entity.Sub2 = course.Sub2;
                    entity.Sub3 = course.Sub3;
                    return Ok(entity.CourseName);
                }
            }
            return NotFound();  
        }

        // DELETE api/student/id
        [HttpDelete("api/stud/{id}")]   
        public IActionResult DelStudentDetails(int id)
        {
            foreach (var entity in stud)
            {
                if (entity.StudentId == id)
                {
                    stud.Remove(entity);
                    return Ok();
                }
            }
                return NotFound();

        }

        // DELETE api/course/id
        [HttpDelete("api/course/{id}")]
        public IActionResult DeleteCourseDetails(string id)
        {
            foreach (var entity in Cour)
            {
                if (entity.CourseName == id)
                {
                    Cour.Remove(entity);
                    return Ok();
                }
            }
            return NotFound();

        }
    }
}

