using System.Collections.Generic;
using System.Linq;

namespace YouTube.Models
{
    /// <summary>
    /// The YouTube return model
    /// </summary>
    public class SimpleYouTube : IEnumerable<SelectedVideo>
    {
        /// <summary>
        /// The selected videos.
        /// </summary>
        private readonly IEnumerable<SelectedVideo> _selectedVideos;

        /// <summary>
        /// Initializes a new instance of the <see cref="YouTube"/> class.
        /// </summary>
        /// <param name="selectedVideos">
        /// The selected videos.
        /// </param>
        public SimpleYouTube(IEnumerable<SelectedVideo> selectedVideos)
        {
            _selectedVideos = selectedVideos ?? new List<SelectedVideo>();
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        public IEnumerator<SelectedVideo> GetEnumerator()
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
