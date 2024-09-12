using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(obj.GetType() != typeof(CountryResponse)) return false;
            CountryResponse response = (CountryResponse)obj;
            return this.CountryID == response.CountryID && this.CountryName == response.CountryName;
        }

        public override int GetHashCode()
        {
           return base.GetHashCode();
        }
    }
    public static class CountryExtension
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse() { CountryID = country.CountryID, CountryName = country.CountryName };
        }
    }
}
