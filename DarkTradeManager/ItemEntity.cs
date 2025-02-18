using System.ComponentModel.DataAnnotations;

namespace DarkTradeManager
{
    public class ItemEntity
    {
        [Key]
        public string ArticleNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        // Здесь хранится путь к изображению
        public string ImagePath { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        [Required]
        public decimal Cost { get; set; }

        public byte? Discount { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
