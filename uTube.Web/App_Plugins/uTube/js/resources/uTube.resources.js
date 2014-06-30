angular.module('umbraco.resources').factory('uTubeResource', function ($q, $http) {

    //Base API URL
    var apiUrl = Umbraco.Sys.ServerVariables["uTube"]["ApiUrl"];

    //the factory object returned
    return {

        getChannelVideos: function (channelId, pageToken) {
            return $http.post(apiUrl + "VideosForChannel", {pageToken: pageToken, channelId: channelId, orderBy: "date"});
        }

    };
});