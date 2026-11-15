namespace StudentApi.Contracts
{
    public class AssignStudentToLessonResponse
    {
        public Guid LessonId { get; init; }

        public List<Guid> NeptunCodes { get; init; }
    }
}