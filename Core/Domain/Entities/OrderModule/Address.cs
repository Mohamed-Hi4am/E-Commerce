using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderModule
{
    public class Address
    {
        public Address()
        {
            
        }
        public Address(string fristName, string lastName, string street, string city, string country)
        {
            FristName = fristName;
            LastName = lastName;
            Street = street;
            City = city;
            Country = country;
        }

        public string FristName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
