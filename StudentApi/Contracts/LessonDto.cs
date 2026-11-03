namespace StudentApi.Contracts
{
    public class LessonDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Room { get; init; }

        public DateTime StartsAt { get; init; }

        public int DurationMinutes { get; init; }
    }
}