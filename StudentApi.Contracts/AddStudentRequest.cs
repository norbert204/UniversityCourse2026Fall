using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts
{
    public class AddStudentRequest
    {
        [Required]
        public string Name { get; init; }

        public DateOnly BirthDate { get; init; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; init; }
    }
}