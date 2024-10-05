using System.ComponentModel.DataAnnotations;

namespace Store.Service.Services.basketService.CustomerBasketDto
{
    public class BasketItemDto
    {
        [Required]
        [Range(1, int.MaxValue)]

        public int ProductId { get; set; }
        [Required]

        public string ProductName { get; set; }
        [Required]
        [Range (0.1, double.MaxValue,ErrorMessage ="price Must be Greater than zero")]
        public decimal Price { get; set; }

        [Required]
        [Range(1,10,  ErrorMessage = "Quntity Must be Between 1 to 10")]
        public int Quantity { get; set; }
        [Required]

        public string PictureUrl { get; set; }
        [Required]

        public string BrandName { get; set; }
        [Required]

        public string TypeName { get; set; }
    }
}