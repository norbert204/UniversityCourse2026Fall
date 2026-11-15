using System.Net.Http.Json;
using StudentApi.Contracts;

namespace Frontend.Services;

public sealed class RegistrationApiClient(HttpClient httpClient)
{
    public Uri BaseAddress => httpClient.BaseAddress!;

    public async Task<IReadOnlyList<StudentDto>> GetAllStudentsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<GetStudentsResponse>("Student");
        return response?.Students ?? throw new InvalidDataException("The API returned no students list.");
    }

    public async Task<IReadOnlyList<LessonDto>> GetLessonsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<GetLessonsResponse>("Lesson");
        return response?.Lessons ?? throw new InvalidDataException("The API returned no lessons list.");
    }

    public async Task<IReadOnlyList<StudentDto>> GetLessonStudentsAsync(Guid lessonId)
    {
        var response = await httpClient.GetFromJsonAsync<GetStudentsResponse>($"Lesson/{lessonId}/students");
        return response?.Students ?? throw new InvalidDataException("The API returned no lesson students list.");
    }

    public async Task<Guid> AddStudentAsync(AddStudentRequest request)
    {
        using var response = await httpClient.PostAsJsonAsync("Student", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AddStudentResponse>();
        return (result ?? throw new InvalidDataException("The API returned no student response.")).NeptunCode;
    }

    public async Task<Guid> AddLessonAsync(AddLessonRequest request)
    {
        using var response = await httpClient.PostAsJsonAsync("Lesson", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AddLessonResponse>();
        return (result ?? throw new InvalidDataException("The API returned no lesson response.")).Id;
    }

    public async Task DeleteStudentAsync(Guid id)
    {
        using var response = await httpClient.DeleteAsync($"Student/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateStudentAsync(Guid id, UpdateStudentRequest request)
    {
        using var response = await httpClient.PutAsJsonAsync($"Student/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task AssignStudentAsync(Guid lessonId, Guid neptunCode)
    {
        using var response = await httpClient.PostAsJsonAsync(
            $"Lesson/{lessonId}/students", new AssignStudentToLessonRequest { NeptunCodes = [neptunCode] });
        response.EnsureSuccessStatusCode();
    }
}