using OfficeApp.DTOs;

namespace OfficeApp.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<List<CompanyDto>> GetAllCompaniesAsync();
        Task<List<CompanyDto>> GetAllCompaniesAsync(List<int> companyIds);
        Task<CompanyDto?> GetCompanyByIdAsync(int id);
        Task CreateCompanyAsync(CompanyDto dto);
        Task UpdateCompanyAsync(int id, CompanyDto dto);
        Task DeleteCompanyAsync(int id);
    }
}
