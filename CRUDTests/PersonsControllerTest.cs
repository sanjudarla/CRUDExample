using AutoFixture;
using Microsoft.Data.SqlClient;
using CRUDExample.Controllers;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly ICountriesAdderService _countriesAdderService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ILogger<PersonsController> _logger;

        private readonly Mock<IPersonsDeleterService> _personsDeleterServiceMock;
        private readonly Mock<IPersonsAdderService> _personsAdderServiceMock;
        private readonly Mock<IPersonsGetterService> _personsGetterServiceMock;
        private readonly Mock<IPersonsUpdaterService> _personsUpdaterServiceMock;
        private readonly Mock<IPersonsSorterService> _personsSorterServiceMock;
        private readonly Mock<ICountriesAdderService> _countriesAdderServiceMock;
        private readonly Mock<ICountriesGetterService> _countriesGetterServiceMock;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;

        private readonly Fixture _fixture;


        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _personsDeleterServiceMock = new Mock<IPersonsDeleterService>();
            _personsAdderServiceMock = new Mock<IPersonsAdderService>();
            _personsGetterServiceMock = new Mock<IPersonsGetterService>();
            _personsSorterServiceMock = new Mock<IPersonsSorterService>();
            _personsUpdaterServiceMock = new Mock<IPersonsUpdaterService>();
            _countriesAdderServiceMock = new Mock<ICountriesAdderService>();
            _countriesGetterServiceMock = new Mock<ICountriesGetterService>();
            _loggerMock = new Mock<ILogger<PersonsController>>();

            _personsAdderService = _personsAdderServiceMock.Object;
            _personsGetterService = _personsGetterServiceMock.Object;
            _personsUpdaterService = _personsUpdaterServiceMock.Object;
            _personsSorterService = _personsSorterServiceMock.Object;
            _personsDeleterService = _personsDeleterServiceMock.Object;
            _countriesAdderService = _countriesAdderServiceMock.Object;
            _countriesGetterService = _countriesGetterServiceMock.Object;
            _logger = _loggerMock.Object;


        }
        #region index
        [Fact]
        public async Task Index_ShouldReturnIndexWithPersonsList()
        {
            //Arrange
            List<PersonResponse> personResponses = _fixture.Create<List<PersonResponse>>();

            PersonsController personsController = new PersonsController(_personsDeleterService,
                _personsAdderService, 
                _personsSorterService,
               _personsUpdaterService, 
               _personsGetterService, 
               _countriesAdderService,
               _countriesGetterService,
               _logger);

            _personsGetterServiceMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(personResponses);

            _personsSorterServiceMock.Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
               .ReturnsAsync(personResponses);

            //Act
            IActionResult result = await personsController.Index(_fixture.Create<string>()
                 , _fixture.Create<string>(), _fixture.Create<string>()
                 , _fixture.Create<SortOrderOptions>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(personResponses);
        }
        #endregion
        #region Create
        [Fact]
        public async void Create_IfNoModelErrors_ToReturnRedirectToIndex()
        {
            //Arrange
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

            PersonResponse person_response = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countriesGetterServiceMock
             .Setup(temp => temp.GetAllCountries())
             .ReturnsAsync(countries);

            _personsAdderServiceMock
             .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
             .ReturnsAsync(person_response);

            PersonsController personsController = new PersonsController(_personsDeleterService,
                 _personsAdderService,
                 _personsSorterService,
                _personsUpdaterService,
                _personsGetterService,
                _countriesAdderService,
                _countriesGetterService,
                _logger);


            //Act
            IActionResult result = await personsController.Create(person_add_request);

            //Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ActionName.Should().Be("Index");
        }

        #endregion

    }
}
