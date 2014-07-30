angular.module('umbraco.resources')
.factory('YouTubeResource',
['$http',
function ($http) {

    //Base API URL
    var apiUrl = Umbraco.Sys.ServerVariables["YouTube"]["ApiUrl"];

    //the factory object returned
    return {

        getChannelVideos: function (channelId, orderBy, searchQuery, pageToken) {
            return $http.post(apiUrl + "VideosForChannel", { pageToken: pageToken, channelId: channelId, searchQuery: searchQuery, orderBy: orderBy });
        },

        queryUsernameForChannel: function(usernameToQuery) {
        	return $http.get(apiUrl + "ChannelFromUsername?username=" + usernameToQuery);
        }

    };
}]);
