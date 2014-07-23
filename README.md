YouTube for Umbraco (V7+)
=====
This is a project for adding YouTube videos to your Umbraco site.
Currently it only consists of a single property editor that allows you to easily pick videos from a specific YouTube channel.

##YouTube Channel Picker
Currently this package consists of the Google .NET C# Library for YouTube API's along with a V7 Angular Property Editor for selecting video/s from a YouTube channel.

###Settings
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

###Thanks
Many thanks to Jeavon Leopold who helped collab on this project and creating the C# PropertyValue Convertor, which enables us to turn the simple JSON object we store in Umbraco of YouTube IDs and Video Titles and turn them into a collection of strongly typed YouTube video objects, allowing you to get much more detailed info on the video.

For example such as thumbnails, view counts, likes, tags and more.

