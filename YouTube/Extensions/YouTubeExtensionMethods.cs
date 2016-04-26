using Google.Apis.YouTube.v3.Data;

namespace YouTube.Extensions
{
    public static class YouTubeExtensionMethods
    {
        /// <summary>
        /// Embed a YouTube video with the Player API options
        /// https://developers.google.com/youtube/youtube_player_demo
        /// </summary>
        /// <param name="videoItem">The video item</param>
        /// <param name="width">The width of the video the height will be automatically calculated at 16:9 ratio. Default is 640</param>
        /// <param name="autoPlay">A boolean to enable the video to autoplay. Default is false</param>
        /// <param name="showControls">A boolean to show or hide the controls. Default is true</param>
        /// <param name="enableJsApi">A boolean to enable the video to be controlled with the JS API. Default is false</param>
        /// <param name="loop">A boolean to enable the video to loop when finished. Default is false</param>
        /// <param name="modestBranding">A boolean to show more modest YouTube branding & logos. Default is false</param>
        /// <param name="rel">A boolean to enable related videos to be displaed at the end of the video playing. Default is true</param>
        /// <param name="showInfo">A boolean to display the video title & rating before the video plays. Default is true</param>
        /// <param name="theme">An enum of styles that change the control styles</param>
        /// <param name="playlistId">The playlist id in which you would like to play</param>
        /// <returns>A raw string with the iframe embed HTML with the appeneded options</returns>
        public static string EmbedVideoWithOptions(this Video videoItem, int width = 640, bool autoPlay = false, bool showControls = true, bool enableJsApi = false, bool loop = false, bool modestBranding = false, bool rel = true, bool showInfo = true, Theme theme = Theme.dark, string playlistId = "")
        {
            // From width calculate height (16:9 ratio)
            // 640 width = 340 height
            // 640 / 16 = 40
            // 40 * 9 = 340
            var height = (width/16) * 9;

            //Convert booleans to 0 or 1 strings for YouTube querystring params
            var autoPlayString          = autoPlay ? "1" : "0";
            var showControlsString      = showControls ? "1" : "0";
            var enableJsApiString       = enableJsApi ? "1" : "0";
            var loopString              = loop ? "1" : "0";
            // if loop set to true and there is no playlist provided then loop the singular video 
            var playlistId              = loop && string.IsNullOrWhiteSpace(playlistId) ? videoItem.Id : playlistId; 
            var modestBrandingString    = modestBranding ? "1" : "0";
            var relString               = rel ? "1" : "0";
            var showInfoString          = showInfo ? "1" : "0";

            //If we use JS API ensure the ID for the iframe is unique
            var uniqueHtmlId            = string.Format("youtubevideo-{0}", videoItem.Id);

            var embedHtml = string.Format("<iframe id='{0}' type='text/html' width='{1}' height='{2}' src='https://www.youtube.com/embed/{3}?autoplay={4}&controls={5}&enablejsapi={6}&loop={7}&modestbranding={8}&rel={9}&showinfo={10}&theme={11}&playlist={12}' frameborder='0' allowfullscreen></iframe>",
                uniqueHtmlId, width, height, videoItem.Id, autoPlayString, showControlsString, enableJsApiString, loopString, modestBrandingString, relString, showInfoString, theme, playlistId);

            return embedHtml;
        }

        /// <summary>
        /// The themes available for the YouTube player
        /// Styles controls if they are visible
        /// </summary>
        public enum Theme
        {
            light,
            dark,
        };

    }
}
