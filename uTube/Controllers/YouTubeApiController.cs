using System;
using System.Web.Http;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace uTube.Controllers
{
    [PluginController("uTube")]
    public class YouTubeApiController : UmbracoAuthorizedApiController
    {
        //http://localhost:50533/umbraco/backoffice/uTube/YouTubeApi/GetHello?name=warren
        [HttpGet]
        public string GetHello(string name)
        {
            return "Hello " + name;
        }

        //http://localhost:50533/umbraco/backoffice/uTube/YouTubeApi/testy
        [HttpGet]
        public SearchListResponse Testy()
        {
            var popularVideos = YouTube.GetVideosForChannel(null,"UC-lHJZR3Gqxm24_Vd_AJ5Yw", SearchResource.ListRequest.OrderEnum.Date);

            return popularVideos;
        }

        //TODO: Change to POST not GET
        //http://localhost:50533/umbraco/backoffice/uTube/YouTubeApi/VideosForChannel?pageToken=&channelId=UC-lHJZR3Gqxm24_Vd_AJ5Yw&orderBy=date
        [HttpGet]
        public SearchListResponse VideosForChannel(string pageToken, string channelId, string orderBy)
        {
            //Convert string orderby to the enum that we expect
            SearchResource.ListRequest.OrderEnum order;
            Enum.TryParse(orderBy, out order);

            //Go & get tthe videos
            var channelVideos = YouTube.GetVideosForChannel(pageToken, channelId, order);

            return channelVideos;
        }
    }
}
