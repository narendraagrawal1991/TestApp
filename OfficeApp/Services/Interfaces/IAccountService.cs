using OfficeApp.DTOs;
using OfficeApp.Models;

namespace OfficeApp.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginEntity?> ValidateLoginAsync(LoginDto dto);
    }
}
