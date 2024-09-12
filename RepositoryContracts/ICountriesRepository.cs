
using Entities;

namespace RepositoryContracts
{
    public interface ICountriesRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        Task<Country> AddCountry(Country country);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<Country>> GetAllCountries();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        Task<Country?> GetCountryByCountryId(Guid? countryID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        Task<Country?> GetCountryByCountryName(string countryName);

    }
}
