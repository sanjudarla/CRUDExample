﻿using CsvHelper;
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
    public class PersonsGetterService : IPersonsGetterService
    {
        private readonly IPersonsRepository _personsRepository
            ;
        private readonly ILogger<PersonsGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsGetterService(IPersonsRepository perosnsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = perosnsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }


        public virtual async Task<List<PersonResponse>> GetAllPersons()
        {
            _logger.LogInformation("GetAllPersons of persons service.");


            //Using the Stored Procedures
            //return _db.sp_GetAllPersons().Select(temp => ConvertPersonToPersonResponse(temp)).ToList();
            var person = await _personsRepository.GetAllPersons();

            return person.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public virtual async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
            {
                return null;
            }
            Person? personDetails = await _personsRepository.GetPersonByPersonID(personID.Value);
            if (personDetails == null)
            {
                return null;
            }
            return personDetails.ToPersonResponse();

        }

        public virtual async Task<List<PersonResponse>> GetFilteredPersons(string SearchBy, string? searchString)
        {
            _logger.LogInformation("GetFilteredPersons of persons service.");
            List<Person> persons = null;

            using (Operation.Time("Time for filtered persons for database"))
            {


                persons = SearchBy switch

                {
                    nameof(PersonResponse.PersonName) =>

                          await _personsRepository.GetFilteredPersons(temp =>
                              temp.PersonName.Contains(searchString)),


                    nameof(PersonResponse.Email) =>

                       await _personsRepository.GetFilteredPersons(temp =>
                        temp.Email.Contains(searchString)),


                    nameof(PersonResponse.DateOfBirth) =>

                        await _personsRepository.GetFilteredPersons(temp =>
                       temp.DateOfBirth.Value.ToString("dd MMMM yyyy")
                       .Contains(searchString)),

                    nameof(PersonResponse.Gender) =>

                       await _personsRepository.GetFilteredPersons(temp =>
                       temp.Gender.Contains(searchString)),


                    nameof(PersonResponse.CountryID) =>

                        await _personsRepository.GetFilteredPersons(temp =>
                       temp.Country.CountryName.Contains(searchString)),


                    nameof(PersonResponse.Address) =>

                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.Address.Contains(searchString)),


                    _ => await _personsRepository.GetAllPersons()

                };
            }
            _diagnosticContext.Set("Persons", persons);
            return persons.Select(temp => temp.ToPersonResponse()).ToList();

        }


        public virtual async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamwriter = new StreamWriter(memoryStream);

            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            CsvWriter csvWriter = new CsvWriter(streamwriter, csvConfiguration);

            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            csvWriter.WriteField(nameof(PersonResponse.Gender));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetter));

            csvWriter.NextRecord();
            List<PersonResponse> persons = await GetAllPersons();

            foreach (PersonResponse person in persons)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Email);
                if (person.DateOfBirth.HasValue)
                    csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MMM-dd"));
                csvWriter.WriteField(person.Age);
                csvWriter.WriteField(person.Gender);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.ReceiveNewsLetter);
                csvWriter.NextRecord();
                csvWriter.Flush();

            }


            memoryStream.Position = 0;
            return memoryStream;

        }
        public virtual async  Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Date Of Birth";
                worksheet.Cells["D1"].Value = "Age";
                worksheet.Cells["E1"].Value = "Gender";
                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "Address";
                worksheet.Cells["H1"].Value = "Receive News Letter";

                using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    headerCells.Style.Font.Bold = true;
                };

                int row = 2;
                List<PersonResponse> persons = await GetAllPersons();
                foreach (PersonResponse person in persons)
                {
                    worksheet.Cells[row, 1].Value = person.PersonName;
                    worksheet.Cells[row, 2].Value = person.Email;
                    if (person.DateOfBirth.HasValue)
                    {
                        worksheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MMM-dd");

                    }
                    worksheet.Cells[row, 4].Value = person.Age;
                    worksheet.Cells[row, 5].Value = person.Gender;
                    worksheet.Cells[row, 6].Value = person.Country;
                    worksheet.Cells[row, 7].Value = person.Address;
                    worksheet.Cells[row, 8].Value = person.ReceiveNewsLetter;
                    row++;
                }
                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();
                await excelPackage.SaveAsync();
            }
            memoryStream.Position = 0;
            return memoryStream;


        }


        
    }
}

