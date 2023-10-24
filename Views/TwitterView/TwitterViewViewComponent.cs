using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Umbraco.Cms.Core;
using UmbracoDemo.Core.ViewModel;

namespace UmbracoDemo.Web.Views.TwitterView;

[ViewComponent]
public class TwitterViewViewComponent : ViewComponent
{
    private readonly IPublishedContentQuery _contentQuery;

    public TwitterViewViewComponent(IPublishedContentQuery contentQuery)
    {
        _contentQuery = contentQuery;
    }

    public IViewComponentResult Invoke(string twitterHandle)
    {
        var model = new TwitterViewModel { TwitterHandle = twitterHandle };

        try
        {
            var tweets = GetLatestTweetsFromJson(twitterHandle, 4);
            model.Json = tweets;
            model.Error = false;
        }
        catch (Exception e)
        {
            model.Error = true;
            model.Message = e.Message;
        }

        return View(model);
    }

    private string GetLatestTweetsFromJson(string twitterHandler, int i)
    {
        return
            "{\r\n      \"text\": \"RT @PostGradProblem: In preparation for the NFL lockout, I will be spending twice as much time analyzing my fantasy baseball team during ...\", \r\n      \"truncated\": true, \r\n      \"in_reply_to_user_id\": null, \r\n      \"in_reply_to_status_id\": null, \r\n      \"favorited\": false, \r\n      \"source\": \"<a href=\\\"http://twitter.com/\\\" rel=\\\"nofollow\\\">Twitter for iPhone</a>\", \r\n      \"in_reply_to_screen_name\": null, \r\n      \"in_reply_to_status_id_str\": null, \r\n      \"id_str\": \"54691802283900928\", \r\n      \"entities\": {\r\n            \"user_mentions\": [\r\n                  {\r\n                        \"indices\": [\r\n                              3, \r\n                              19\r\n                        ], \r\n                        \"screen_name\": \"PostGradProblem\", \r\n                        \"id_str\": \"271572434\", \r\n                        \"name\": \"PostGradProblems\", \r\n                        \"id\": 271572434\r\n                  }\r\n            ], \r\n            \"urls\": [ ], \r\n            \"hashtags\": [ ]\r\n      }, \r\n      \"contributors\": null, \r\n      \"retweeted\": false, \r\n      \"in_reply_to_user_id_str\": null, \r\n      \"place\": null, \r\n      \"retweet_count\": 4, \r\n      \"created_at\": \"Sun Apr 03 23:48:36 +0000 2011\", \r\n      \"retweeted_status\": {\r\n            \"text\": \"In preparation for the NFL lockout, I will be spending twice as much time analyzing my fantasy baseball team during company time. #PGP\", \r\n            \"truncated\": false, \r\n            \"in_reply_to_user_id\": null, \r\n            \"in_reply_to_status_id\": null, \r\n            \"favorited\": false, \r\n            \"source\": \"<a href=\\\"http://www.hootsuite.com\\\" rel=\\\"nofollow\\\">HootSuite</a>\", \r\n            \"in_reply_to_screen_name\": null, \r\n            \"in_reply_to_status_id_str\": null, \r\n            \"id_str\": \"54640519019642881\", \r\n            \"entities\": {\r\n                  \"user_mentions\": [ ], \r\n                  \"urls\": [ ], \r\n                  \"hashtags\": [\r\n                        {\r\n                              \"text\": \"PGP\", \r\n                              \"indices\": [\r\n                                    130, \r\n                                    134\r\n                              ]\r\n                        }\r\n                  ]\r\n            }, \r\n            \"contributors\": null, \r\n            \"retweeted\": false, \r\n            \"in_reply_to_user_id_str\": null, \r\n            \"place\": null, \r\n            \"retweet_count\": 4, \r\n            \"created_at\": \"Sun Apr 03 20:24:49 +0000 2011\", \r\n            \"user\": {\r\n                  \"notifications\": null, \r\n                  \"profile_use_background_image\": true, \r\n                  \"statuses_count\": 31, \r\n                  \"profile_background_color\": \"C0DEED\", \r\n                  \"followers_count\": 3066, \r\n                  \"profile_image_url\": \"http://a2.twimg.com/profile_images/1285770264/PGP_normal.jpg\", \r\n                  \"listed_count\": 6, \r\n                  \"profile_background_image_url\": \"http://a3.twimg.com/a/1301071706/images/themes/theme1/bg.png\", \r\n                  \"description\": \"\", \r\n                  \"screen_name\": \"PostGradProblem\", \r\n                  \"default_profile\": true, \r\n                  \"verified\": false, \r\n                  \"time_zone\": null, \r\n                  \"profile_text_color\": \"333333\", \r\n                  \"is_translator\": false, \r\n                  \"profile_sidebar_fill_color\": \"DDEEF6\", \r\n                  \"location\": \"\", \r\n                  \"id_str\": \"271572434\", \r\n                  \"default_profile_image\": false, \r\n                  \"profile_background_tile\": false, \r\n                  \"lang\": \"en\", \r\n                  \"friends_count\": 21, \r\n                  \"protected\": false, \r\n                  \"favourites_count\": 0, \r\n                  \"created_at\": \"Thu Mar 24 19:45:44 +0000 2011\", \r\n                  \"profile_link_color\": \"0084B4\", \r\n                  \"name\": \"PostGradProblems\", \r\n                  \"show_all_inline_media\": false, \r\n                  \"follow_request_sent\": null, \r\n                  \"geo_enabled\": false, \r\n                  \"profile_sidebar_border_color\": \"C0DEED\", \r\n                  \"url\": null, \r\n                  \"id\": 271572434, \r\n                  \"contributors_enabled\": false, \r\n                  \"following\": null, \r\n                  \"utc_offset\": null\r\n            }, \r\n            \"id\": 54640519019642880, \r\n            \"coordinates\": null, \r\n            \"geo\": null\r\n      }, \r\n      \"user\": {\r\n            \"notifications\": null, \r\n            \"profile_use_background_image\": true, \r\n            \"statuses_count\": 351, \r\n            \"profile_background_color\": \"C0DEED\", \r\n            \"followers_count\": 48, \r\n            \"profile_image_url\": \"http://a1.twimg.com/profile_images/455128973/gCsVUnofNqqyd6tdOGevROvko1_500_normal.jpg\", \r\n            \"listed_count\": 0, \r\n            \"profile_background_image_url\": \"http://a3.twimg.com/a/1300479984/images/themes/theme1/bg.png\", \r\n            \"description\": \"watcha doin in my waters?\", \r\n            \"screen_name\": \"OldGREG85\", \r\n            \"default_profile\": true, \r\n            \"verified\": false, \r\n            \"time_zone\": \"Hawaii\", \r\n            \"profile_text_color\": \"333333\", \r\n            \"is_translator\": false, \r\n            \"profile_sidebar_fill_color\": \"DDEEF6\", \r\n            \"location\": \"Texas\", \r\n            \"id_str\": \"80177619\", \r\n            \"default_profile_image\": false, \r\n            \"profile_background_tile\": false, \r\n            \"lang\": \"en\", \r\n            \"friends_count\": 81, \r\n            \"protected\": false, \r\n            \"favourites_count\": 0, \r\n            \"created_at\": \"Tue Oct 06 01:13:17 +0000 2009\", \r\n            \"profile_link_color\": \"0084B4\", \r\n            \"name\": \"GG\", \r\n            \"show_all_inline_media\": false, \r\n            \"follow_request_sent\": null, \r\n            \"geo_enabled\": false, \r\n            \"profile_sidebar_border_color\": \"C0DEED\", \r\n            \"url\": null, \r\n            \"id\": 80177619, \r\n            \"contributors_enabled\": false, \r\n            \"following\": null, \r\n            \"utc_offset\": -36000\r\n      }, \r\n      \"id\": 54691802283900930, \r\n      \"coordinates\": null, \r\n      \"geo\": null\r\n}";
    }

