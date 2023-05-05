using exam_api_project.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace exam_api_project.Controllers;

/// <summary>
///     Controller for creating a zip file from a directory and returning it as a file for download
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    
    private readonly IFileService _fileService;

    /// <summary>
    /// Initializes a new instance of the FilesController class.
    /// </summary>
    /// <param name="fileService">An implementation of the IFileService interface.</param>
    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Creates a ZIP file from a directory and returns it for download.
    /// </summary>
    /// <returns>An IActionResult containing the generated ZIP file.</returns>
    [HttpGet]
    public Task<IActionResult> DownloadZipFile()
    {
        try
        {
            var memoryStream = _fileService.DownloadZipFile();
            return Task.FromResult<IActionResult>(File(memoryStream, "application/zip", "de-varme-hænder.zip"));
        }
        catch (Exception e)
        {
            // Log the exception's stack trace and return an error status code with a custom message
            Log.Error(e.StackTrace);
            return Task.FromResult<IActionResult>(StatusCode(500, "Something went wrong, could not create zip"));
        }
    }
}