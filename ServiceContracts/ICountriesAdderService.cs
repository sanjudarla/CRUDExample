using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manupulating business entity.
    /// </summary>
    public interface ICountriesAdderService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns></returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest); 
        
        

    }
}