    private string GetLatestTweets(string twitterHandle, int numberOfTweets)
    {
        var siteSettings = _contentQuery.ContentAtRoot().First(c => c.ContentType.Alias == "siteSettings");

        var oauth_token = siteSettings.Value<string>("twitterAccessToken");
        var oauth_token_secret = siteSettings.Value<string>("twitterAccessTokenSecret");
        var oauth_consumer_key = siteSettings.Value<string>("twitterConsumerApiKey");
        var oauth_consumer_secret = siteSettings.Value<string>("twitterConsumerSecretApiKey");

        //Get the data from twitter
        var excludeReplies = "true";
        var trimUser = "true";
        var extendedTweets = "extended";

        //oauth
        var oauth_version = "1.0";
        var oauth_signature_method = "HMAC-SHA1";
        var oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
        var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

        //construct url
        var resource_url = "https://api.twitter.com/1.1/statuses/user_timeline.json";

        //encrypted oAuth signature
        var baseFormat = "count={0}&exclude_replies={1}&oauth_consumer_key={2}&oauth_nonce={3}&oauth_signature_method={4}" +
            "&oauth_timestamp={5}&oauth_token={6}&oauth_version={7}&screen_name={8}&trim_user={9}&tweet_mode={10}";

        var baseString = string.Format(baseFormat,
                                    numberOfTweets,
                                    excludeReplies,
                                    oauth_consumer_key,
                                    oauth_nonce,
                                    oauth_signature_method,
                                    oauth_timestamp,
                                    oauth_token,
                                    oauth_version,
                                    twitterHandle,
                                    trimUser,
                                    extendedTweets
                                    );

        baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url),
                        "&", Uri.EscapeDataString(baseString));

        //Encrypt data

        var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                    "&", Uri.EscapeDataString(oauth_token_secret));

        string oauth_signature;
        using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
        {
            oauth_signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
        }



        var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                "oauth_version=\"{6}\"";

        var authHeader = string.Format(headerFormat,
                                Uri.EscapeDataString(oauth_nonce),
                                Uri.EscapeDataString(oauth_signature_method),
                                Uri.EscapeDataString(oauth_timestamp),
                                Uri.EscapeDataString(oauth_consumer_key),
                                Uri.EscapeDataString(oauth_token),
                                Uri.EscapeDataString(oauth_signature),
                                Uri.EscapeDataString(oauth_version)
                        );


        ServicePointManager.Expect100Continue = false;
        var postBody = "screen_name=" + twitterHandle;
        postBody = postBody + string.Format("&count={0}&trim_user={1}&exclude_replies={2}&tweet_mode={3}", numberOfTweets, trimUser, excludeReplies, extendedTweets);
        resource_url += "?" + postBody;


        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
        request.Headers.Add("Authorization", authHeader);
        request.Method = "GET";
        request.ContentType = "application/x-www-form-urlencoded";
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        WebResponse response = request.GetResponse();
        string responseData;

        //read from twitter
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            responseData = reader.ReadToEnd();

        }

        return responseData;
    }
}