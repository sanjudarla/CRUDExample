using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;

namespace Services
{
    public class CountriesGetterService : ICountriesGetterService
    {
        private readonly ICountriesRepository _countriesRepositort;


        public CountriesGetterService(ICountriesRepository countriesRepository)
        {
            _countriesRepositort = countriesRepository;
        }
        public async Task<List<CountryResponse>> GetAllCountries()
        {
            List<Country> countries = await _countriesRepositort.GetAllCountries();

            return countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }
            Country? countryResponse = await _countriesRepositort.GetCountryByCountryId(countryID);


            if (countryResponse == null)
            {
                return null;
            }
            return countryResponse.ToCountryResponse();
        }
       
          
    }
}
