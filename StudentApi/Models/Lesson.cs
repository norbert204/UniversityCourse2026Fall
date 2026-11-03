namespace StudentApi.Models
{
    public class Lesson
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Room { get; set; }

        public DateTime StartsAt { get; set; }

        public int DurationMinutes { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}