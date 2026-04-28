using OfficeApp.DTOs;
using System.ComponentModel.DataAnnotations;

namespace OfficeApp.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class LoginModelResponse
    {
        public int userId { get; set; }
        public string Username { get; set; }

        public string token { get; set; }
        public List<CompanyDto> companyIds { get; set; }
    }
}
