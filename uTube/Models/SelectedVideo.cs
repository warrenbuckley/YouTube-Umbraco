namespace uTube.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// View Model for selected videos
    /// </summary>
    public class SelectedVideo
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
