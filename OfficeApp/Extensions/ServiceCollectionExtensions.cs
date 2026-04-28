using OfficeApp.Filters;
using OfficeApp.Repositories.Implementations;
using OfficeApp.Repositories.Interfaces;
using OfficeApp.Services.Implementations;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IPartyRepository, PartyRepository>();
            services.AddScoped<IVehicleOwnerRepository, VehicleOwnerRepository>();
            services.AddScoped<ILRFormRepository, LRFormRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();

            // Services
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IPartyService, PartyService>();
            services.AddScoped<IVehicleOwnerService, VehicleOwnerService>();
            services.AddScoped<ILRFormService, LRFormService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<ILookupService, LookupService>();

            // Filters
            services.AddScoped<AuthenticationFilter>();

            return services;
        }
    }
}
