using OfficeApp.DTOs;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repo;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(ICompanyRepository repo, ILogger<CompanyService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<List<CompanyDto>> GetAllCompaniesAsync()
        {
            var companies = await _repo.GetAllAsync();
            return companies.Select(MapToDto).ToList();
        }

        public async Task<List<CompanyDto>> GetAllCompaniesAsync(List<int> companyIds)
        {
            var companies = await _repo.GetAllAsync(companyIds);
            return companies.Select(MapToDto).ToList();
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
        {
            var company = await _repo.GetByIdAsync(id);
            return company == null ? null : MapToDto(company);
        }

        public async Task CreateCompanyAsync(CompanyDto dto)
        {
            var entity = MapToEntity(dto);
            await _repo.AddAsync(entity);
            _logger.LogInformation("Company '{Name}' created with ID {Id}", entity.CompanyName, entity.CompanyId);
        }

        public async Task UpdateCompanyAsync(int id, CompanyDto dto)
        {
            var entity = MapToEntity(dto);
            entity.CompanyId = id;
            await _repo.UpdateAsync(entity);
            _logger.LogInformation("Company ID {Id} updated", id);
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity != null)
            {
                await _repo.DeleteAsync(entity);
                _logger.LogInformation("Company ID {Id} deleted", id);
            }
        }

        private static CompanyDto MapToDto(Company c) => new()
        {
            CompanyId = c.CompanyId,
            CompanyName = c.CompanyName,
            Address = c.Address,
            City = c.City,
            State = c.State,
            Pincode = c.Pincode,
            ContactNo = c.ContactNo,
            EmailId = c.EmailId,
            GSTNo = c.GSTNo
        };

        private static Company MapToEntity(CompanyDto d) => new()
        {
            CompanyId = d.CompanyId,
            CompanyName = d.CompanyName,
            Address = d.Address,
            City = d.City,
            State = d.State,
            Pincode = d.Pincode,
            ContactNo = d.ContactNo,
            EmailId = d.EmailId,
            GSTNo = d.GSTNo
        };
    }
}
