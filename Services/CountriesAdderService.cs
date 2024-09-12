using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;

namespace Services
{
    public class CountriesAdderService : ICountriesAdderService
    {
        private readonly ICountriesRepository _countriesRepositort;


        public CountriesAdderService(ICountriesRepository countriesRepository)
        {
            _countriesRepositort = countriesRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation:countryAddRequest Cannot Be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            //Validation:If country Name is null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            //Validation:Duplicate country names are not allowed
            if (await _countriesRepositort.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Given Country Name Already Exists");
            }
            //Convert object from countryAddrequest to countryType
            Country country = countryAddRequest.ToCountry();
            //generate Country ID
            country.CountryID = Guid.NewGuid();
            //Add country object into countries
            await _countriesRepositort.AddCountry(country);
            return country.ToCountryResponse();


        }

        

        

    }
}
