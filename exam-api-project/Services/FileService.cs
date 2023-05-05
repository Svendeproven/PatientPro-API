using System.IO.Compression;
using exam_api_project.Services.Interfaces;

namespace exam_api_project.Services;

/// <summary>
///     Provides file handling services such as creating zip archives.
/// </summary>
public class FileService : IFileService
{

    /// <summary>
    ///     Creates a zip archive containing the files in the "downloads" folder and returns a MemoryStream.
    /// </summary>
    /// <returns>A MemoryStream containing the zip archive data.</returns>
    public MemoryStream DownloadZipFile()
    {
        // Create a new MemoryStream to store the zip data in memory
        var memoryStream = new MemoryStream();

        // Create a new ZipArchive with the MemoryStream, set to Create mode and keep the stream open
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            // Get the base directory of the application
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Combine the base directory with the 'downloads' folder
            var downloadsDirectory = Path.Combine(baseDirectory, "downloads");

            AddToZip(downloadsDirectory, "", zipArchive);
        }

        // Reset the position of the memory stream to the beginning
        memoryStream.Position = 0;

        // Return the memory stream as a File result with the MIME type 'application/zip' and a custom filename
        return memoryStream;
    }

    /// <summary>
    ///     Recursively adds the content of the specified directory to the provided ZipArchive.
    /// </summary>
    /// <param name="directory">The directory path to add to the zip archive.</param>
    /// <param name="directoryPathInZip">The path of the directory within the zip archive.</param>
    /// <param name="zipArchive">The ZipArchive instance to add the directory content to.</param>
    private void AddToZip(string directory, string directoryPathInZip, ZipArchive zipArchive)
    {
        // Add each file in the directory to the zip
        foreach (var file in Directory.GetFiles(directory))
            try
            {
                // Create a new entry in the zip archive using the file and its name
                zipArchive.CreateEntryFromFile(file, Path.Combine(directoryPathInZip, Path.GetFileName(file)));
            }
            catch (Exception e)
            {
                // Log the exception's stack trace and rethrow it
                Log.Error(e.StackTrace);
            }

        // Process subdirectories
        foreach (var subdirectory in Directory.GetDirectories(directory))
            // Recursively add the subdirectory and its content to the zip
            AddToZip(subdirectory, Path.Combine(directoryPathInZip, Path.GetFileName(subdirectory)), zipArchive);
    }
}