using System.Web;

namespace exam_api_project.Utilities;

/// <summary>
///     A utility class for parsing query string parameters into a list of filters.
/// </summary>
public static class QueryStringParser
{
    /// <summary>
    ///     Parses a query string into a list of filters.
    /// </summary>
    /// <param name="query">The query string to parse.</param>
    /// <returns>A list of <see cref="Filter"/> objects representing the parsed query parameters.</returns>
    public static List<Filter> Parse(string query)
    {
        // Create a list of filters
        var filters = new List<Filter>();

        // If the query string is empty, return the empty list
        if (string.IsNullOrEmpty(query)) return filters;
        // Remove the '?' from the query string
        var queryParams = HttpUtility.ParseQueryString(query);
        // Loop through all query parameters and add them to the list of filters
        foreach (var key in queryParams.AllKeys)
        {
            // Add the filter to the list
            filters.Add(new Filter
            {
                PropertyName = key,
                PropertyValue = queryParams[key]
            });
        }

        return filters;
    }
}