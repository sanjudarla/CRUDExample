using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manupulating business entity.
    /// </summary>
    public interface ICountriesGetterService
    {
       
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<CountryResponse>> GetAllCountries();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);
        

    }
}
