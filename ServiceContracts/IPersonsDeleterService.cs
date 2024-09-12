﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        Task<bool> DeletePerson(Guid? personID);


    }
}
