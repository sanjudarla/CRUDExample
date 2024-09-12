using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;
            PersonResponse? response = (PersonResponse)obj;
            return PersonID == response.PersonID
                && PersonName == response.PersonName
                && Email == response.Email
                && DateOfBirth == response.DateOfBirth
                && Gender == response.Gender
                && CountryID == response.CountryID
                && Country == response.Country
                && Address == response.Address
                && ReceiveNewsLetter == response.ReceiveNewsLetter;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"PersonID:{PersonID} PersonName:{PersonName} Email:{Email}" +
                $" DateOfBirth:{DateOfBirth?.ToString("dd MM yyyy")} Gender:{Gender} Address: {Address}" +
                $"CountryId:{CountryID} ReceivedNewsLetter: {ReceiveNewsLetter}";

        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {

                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                ReceiveNewsLetter = ReceiveNewsLetter,
                Address = Address,
                CountryID = CountryID,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true)
            };
        }

    }
    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person? person)
        {
            return new PersonResponse()
            {

                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                ReceiveNewsLetter = person.ReceiveNewsLetter,
                Address = person.Address,
                CountryID = person.CountryID,
                Gender = person.Gender,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
                Country = person.Country?.CountryName
            };
        }
       
    }


}
