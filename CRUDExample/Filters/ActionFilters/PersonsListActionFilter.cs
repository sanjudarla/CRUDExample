using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName}",nameof(PersonsListActionFilter),nameof(ActionExecutedContext));
            PersonsController personsController = (PersonsController)context.Controller;


            IDictionary<string, object?>? perameters = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];
            if (perameters != null)
            {
                if (perameters.ContainsKey("searchBy"))
                {
                    personsController.ViewData["CurrentSearchBy"] =
                        Convert.ToString(perameters["searchBy"]);
                }
                if (perameters.ContainsKey("searchString"))
                {
                    personsController.ViewData["CurrentSearchString"] =
                        Convert.ToString(perameters["searchString"]);
                }
                if (perameters.ContainsKey("sortBy"))
                {
                    personsController.ViewData["CurrentSortBy"] =
                        Convert.ToString(perameters["sortBy"]);
                }
                else
                {
                    personsController.ViewData["CurrentSortBy"] = nameof(PersonResponse.PersonName);
                }
                if (perameters.ContainsKey("sortOrder"))
                {
                    personsController.ViewData["CurrentSortOrder"] =
                        Convert.ToString(perameters["sortOrder"]);
                }
                else
                {
                    personsController.ViewData["CurrentSortOrder"] = nameof(SortOrderOptions.ASC);

                }
            }
            personsController.ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {  nameof(PersonResponse.PersonName),"Person Name" },
                {  nameof(PersonResponse.Email),"Email" },
                {  nameof(PersonResponse.DateOfBirth),"Date of Birth" },
                {  nameof(PersonResponse.Gender),"Gender" },
                {  nameof(PersonResponse.CountryID),"Country" },
                {  nameof(PersonResponse.Address),"Address" },
            };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;
            _logger.LogInformation("{FilterName}.{MethodName}",nameof(PersonsListActionFilter),nameof(OnActionExecuting));
            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchByOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address)

                    };
                    //resetting searchBy perameter value

                    if (searchByOptions.Any(temp => temp == searchBy) == false)
                    {
                        _logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                        _logger.LogInformation("searchBy updated value {searchBy}", context.ActionArguments["searchBy"]);

                    }
                }
            }

        }
    }
}
