namespace StudentApi.Contracts
{
    public class AddStudentRequest
    {
        public string Name { get; init; }

        public DateOnly BirthDate { get; init; }

        public string EmailAddress { get; init; }
    }
}
