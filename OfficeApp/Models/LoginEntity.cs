using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeApp.Models
{
    [Table("UserLogin")]
    public class LoginEntity
    {
        public string CompanyId { get; set; }

        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
