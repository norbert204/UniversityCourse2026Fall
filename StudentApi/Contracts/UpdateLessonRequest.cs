using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts
{
    public class UpdateLessonRequest
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Room { get; init; }

        [Required]
        public DateTime? StartsAt { get; init; }

        [Range(1, 1440)]
        public int DurationMinutes { get; init; }
    }
}