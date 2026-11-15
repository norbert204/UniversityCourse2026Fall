using System.Text.Json;

namespace Frontend.Services;

public sealed class RequestState
{
    public bool IsBusy { get; private set; }
    public string? Error { get; private set; }

    public void ClearError() => Error = null;

    public async Task RunAsync(Func<Task> action, string errorMessage)
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        Error = null;
        try
        {
            await action();
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or InvalidDataException)
        {
            Error = $"{errorMessage} {exception.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
