using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultsFilters
{
    public class PersonsListResultsFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonsListResultsFilter> _logger;
        public PersonsListResultsFilter(ILogger<PersonsListResultsFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} - before",
                nameof(PersonsListResultsFilter),nameof(OnResultExecutionAsync));
            context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            await next();
            _logger.LogInformation("{FilterName}.{MethodName} - after",
                nameof(PersonsListResultsFilter), nameof(OnResultExecutionAsync));

            
        }
    }
}
