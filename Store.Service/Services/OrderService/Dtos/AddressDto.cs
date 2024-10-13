using System.ComponentModel.DataAnnotations;

namespace Store.Service.Services.OrderService.Dtos
{
    public class AddressDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]

        public string city { get; set; }
        [Required]

        public string state { get; set; }
        [Required]

        public string Street { get; set; }
        [Required]

        public string PostalCode { get; set; }
    }
}