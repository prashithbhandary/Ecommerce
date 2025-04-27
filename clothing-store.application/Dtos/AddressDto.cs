using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.Dtos
{
    public class AddAddressDto
    {
        [Required] public string name { get; set; }
        [Required] public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        [Required] public string City { get; set; }
        [Required] public string State { get; set; }
        [Required] public string PostalCode { get; set; }
        [Required] public string Country { get; set; }
        [Required] public int UserId { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class UpdateAddressDto : AddAddressDto
    {
        [Required] public int Id { get; set; }
    }
}
