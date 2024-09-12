using CRUDExample.Filters;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilter;
using CRUDExample.Filters.ExceptionFilter;
using CRUDExample.Filters.ResourceFilters;
using CRUDExample.Filters.ResultsFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDExample.Controllers
{
    //[TypeFilter(typeof(ResponseHeaderFilterFactoryAttribute),
    //    Arguments = new object[] { "my-key-from-controller", "my-value-from-controller", 3 }, Order = 3)]
    [Route("[controller]")]
    [ResponseHeaderFilterFactory("my-key", "my-value", 4)]
    //[TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonsAlwaysRunResultsFilter))]

    public class PersonsController : Controller
    {
        private readonly IPersonsDeleterService _personsDeleteService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly ICountriesAdderService _countriesAdderService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController(IPersonsDeleterService personsDeleteService,
             IPersonsAdderService personsAdderService,
            IPersonsSorterService personsSorterService,
            IPersonsUpdaterService personsUpdaterService,
            IPersonsGetterService personsGetterService,
            ICountriesAdderService countriesAdderService,
            ICountriesGetterService countriesGetterService,
            ILogger<PersonsController> logger)

        {
            _personsAdderService = personsAdderService;
            _personsSorterService = personsSorterService;
            _personsDeleteService = personsDeleteService;
            _personsGetterService = personsGetterService;
            _personsUpdaterService = personsUpdaterService;
            _countriesAdderService = countriesAdderService;
            _countriesGetterService = countriesGetterService;
            _logger = logger;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchString"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>

        [Route("[action]")]
        [Route("/")]
        [ServiceFilter(typeof(PersonsListActionFilter), Order = 4)]
        [ResponseHeaderFilterFactory("my-Key-from-action", "my-Value-from-action", 1)]
        [TypeFilter(typeof(PersonsListResultsFilter))]
        [SkipFilter]
        
        public async Task<IActionResult> Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index Action of the person controller");
            _logger.LogDebug($"Search By {searchBy},SearchString {searchString},sortBy {sortBy},sortOrder {sortOrder}");

            List<PersonResponse> person = await _personsGetterService.GetFilteredPersons(searchBy, searchString);


            List<PersonResponse> sortedPersons = await _personsSorterService.GetSortedPersons(person, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(sortedPersons);
        }




        [HttpGet]
        [Route("[action]")]
        [ResponseHeaderFilterFactory("my-key", "my-value", 4)]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countryResponse = await _countriesGetterService.GetAllCountries();
            ViewBag.Countries = countryResponse.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });
            return View();
        }



        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(FeatureDisabledResourceFilter), Arguments = new object[] { false })]

        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            PersonResponse person = await _personsAdderService.AddPerson(personRequest);
            return RedirectToAction("Index", "Persons");
        }




        [Route("[action]/{personId}")]
        [TypeFilter(typeof(TokenResultFilter))]

        [HttpGet]
        public async Task<IActionResult> Edit(Guid personId)
        {
            PersonResponse? response = await _personsGetterService.GetPersonByPersonID(personId);
            if (response == null)
            {
                return RedirectToAction("index");
            }
            PersonUpdateRequest updateRequestObject = response.ToPersonUpdateRequest();

            List<CountryResponse> countryResponse = await _countriesGetterService.GetAllCountries();
            ViewBag.Countries = countryResponse.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });
            return View(updateRequestObject);
        }




        [Route("[action]/{personId}")]
        [HttpPost]
        [TypeFilter(typeof(PersonsAlwaysRunResultsFilter))]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            PersonResponse? response = await _personsGetterService.GetPersonByPersonID(personRequest.PersonID);
            if (response == null)
            {
                return RedirectToAction("index");
            }
            PersonResponse updateResponse = await _personsUpdaterService.UpdatePerson(personRequest);
            return RedirectToAction("index");
        }





        [Route("[action]/{personId}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid personId)
        {
            PersonResponse? response = await _personsGetterService.GetPersonByPersonID(personId);
            if (response == null)
            {
                return RedirectToAction("index");
            }

            return View(response);
        }





        [Route("[action]/{personId}")]
        [HttpPost]
        public async Task<IActionResult> Delete(PersonUpdateRequest persondeleteRequest)
        {
            PersonResponse? response = await _personsGetterService.GetPersonByPersonID(persondeleteRequest.PersonID);
            if (response == null)
            {
                return RedirectToAction("index");
            }
            await _personsDeleteService.DeletePerson(response.PersonID);

            return RedirectToAction("index");
        }





        [Route("PersonsPDF")]
        public async Task<IActionResult> PersonsPDF()
        {
            //et list of persons
            List<PersonResponse> persons = await _personsGetterService.GetAllPersons();
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }





        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personsGetterService.GetPersonsCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }






        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personsGetterService.GetPersonsExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }

        
    }

}