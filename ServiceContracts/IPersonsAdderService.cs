using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsAdderService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addPersonRequest"></param>
        /// <returns></returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? addPersonRequest);
     
    }
}
