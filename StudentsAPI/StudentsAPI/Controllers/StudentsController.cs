using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private static List<Student> _students = new List<Student>()
        {
            new Student()
            {
                Id = 1,
                Name="Vusal"
            },
            new Student()
            {
                Id=2,
                Name="Vusale"
            },
            new Student()
            {
                Id=3,
                Name="Gunay"
            },
            new Student()
            {
                Id=4,
                Name="Aydan"
            }
        };

        [HttpGet("")]
        public IActionResult GetAllStudent()
        {
            return Ok(_students);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetByIdStudent(int id)
        {
            var student = _students.FirstOrDefault(x => x.Id == id);
            return Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            _students.Add(student);
            return Ok(_students);
        }

        [HttpPut]
        public IActionResult UpdateStudent(Student student)
        {
            var existStudent=_students.FirstOrDefault(x=>x.Id==student.Id);  
            existStudent.Name= student.Name;
            return Ok(existStudent);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student=_students.FirstOrDefault(x=>x.Id==id);
            _students.Remove(student);
            return Ok(_students);
        }
    }
}
