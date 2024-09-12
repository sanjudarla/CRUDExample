﻿using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesAdderService _countriesService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;


        public PersonCreateAndEditPostActionFilter(ICountriesAdderService countriesService, ILogger<PersonCreateAndEditPostActionFilter> logger)
        {
            _countriesService = countriesService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is PersonsController personsController)
            {


                //Before Logic
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();

                    personsController.ViewBag.Countries = countries.Select(temp =>
                    new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    var personRequest = context.ActionArguments["personRequest"];
                    context.Result =  personsController.View(personRequest);
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
            _logger.LogInformation("In after Logic of PersonsCreateAndEditPostActionFilter");
            
        }
    }
}
