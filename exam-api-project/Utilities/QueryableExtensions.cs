using System.Linq.Expressions;

namespace exam_api_project.Utilities;

/// <summary>
/// Static class for filtering IQueryable objects
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// This method handles filtering of IQueryable objects by a single property from a Filter object
    /// </summary>
    /// <typeparam name="T">The type of the elements in the IQueryable.</typeparam>
    /// <param name="query">The IQueryable to filter.</param>
    /// <param name="filters">A list of <see cref="Filter"/> objects containing the properties and values to filter by.</param>
    /// <returns>An IQueryable of type T with the specified filters applied.</returns>
    public static IQueryable<T> FilterByProperties<T>(this IQueryable<T> query, List<Filter> filters)
    {
        // If there are no filters, return the original query
        if (filters == null)
        {
            return query;
        }

        // Loop through all filters and apply them to the query
        foreach (var filter in filters)
        {
            // Create the expression tree for the filter
            var parameter = Expression.Parameter(typeof(T), "x");
            // Get the property from the parameter
            var property = Expression.Property(parameter, filter.PropertyName);
            // Get the value from the filter
            var constant = Expression.Constant(filter.PropertyValue);
            // Get the type of the property
            var propertyType = property.Type;
            // If the property is an enum, parse the value to the correct enum type
            var value = propertyType.IsEnum
                ? Expression.Constant(Enum.Parse(propertyType, constant.Value.ToString()))
                : Expression.Constant(Convert.ChangeType(constant.Value, propertyType));
            // Create the expression tree for the filter
            var predicate = Expression.Equal(property, value);
            // Create the lambda expression
            var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
            // Apply the filter to the query
            query = query.Where(lambda);
        }

        return query;
    }
}