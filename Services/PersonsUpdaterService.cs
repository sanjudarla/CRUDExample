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
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        private readonly IPersonsRepository _personsRepository
            ;
        private readonly ILogger<PersonsUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsUpdaterService(IPersonsRepository perosnsRepository, ILogger<PersonsUpdaterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = perosnsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }


        
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? updatePersonRequest)
        {
            if (updatePersonRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }
            ValidationHelper.ModelValidation(updatePersonRequest);
            Person? mathingPerson = await _personsRepository.GetPersonByPersonID(updatePersonRequest.PersonID);
            if (mathingPerson == null)
            {
                throw new InvalidPersonIdException("given Person id does not exist");

            }
            mathingPerson.PersonName = updatePersonRequest.PersonName;
            mathingPerson.Email = updatePersonRequest.Email;
            mathingPerson.DateOfBirth = updatePersonRequest.DateOfBirth;
            mathingPerson.Gender = updatePersonRequest.Gender.ToString();
            mathingPerson.CountryID = updatePersonRequest.CountryID;
            mathingPerson.Address = updatePersonRequest.Address;
            mathingPerson.ReceiveNewsLetter = updatePersonRequest.ReceiveNewsLetter;
            await _personsRepository.UpdatePerson(mathingPerson);

            return mathingPerson.ToPersonResponse();
        }
       
    }
}

