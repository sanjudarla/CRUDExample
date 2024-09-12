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


namespace Services
{
    public class PersonsSorterService : IPersonsSorterService
    {
        private readonly IPersonsRepository _personsRepository
            ;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsSorterService(IPersonsRepository perosnsRepository, ILogger<PersonsSorterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = perosnsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }


        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allpersons, string sortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("GetSortedPersons of persons service.");

            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
                switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                => allpersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
               => allpersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC)
               => allpersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
               => allpersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                => allpersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
               => allpersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                 => allpersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
               => allpersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                 => allpersons.OrderBy(temp => temp.Gender).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
               => allpersons.OrderByDescending(temp => temp.Gender).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC)
                => allpersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC)
               => allpersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                => allpersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
               => allpersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.ASC)
                => allpersons.OrderBy(temp => temp.ReceiveNewsLetter).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.DESC)
               => allpersons.OrderByDescending(temp => temp.ReceiveNewsLetter).ToList(),

                _ => allpersons

            };
            return sortedPersons;
        }

        
    }
}

