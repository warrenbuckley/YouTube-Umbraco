using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.UI.JavaScript;
using YouTube.Controllers;

namespace YouTube
{
    public class UmbracoStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;
        }

        /// <summary>
        /// When logged into Umbraco backoffice - add to the Umbraco JSON collection
        /// So that we can get the API URL for YouTube API calls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> e)
        {
            //Make sure we have a HTTP Context - otherwise can't set this up
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("HttpContext is null");
            }

            //Create a .NET MVC URL Helper
            var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));
            
            //Add to dictionary (uTube)
            e.Add("YouTube", new Dictionary<string, object>
            {
                //Then child item of uTube - Add Base Url to controller - minus the method name
                {"ApiUrl", urlHelper.GetUmbracoApiServiceBaseUrl<YouTubeApiController>(controller => controller.VideosForChannel(null))},
            });
        }

    }
}
