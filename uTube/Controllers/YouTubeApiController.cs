using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using uTube.Models;

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

        //http://localhost:50533/umbraco/backoffice/uTube/YouTubeApi/VideosForChannel
        [HttpPost]
        public SearchListResponse VideosForChannel(ApiModel model)
        {
            //Convert string orderby to the enum that we expect
            try
            {
                SearchResource.ListRequest.OrderEnum order = (SearchResource.ListRequest.OrderEnum)Enum.Parse(typeof(SearchResource.ListRequest.OrderEnum), model.OrderBy, true);

                //Go & get the videos
                var channelVideos = YouTube.GetVideosForChannel(model.PageToken, model.ChannelId, model.SearchQuery, order);

                //Return the response from YouTube API
                return channelVideos;

            }
            catch (ArgumentException)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                message.Content = new StringContent("Order by cannot be converted to the enum");

                throw new HttpResponseException(message);
            }
        }
    }
}
