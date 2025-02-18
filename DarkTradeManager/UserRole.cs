using System.ComponentModel.DataAnnotations;

namespace DarkTradeManager
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
