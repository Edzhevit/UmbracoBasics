using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using UmbracoDemo.Core.Interfaces;
using UmbracoDemo.Core.Services;

namespace UmbracoDemo.Core;

public class RegisterServicesComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddScoped<IEmailService, EmailService>();
    }
}