using Microsoft.AspNetCore.Mvc;
using September012026.Models.Domain;
using September012026.Models.DTO;
using September012026.Mapper;
using System.Text.RegularExpressions;

namespace September012026.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        static List<Student> students = new List<Student>();

        // GET: api/Students
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(students);
        }

        // GET: api/Students/1
        [HttpGet("{id:int}")]
        public IActionResult GetStudent(
            [FromRoute] int id)
        {
            var student = students
                .Where(m => m.Id == id)
                .FirstOrDefault();

            if (student != null)
            {
                return Ok(student);
            }

            return NotFound();
        }

        // GET: api/Students/search?lastName=Garcia&firstName=Alex
        [HttpGet("search")]
        public IActionResult SearchStudent(
            [FromQuery] string? lastName,
            [FromQuery] string? firstName)
        {
            var studentsFound = students.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                studentsFound = studentsFound.Where(
                    m => m.LastName.Contains(
                        lastName,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                studentsFound = studentsFound.Where(
                    m => m.FirstName.Contains(
                        firstName,
                        StringComparison.OrdinalIgnoreCase));
            }

            return Ok(studentsFound);
        }

        // POST: api/Students
        [HttpPost]
        public IActionResult AddStudent(
            [FromBody] AddStudentDto student)
        {
            if (student == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(student.StudentNumber))
            {
                return BadRequest("Student Number is required.");
            }

            // Student Number format: 2026-00001
            if (!Regex.IsMatch(
                student.StudentNumber,
                @"^\d{4}-\d{5}$"))
            {
                return BadRequest(
                    "Student Number must follow the format YYYY-NNNNN.");
            }

            // Check Student Number uniqueness
            var existingStudent = students
                .FirstOrDefault(m =>
                    m.StudentNumber.Equals(
                        student.StudentNumber,
                        StringComparison.OrdinalIgnoreCase));

            if (existingStudent != null)
            {
                return Conflict(
                    "Student Number already exists.");
            }

            // Auto-generate sequential Id
            int newId = students.Count == 0
                ? 1
                : students.Max(m => m.Id) + 1;

            var newStudent = student.MapToStudent();

            newStudent.Id = newId;

            students.Add(newStudent);

            return CreatedAtAction(
                nameof(GetStudent),
                new { id = newStudent.Id },
                newStudent);
        }

        // PUT: api/Students/1
        [HttpPut("{id:int}")]
        public IActionResult EditStudent(
            [FromRoute] int id,
            [FromBody] EditStudentDto student)
        {
            if (student == null)
            {
                return BadRequest();
            }

            var studentForEdit = students
                .Where(m => m.Id == id)
                .FirstOrDefault();

            if (studentForEdit == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(student.StudentNumber))
            {
                return BadRequest(
                    "Student Number is required.");
            }

            // Check Student Number format
            if (!Regex.IsMatch(
                student.StudentNumber,
                @"^\d{4}-\d{5}$"))
            {
                return BadRequest(
                    "Student Number must follow the format YYYY-NNNNN.");
            }

            // Check Student Number uniqueness
            var duplicateStudent = students
                .FirstOrDefault(m =>
                    m.Id != id &&
                    m.StudentNumber.Equals(
                        student.StudentNumber,
                        StringComparison.OrdinalIgnoreCase));

            if (duplicateStudent != null)
            {
                return Conflict(
                    "Student Number already exists.");
            }

            studentForEdit.StudentNumber =
                student.StudentNumber;

            studentForEdit.LastName =
                student.LastName;

            studentForEdit.FirstName =
                student.FirstName;

            studentForEdit.Gender =
                student.Gender;

            studentForEdit.Address =
                student.Address;

            studentForEdit.Birthday =
                student.Birthday;

            studentForEdit.Birthplace =
                student.Birthplace;

            return Ok(studentForEdit);
        }

        // DELETE: api/Students/1
        [HttpDelete("{id:int}")]
        public IActionResult DeleteStudent(
            [FromRoute] int id)
        {
            var student = students
                .Where(m => m.Id == id)
                .FirstOrDefault();

            if (student == null)
            {
                return NotFound();
            }

            students.Remove(student);

            return NoContent();
        }
    }
}