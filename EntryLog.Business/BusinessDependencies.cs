using System.Net.Http.Headers;
using EntryLog.Business.Constants;
using EntryLog.Business.Cryptography;
using EntryLog.Business.ImageBB;
using EntryLog.Business.Interfaces;
using EntryLog.Business.Mailtrap;
using EntryLog.Business.Mailtrap.Models;
using EntryLog.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EntryLog.Business;

public static class BusinessDependencies
{
    public static IServiceCollection AddBusinessDependecies(this IServiceCollection service, IConfiguration configuration)
    {
        // Configuracion IOptions
        service.Configure<MailtrapApiOptions>(configuration.GetSection("MailtrapApiOptions"));
        service.Configure<Argon2PasswordHashOptions>(configuration.GetSection("Argon2PasswordHashOptions"));
        service.Configure<EncryptionKeyValues>(configuration.GetSection("EncryptionKeyValues"));
        service.Configure<ImageBbOptions>(configuration.GetSection("ImageBbOptions"));
        
        // HttpClientFactory
        service.AddHttpClient(ApiNames.MailtrapIO, (sp, client) =>
        {
            // Obtener el servicio registrado en el IServiceCollection a traves de IServiceProvider
            MailtrapApiOptions options = sp.GetRequiredService<IOptions<MailtrapApiOptions>>().Value;
            
            client.BaseAddress = new Uri("https://send.api.mailtrap.io");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiToken);
        });

        service.AddHttpClient(ApiNames.ImageBb, (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<ImageBbOptions>>().Value;
            
            client.BaseAddress = new Uri(options.ApiUrl);
        });
        
        
        // Servicics de aplicación
        service.AddScoped<IAppUserService, AppUserService>();
        service.AddScoped<IWorkSessionService, WorkSessionService>();
        
        // Servicios de infraestructura
        service.AddScoped<IEmailSendService, MailtrapApiService>();
        service.AddScoped<IEncryptionService, RSAAsymmetricEncryptionService>();
        service.AddScoped<IPasswordHasherService, Argon2PasswordHasherService>();
        service.AddScoped<ILoadImagesService, ImageBbService>();


        return service;
    }
}