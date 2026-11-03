using StudentApi.Contracts;
using StudentApi.Models;

namespace StudentApi.Mappers
{
    public static class LessonMappers
    {
        public static Lesson ToLessonEntity(this AddLessonRequest request) =>
            new()
            {
                Name = request.Name,
                Room = request.Room,
                StartsAt = request.StartsAt.Value,
                DurationMinutes = request.DurationMinutes,
            };

        public static Lesson ToLessonEntity(this UpdateLessonRequest request) =>
            new()
            {
                Name = request.Name,
                Room = request.Room,
                StartsAt = request.StartsAt.Value,
                DurationMinutes = request.DurationMinutes,
            };

        public static LessonDto ToDto(this Lesson entity) =>
            new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Room = entity.Room,
                StartsAt = entity.StartsAt,
                DurationMinutes = entity.DurationMinutes,
            };

        public static List<LessonDto> ToDto(this List<Lesson> entities) =>
            entities.Select(lesson => lesson.ToDto()).ToList();
    }
}