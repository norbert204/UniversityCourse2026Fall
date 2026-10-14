namespace StudentApi.Contracts
{
    public class StudentDto
    {
        public string Name { get; init; }

        public Guid NeptunCode { get; init; }

        public DateOnly BirthDate { get; init; }

        public string EmailAddress { get; init; }
    }
}
