using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoDemo.Core.Interfaces;
using UmbracoDemo.Core.ViewModel;

namespace UmbracoDemo.Core.Controllers;

public class ContactFormController : SurfaceController
{
    private readonly IContentService _contentService;
    private readonly UmbracoHelper _helper;
    private readonly ILogger<ContactFormController> _logger;
    private readonly IEmailService _emailService;

    public ContactFormController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, IContentService contentService, ILogger<ContactFormController> logger, IEmailService emailService, UmbracoHelper helper) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _contentService = contentService;
        _logger = logger;
        _emailService = emailService;
        _helper = helper;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Submit(ContactFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("Error", "Please check the form.");
            return CurrentUmbracoPage();
        }
        var siteSettings = _helper.ContentAtRoot().DescendantsOrSelfOfType("siteSettings").FirstOrDefault();

        var isCaptchaValid = IsCaptchaValid(Request.Form["GoogleCaptchaToken"], siteSettings.Value<string>("recaptchaSecretKey"));
        if (!isCaptchaValid)
        {
            ModelState.AddModelError("Captcha", "The captcha is not valid.");
            return CurrentUmbracoPage();
        }

        try
        {
            var contactForms = _helper.ContentAtRoot().DescendantsOrSelfOfType("contactForms").FirstOrDefault();
            if (contactForms != null)
            {
                var newContactForm = _contentService.Create("Contact", contactForms.Id, "contactForm");
                newContactForm.SetValue("contactName", model.Name);
                newContactForm.SetValue("contactEmail", model.EmailAddress);
                newContactForm.SetValue("contactSubject", model.Subject);
                newContactForm.SetValue("contactComments", model.Comment);
                _contentService.SaveAndPublish(newContactForm);
            }

            _emailService.SendContactNotificationToAdmin(model);
            TempData["status"] = "OK";

            return RedirectToCurrentUmbracoPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError("There was an error in the contact form submission", e.Message);
            ModelState.AddModelError("Error", "Sorry there was a problem noting your details.");
        }

        return CurrentUmbracoPage();
    }

    private bool IsCaptchaValid(string token, string secret)
    {
        HttpClient httpClient = new HttpClient();
        var res = httpClient.GetAsync(
            $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}").Result;

        if (res.StatusCode != HttpStatusCode.OK)
        {
            return false;
        }

        var jsonRes = res.Content.ReadAsStringAsync().Result;
        dynamic jsonData = JObject.Parse(jsonRes);
        return true;
    }
}