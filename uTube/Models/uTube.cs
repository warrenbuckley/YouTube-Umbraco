namespace uTube.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The uTube return model
    /// </summary>
    public class uTube : IEnumerable<Google.Apis.YouTube.v3.Data.Video>
    {
        /// <summary>
        /// The selected videos.
        /// </summary>
        private readonly List<Google.Apis.YouTube.v3.Data.Video> _selectedVideos = new List<Google.Apis.YouTube.v3.Data.Video>();

        /// <summary>
        /// Initializes a new instance of the <see cref="uTube"/> class.
        /// </summary>
        /// <param name="selectedVideos">
        /// The selected videos.
        /// </param>
        public uTube(string[] selectedVideos)
        {
            if (selectedVideos != null)
            {
                var videoListResponse = YouTube.GetVideo(string.Join(", ", selectedVideos));
                foreach (var video in videoListResponse.Items)
                {
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
        public IEnumerator<Google.Apis.YouTube.v3.Data.Video> GetEnumerator()
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
