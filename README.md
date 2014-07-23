YouTube for Umbraco (V7+)
=====
This is a project for adding YouTube videos to your Umbraco site.
Currently it only consists of a single property editor that allows you to easily pick videos from a specific YouTube channel.

##YouTube Channel Picker
Currently this package consists of the Google .NET C# Library for YouTube API's along with a V7 Angular Property Editor for selecting video/s from a YouTube channel.

###Settings (Prevalues)
The YouTube Channel picker has two settings or prevalues as it's more commonly known:

####Order By
Order the videos by:
* Date
* Title
* Views
* Ratings

####Channel ID
You can find your YouTube channel ID by browsing the YouTube site at the following page:
http://www.youtube.com/account_advanced

###Package File Contents
####DLLs in the Bin Folder
* bin/System.Net.Http.Primitives.dll
* bin/Microsoft.Threading.Tasks.dll
* bin/Microsoft.Threading.Tasks.Extensions.dll
* bin/Google.Apis.Auth.dll
* bin/Google.Apis.Auth.PlatformServices.dll
* bin/Google.Apis.Core.dll
* bin/Google.Apis.Core.pdb
* bin/Google.Apis.dll
* bin/Google.Apis.pdb
* bin/Google.Apis.PlatformServices.dll
* bin/Google.Apis.YouTube.v3.dll
* bin/Google.Apis.YouTube.v3.pdb
* bin/YouTube.Umbraco.dll
* bin/YouTube.Umbraco.pdb

####Package Files
* App_Plugins/YouTube/package.manifest
* App_Plugins/YouTube/css/YouTube.css
* App_Plugins/YouTube/js/YouTube.js
* App_Plugins/YouTube/views/preValues/YouTube.OrderBy.html
* App_Plugins/YouTube/views/propertyEditors/YouTube.Channel.html


##Use in templates
With the thanks to Jeavon Leopold he has created a property value convertor that takes our simple JSON that we save to the Umbraco DB and makes a more useful strongly typed object to work with.
```csharp
@using YouTube.Models

@foreach (var typedVideo in Model.Content.GetPropertyValue<YouTube>("video"))
{   
    <div>@Html.Raw(typedVideo.Player.EmbedHtml)</div>
    @typedVideo.Snippet.Title<br/>
    @typedVideo.Statistics.ViewCount<br/>
    @typedVideo.Snippet.PublishedAt.Value.ToShortDateString()
}
```

Or override the sort order and order by values on the video, such as view count
```csharp
@foreach (var typedVideo in Model.Content.GetPropertyValue<YouTube>("video").OrderByDescending(x => x.Statistics.ViewCount))
{
    <div>@Html.Raw(typedVideo.Player.EmbedHtml)</div>
    @typedVideo.Snippet.Title<br />
    @typedVideo.Statistics.ViewCount<br />
    @typedVideo.Snippet.PublishedAt.Value.ToShortDateString()
}
```


###Thanks
Many thanks to Jeavon Leopold who helped collab on this project and creating the C# PropertyValue Convertor, which enables us to turn the simple JSON object we store in Umbraco of YouTube IDs and Video Titles and turn them into a collection of strongly typed YouTube video objects, allowing you to get much more detailed info on the video.

For example such as thumbnails, view counts, likes, tags and more.

