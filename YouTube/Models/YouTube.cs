using System.Collections.Generic;
using Google.Apis.YouTube.v3.Data;

namespace YouTube.Models
{
    /// <summary>
    /// The YouTube return model
    /// </summary>
    public class YouTube : IEnumerable<Video>
    {
        /// <summary>
        /// The selected videos.
        /// </summary>
        private readonly List<Video> _selectedVideos = new List<Video>();

        /// <summary>
        /// Initializes a new instance of the <see cref="YouTube"/> class.
        /// </summary>
        /// <param name="selectedVideos">
        /// The selected videos.
        /// </param>
        public YouTube(string[] selectedVideos)
        {
            if (selectedVideos != null)
            {
                var videoListResponse = YouTubeHelper.GetVideo(string.Join(", ", selectedVideos));
                foreach (var video in videoListResponse.Items)
                {
                    video.Player.EmbedHtml = video.Player.EmbedHtml.Replace("/>", "></iframe>");
                    this._selectedVideos.Add(video);
                }
            }
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        public IEnumerator<Video> GetEnumerator()
        {
            return this._selectedVideos.GetEnumerator();
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
