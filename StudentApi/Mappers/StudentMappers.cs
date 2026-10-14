using StudentApi.Contracts;
using StudentApi.Models;

namespace StudentApi.Mappers
{
    public static class StudentMappers
    {
        public static Student ToStudentEntity(this UpdateStudentRequest dto) =>
            new()
            {
                Name = dto.Name,
                EmailAddress = dto.EmailAddress,
                BirthDate = dto.BirthDate,
            };

        public static Student ToStudentEntity(this AddStudentRequest dto) =>
            new()
            {
                Name = dto.Name,
                EmailAddress = dto.EmailAddress,
                BirthDate = dto.BirthDate,
            };

        public static StudentDto ToDto(this Student entity) =>
            new()
            {
                NeptunCode = entity.NeptunCode,
                Name = entity.Name,
                EmailAddress = entity.EmailAddress,
                BirthDate = entity.BirthDate,
            };

        public static List<StudentDto> ToDto(this List<Student> entity) =>
            entity
                .Select(x => x.ToDto())
                .ToList();
    }
}
