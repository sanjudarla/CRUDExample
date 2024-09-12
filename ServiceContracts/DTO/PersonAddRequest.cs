using Entities;
using ServiceContracts.Enums;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person Name cant be empty")]
        public string? PersonName { get; set; }


        [Required(ErrorMessage = "Email Address cant be empty")]
        [EmailAddress(ErrorMessage = "Email Value should be valid")]

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender Cannot be blank")]
        public GenderOptions? Gender { get; set; }
        [Required(ErrorMessage ="Please Select a Country")]
        public Guid? CountryID { get; set; }
        [Required(ErrorMessage = "Address Cannot be blank")]
        public string? Address { get; set; }

        public bool ReceiveNewsLetter { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                Address = Address,
                ReceiveNewsLetter = ReceiveNewsLetter
            };
        }
    }
}
