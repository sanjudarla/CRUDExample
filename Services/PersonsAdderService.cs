using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsAdderService : IPersonsAdderService
    {
        private readonly IPersonsRepository _personsRepository
            ;
        private readonly ILogger<PersonsAdderService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsAdderService(IPersonsRepository perosnsRepository, ILogger<PersonsAdderService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = perosnsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }


        public async Task<PersonResponse> AddPerson(PersonAddRequest? addPersonRequest)
        {

            //Check if personAddrequest is not null
            if (addPersonRequest == null)
            {
                throw new ArgumentNullException(nameof(addPersonRequest));
            }


            //Model Validation
            ValidationHelper.ModelValidation(addPersonRequest);

            //Convert "addPersonRequest" type from PersonAddRequest to Person Type
            Person person = addPersonRequest.ToPerson();
            //Generate a new personId
            person.PersonID = Guid.NewGuid();

            await _personsRepository.AddPerson(person);
            //_db.Persons.Add(person);
            return person.ToPersonResponse();

        }



    }
}

