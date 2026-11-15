using System.Net.Http.Json;
using StudentApi.Contracts;

namespace StudentPortal.Services;

public sealed class RegistrarApiClient(HttpClient httpClient)
{
    public Uri BaseAddress => httpClient.BaseAddress!;

    public async Task<IReadOnlyList<StudentDto>> GetStudentsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<GetStudentsResponse>("Student");
        return response?.Students ?? [];
    }

    public async Task<IReadOnlyList<LessonDto>> GetLessonsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<GetLessonsResponse>("Lesson");
        return response?.Lessons ?? [];
    }

    public async Task<IReadOnlyList<StudentDto>> GetLessonStudentsAsync(Guid lessonId)
    {
        var response = await httpClient.GetFromJsonAsync<GetStudentsResponse>($"Lesson/{lessonId}/students");
        return response?.Students ?? [];
    }

    public async Task<Guid> AddStudentAsync(AddStudentRequest request)
    {
        using var response = await httpClient.PostAsJsonAsync("Student", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AddStudentResponse>();
        return result!.NeptunCode;
    }

    public async Task<Guid> AddLessonAsync(AddLessonRequest request)
    {
        using var response = await httpClient.PostAsJsonAsync("Lesson", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AddLessonResponse>();
        return result!.Id;
    }

    public async Task AssignStudentAsync(Guid lessonId, Guid neptunCode)
    {
        using var response = await httpClient.PostAsJsonAsync(
            $"Lesson/{lessonId}/students",
            new AssignStudentToLessonRequest { NeptunCodes = [neptunCode] });
        response.EnsureSuccessStatusCode();
    }
}