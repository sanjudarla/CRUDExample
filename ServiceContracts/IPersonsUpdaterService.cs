﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsUpdaterService
    {
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatePersonRequest"></param>
        /// <returns></returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? updatePersonRequest);
        

    }
}
