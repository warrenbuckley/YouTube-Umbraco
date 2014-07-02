namespace uTube
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web;

    using uTube.Models;

    /// <summary>
    /// The uTube property value converter.
    /// </summary>
    [PropertyValueType(typeof(uTube))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class uTubeValueConverter : PropertyValueConverterBase
    {
        /// <summary>
        /// Checks if this converter can convert the property editor and registers if it can.
        /// </summary>
        /// <param name="propertyType">
        /// The published property type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.Equals("uTube.channel");
        }

        /// <summary>
        /// Convert the JSON data into a array
        /// </summary>
        /// <param name="propertyType">
        /// The published property type.
        /// </param>
        /// <param name="source">
        /// The value of the property
        /// </param>
        /// <param name="preview">
        /// The preview.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            if (source != null)
            {
                var selectedVideoList = JsonConvert.DeserializeObject<List<SelectedVideo>>(source.ToString());
                if (selectedVideoList != null && selectedVideoList.Any())
                {
                    return selectedVideoList.Select(x => x.Id).ToArray();
                }
            }

            return new string[0];
        }

        /// <summary>
        /// Convert the source nodeId into a Video object
        /// </summary>
        /// <param name="propertyType">
        /// The published property type.
        /// </param>
        /// <param name="source">
        /// The value of the property
        /// </param>
        /// <param name="preview">
        /// The preview.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            var selectedVideos = (string[])source;
            return UmbracoContext.Current != null ? new uTube(selectedVideos) : null;
        }
    }
}
