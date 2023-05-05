namespace exam_api_project.models.Exceptions;

/// <summary>
/// HttpError class for handling http errors
/// </summary>
public class HttpError : Exception
{
    public string Title { get; set; }
    public string Text { get; set; }
    public int Status { get; set; }
}