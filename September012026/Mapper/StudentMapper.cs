using September012026.Models.Domain;
using September012026.Models.DTO;

namespace September012026.Mapper
{
    public static class StudentMapper
    {
        public static AddStudentDto MapToAddStudentDTO(
            this Student student)
        {
            return new AddStudentDto
            {
                StudentNumber = student.StudentNumber,
                LastName = student.LastName,
                FirstName = student.FirstName,
                Gender = student.Gender,
                Address = student.Address,
                Birthday = student.Birthday,
                Birthplace = student.Birthplace
            };
        }

        public static Student MapToStudent(
            this AddStudentDto addStudentDto)
        {
            return new Student
            {
                StudentNumber = addStudentDto.StudentNumber,
                LastName = addStudentDto.LastName,
                FirstName = addStudentDto.FirstName,
                Gender = addStudentDto.Gender,
                Address = addStudentDto.Address,
                Birthday = addStudentDto.Birthday,
                Birthplace = addStudentDto.Birthplace
            };
        }
    }
}