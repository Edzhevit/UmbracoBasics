using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.WebAssets;

namespace UmbracoDemo.Web;

public class CreateBundlesNotificationHandler : INotificationHandler<UmbracoApplicationStartingNotification>
{
    private readonly IRuntimeMinifier _runtimeMinifier;
    private readonly IRuntimeState _runtimeState;

    public CreateBundlesNotificationHandler(IRuntimeMinifier runtimeMinifier, IRuntimeState runtimeState)
    {
        _runtimeMinifier = runtimeMinifier;
        _runtimeState = runtimeState;
    }

    public void Handle(UmbracoApplicationStartingNotification notification)
    {
        if (_runtimeState.Level == RuntimeLevel.Run)
        {
            _runtimeMinifier.CreateJsBundle("registered-js-bundle",
                BundlingOptions.OptimizedAndComposite,
                new[] { "~/scripts/CookieConsent.js", });

            _runtimeMinifier.CreateCssBundle("registered-css-bundle",
                BundlingOptions.OptimizedAndComposite,
                new[] { "~/css/Main.css", "~/css/CookieConsent.css", "~/css/RTE.css" });
        }
    }
}