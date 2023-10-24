using UmbracoDemo.Core.ViewModel;

namespace UmbracoDemo.Core.Interfaces;

public interface IEmailService
{
    void SendContactNotificationToAdmin(ContactFormViewModel model);
}