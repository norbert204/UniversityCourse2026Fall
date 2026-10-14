namespace StudentApi.Models
{
    public class Student
    {
        public string Name { get; set; }

        public Guid NeptunCode { get; set; }

        public DateOnly BirthDate { get; set; }

        public string EmailAddress { get; set; }
    }
}
