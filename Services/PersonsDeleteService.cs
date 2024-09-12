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
    public class PersonsDeleteService : IPersonsDeleterService
    {
        private readonly IPersonsRepository _personsRepository
            ;
        private readonly ILogger<PersonsDeleteService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsDeleteService(IPersonsRepository perosnsRepository, ILogger<PersonsDeleteService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = perosnsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }


        public async Task<bool> DeletePerson(Guid? personID)
        {
            _logger.LogInformation("Currently we reched to Delete Persons Service");
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }
            Person? person = await _personsRepository.GetPersonByPersonID(personID.Value);
            if (person == null)
            {
                return false;
            }

            await _personsRepository.DeletePersonByPersonID(personID.Value);


            return true;
        }

    }
}

