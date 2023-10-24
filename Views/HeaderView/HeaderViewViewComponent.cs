using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using UmbracoDemo.Core.ViewModel;

namespace UmbracoDemo.Web.Views.HeaderView;

[ViewComponent]
public class HeaderViewViewComponent : ViewComponent
{
    private readonly IUmbracoContextAccessor _contextAccessor;

    public HeaderViewViewComponent(IUmbracoContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public IViewComponentResult Invoke()
    {
        var headerView = new HeaderViewModel();
        var content = _contextAccessor.GetRequiredUmbracoContext().PublishedRequest.PublishedContent;
        if (content is null)
        {
            return View(headerView);
        }

        headerView.Title = content.Value<string>("title");
        headerView.SubTitle = content.Value<string>("subTitle");
        headerView.ImageUrl = content.Value<IPublishedContent>("pageBanner").Url();
        return View(headerView);
    }
}