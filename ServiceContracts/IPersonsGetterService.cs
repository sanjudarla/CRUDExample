using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsGetterService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<PersonResponse>> GetAllPersons();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Returns all objects that matches given search field and search string
        /// </summary>
        /// <param name="SearchBy"></param>
        /// <param name="searchString"></param>
        /// <returns>Returns all matching persons based on the search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string SearchBy, string? searchString);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<MemoryStream> GetPersonsCSV();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<MemoryStream> GetPersonsExcel();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //Task<MemoryStream> GetCountriesCSV();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // Task<MemoryStream> GetCountriesExcel();



    }
}
