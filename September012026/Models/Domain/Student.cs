namespace September012026.Models.Domain
{
    public class Student
    {
        public int Id { get; set; }

        public string StudentNumber { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public DateOnly Birthday { get; set; }

        public string Birthplace { get; set; }
    }
}