using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultsFilters
{
    public class PersonsAlwaysRunResultsFilter : IAlwaysRunResultFilter
    {
        private readonly ILogger<PersonsAlwaysRunResultsFilter> _logger;

        public PersonsAlwaysRunResultsFilter(ILogger<PersonsAlwaysRunResultsFilter> logger)
        {
            _logger = logger;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if(context.Filters.OfType<SkipFilter>().Any())
            {
                return;
            }
            
        }
    }
}
