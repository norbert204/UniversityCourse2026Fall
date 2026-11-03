using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts
{
    public class AssignStudentToLessonRequest
    {
        [Required]
        [MinLength(1)]
        public List<Guid> NeptunCodes { get; init; }
    }
}