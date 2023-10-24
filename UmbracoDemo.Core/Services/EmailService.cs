using System.Net.Mail;
using System.Net.Mime;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;
using UmbracoDemo.Core.Interfaces;
using UmbracoDemo.Core.ViewModel;

namespace UmbracoDemo.Core.Services;

public class EmailService : IEmailService
{
    private readonly UmbracoHelper _helper;
    private readonly IContentService _contentService;

    public EmailService(UmbracoHelper helper, IContentService contentService)
    {
        _helper = helper;
        _contentService = contentService;
    }

    public void SendContactNotificationToAdmin(ContactFormViewModel model)
    {
        var emailTemplate = GetEmailTemplate("New Contact Form Notification");
        if (emailTemplate == null)
        {
            throw new Exception("Template not found");
        }

        var subject = emailTemplate.Value<string>("emailTemplateSubjectLine");
        var htmlContent = emailTemplate.Value<string>("emailTemplateHtmlContent");
        var textContent = emailTemplate.Value<string>("emailTemplateTextContent");

        htmlContent = htmlContent.Replace("##name##", model.Name);
        htmlContent = htmlContent.Replace("##email##", model.EmailAddress);
        htmlContent = htmlContent.Replace("##comment##", model.Comment);

        textContent = textContent.Replace("##name##", model.Name);
        textContent = textContent.Replace("##email##", model.EmailAddress);
        textContent = textContent.Replace("##comment##", model.Comment);

        var siteSettings = _helper.ContentAtRoot().DescendantsOrSelfOfType("siteSettings").FirstOrDefault();
        if (siteSettings == null)
        {
            throw new Exception("There are no site settings.");
        }

        var fromAddress = siteSettings.Value<string>("emailSettingsFromAddress");
        var toAddresses = siteSettings.Value<string>("emailSettingsAdminAccounts");

        if (string.IsNullOrEmpty(fromAddress))
        {
            throw new Exception("There needs to be a from address in site settings.");
        }

        if (string.IsNullOrEmpty(toAddresses))
        {
            throw new Exception("There needs to be a to address in site settings.");
        }

        var debugMode = siteSettings.Value<bool>("testMode");
        var testEmailAccounts = siteSettings.Value<string>("emailTestAccounts");
        var recipients = toAddresses;
        if (debugMode)
        {
            recipients = testEmailAccounts;
            subject += "(TEST MODE) - " + toAddresses;
        }

        var emails = _helper.ContentAtRoot().DescendantsOrSelfOfType("emails").FirstOrDefault();
        var newEmail = _contentService.Create(toAddresses, emails.Id, "Email");
        newEmail.SetValue("emailSubject", subject);
        newEmail.SetValue("emailToAddress", recipients);
        newEmail.SetValue("emailHtmlContent", htmlContent);
        newEmail.SetValue("emailTextContent", textContent);
        newEmail.SetValue("emailSent", false);
        _contentService.SaveAndPublish(newEmail);

        var mimeType = new ContentType("text/html");
        var alternateView = AlternateView.CreateAlternateViewFromString(htmlContent, mimeType);

        var smtpMessage = new MailMessage();
        smtpMessage.AlternateViews.Add(alternateView);
        smtpMessage.Subject = subject;
        smtpMessage.Body = textContent;
        smtpMessage.From = new MailAddress(fromAddress);
        var toList = recipients.Split(",");
        foreach (var item in toList)
        {
            if (!string.IsNullOrEmpty(item))
            {
                smtpMessage.To.Add(item);
            }
        }
        
        using var smtp = new SmtpClient();
        smtp.Host = "localhost";
        smtp.Port = 25;
        smtp.Send(smtpMessage);
        newEmail.SetValue("emailSent", true);
        newEmail.SetValue("emailSentDate", DateTime.Now);
        _contentService.SaveAndPublish(newEmail);
    }

    private IPublishedContent GetEmailTemplate(string templateName)
    {
        var templates = _helper.ContentAtRoot().DescendantsOrSelfOfType("emailTemplate");
        var template = templates.FirstOrDefault(x => x.Name == templateName);
        return template;
    }
}