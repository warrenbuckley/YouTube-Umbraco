using System;
using System.Web.Configuration;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTube
{
    using System.Linq;

    public class YouTubeHelper
    {
        //CONSTANTS
        private const string _ApiKey            = "AIzaSyAgXB3nYk3f00eXZd0FGsUjJySf2Fnp7KA";
        private const string _ApplicationName   = "YouTube for Umbraco";
        private const int _noPerPage            = 3;

        /// <summary>
        /// Gets the YouTube Service that we use for all requests
        /// </summary>
        /// <returns></returns>
        public static YouTubeService GetYouTubeService()
        {
            var youTubeService = new YouTubeService(new BaseClientService.Initializer()
            { 
                ApiKey          = WebConfigurationManager.AppSettings["YouTube-Umbraco:ApiKey"] ?? _ApiKey,
                ApplicationName = _ApplicationName
            });

            return youTubeService;
        }

        
        public static SearchListResponse GetVideosForChannel(string pageToken, string channelId, string searchQuery, SearchResource.ListRequest.OrderEnum orderBy)
        {
            //Get YouTube Service
            var youTube = GetYouTubeService();

            //Build up request
            var videoRequest        = youTube.Search.List("snippet");
            videoRequest.ChannelId  = channelId;                        //Get videos for Channel only
            videoRequest.Order      = orderBy;                          //Order by the view count/date (ENum Passed in)
            videoRequest.MaxResults = _noPerPage;                       //3 per page
            videoRequest.Type       = "video";                          //Only get videos, as searches can return results for channel & other types
            videoRequest.PageToken  = pageToken;                        //If more than 3 videos, we can request more videos using a page token (previous & next)

            //If we have a search query then...
            if (!string.IsNullOrEmpty(searchQuery))
            {
                //Change the order by from Date/Views etc to relevance
                //and specify the search query
                videoRequest.Order  = SearchResource.ListRequest.OrderEnum.Relevance;
                videoRequest.Q      = searchQuery;
            }

            //Perform request
            var videoResponse = videoRequest.Execute();

            //Return the list of videos we find
            return videoResponse;
        }


        /// <summary>
        /// Get specific details about a video
        /// </summary>
        /// <param name="videoId">Pass in the YouTube video ID</param>
        /// <returns></returns>
        /// https://www.googleapis.com/youtube/v3/videos?part=snippet%2Cstatistics&id=gRyPjRrjS34&key=AIzaSyAgXB3nYk3f00eXZd0FGsUjJySf2Fnp7KA
        public static VideoListResponse GetVideo(string videoId)
        {
            // Get YouTube Service
            var youTube = GetYouTubeService();

            // TODO: Inspect request properly & see what we actually need or not
            var videoRequest    = youTube.Videos.List("snippet, contentDetails, liveStreamingDetails, player, recordingDetails, statistics, status");
            //var videoRequest    = youTube.Videos.List(String.Empty);
            videoRequest.Id     = videoId;

            // Perform request
            var videoResponse = videoRequest.Execute();

            return videoResponse;
        }


        public static ChannelListResponse GetChannelFromUsername(string usernameToQuery)
        {
            var youTube = GetYouTubeService();
            var channelQueryRequest = youTube.Channels.List("snippet,id,contentDetails,statistics,topicDetails");
            channelQueryRequest.ForUsername = usernameToQuery;
            channelQueryRequest.MaxResults = 1;

            // Perform request
            var channelResponse = channelQueryRequest.Execute();

            // If no items found in channel query attempt a search and find the channel id of the first item
            if (!channelResponse.Items.Any())
            {
                var searchRequest = youTube.Search.List("snippet");
                searchRequest.Q = usernameToQuery;
                searchRequest.MaxResults = 1;
                searchRequest.Type = "channel";
                var searchResponse = searchRequest.Execute();
                if (searchResponse.Items.Any())
                {
                    var channelId = searchResponse.Items.First().Snippet.ChannelId;
                    channelResponse = GetChannelFromId(channelId);
                }
            }

            return channelResponse;
        }

        public static ChannelListResponse GetChannelFromId(string channelId)
        {
            var youTube = GetYouTubeService();

            var channelQueryRequest         = youTube.Channels.List("snippet,id,contentDetails,statistics,topicDetails");
            channelQueryRequest.Id          = channelId;
            channelQueryRequest.MaxResults  = 1;

            // Perform request
            var channelResponse = channelQueryRequest.Execute();

            return channelResponse;
        }
    }
}
