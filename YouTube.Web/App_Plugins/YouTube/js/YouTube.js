angular.module("umbraco").controller("YouTube.channel.controller", function ($scope, YouTubeResource) {

    function debug(message, object){
        //Check we have the console object
        //Some older browsers don't
        if (typeof console === "object") {
            //Now let's check if user set the debug flag on property editor to true
            //In this case a string not a real number of 1 or 0
            var isDebug = $scope.model.config.debug;
            
            if(isDebug === "1"){
                console.log(message, object);
            }
        }
    }

    //Set Has Videos to false - until we get some back from API call
    $scope.hasVideos = false;

    //Debug message
    debug("Scope Model on init", $scope.model);

    //Set to be default empty array or value saved
    $scope.model.value = $scope.model.value ? $scope.model.value : [];


    //Try & get videos for grid on Page Load
    YouTubeResource.getChannelVideos($scope.model.config.channel.channelId, $scope.model.config.orderBy, null, null).then(function(response) {

        //Debug message
        debug("Response Data on init", response.data);

        //Check we have items back from YouTube
        if (response.data.items.length > 0) {

            //Videos
            $scope.videos = response.data;

            //Now we can show the grid of videos
            $scope.hasVideos = true;
        }

    });

    $scope.toggleVideo = function(video) {

        //Create new JSON object as we don't need full object passed in here        
        var newVideoObject = {
            "id": video.id.videoId,
            "title": video.snippet.title
        };
        
        //See if we can find the item or not in the array
        var tryFindItem = $scope.model.value.map(function (e) { return e.id; }).indexOf(newVideoObject.id);
        
        //Check to add or remove item
        if (tryFindItem !== -1) {
            //Found the item in the array
            //Lets remove it at the index we found it at & remove the single item only
            $scope.model.value.splice(tryFindItem, 1);
        } else {
            //Item does not exist in the array, let's add it
            $scope.model.value.push(newVideoObject);
        }
    };

    $scope.getPagedVideos = function(pagedToken) {
        
        //Check we have a paged token
        //May be at beginning or end of list
        //If so don't do anything
        if (pagedToken == null) {
            return;
        }

        //Call getVideos() with our page token
        this.getVideos(pagedToken);
    };

    $scope.getVideos = function (pagedToken) {
       
        //Set Has Videos to false - until we get some back from API call
        $scope.hasVideos = false;

        //Do new request to API
        YouTubeResource.getChannelVideos($scope.model.config.channel.channelId, $scope.model.config.orderBy, $scope.searchQuery, pagedToken).then(function (response) {

            //Debug message
            debug("Response Data from GetVideos()", response.data);

            //Check we have items back from YouTube
            if (response.data.items.length > 0) {

                //Videos
                $scope.videos = response.data;

                //Now we can show the grid of videos
                $scope.hasVideos = true;
            }

        });
    };


    $scope.removeVideo = function(videoIndex) {
        console.log(videoIndex);

        //Lets remove it at the index we pass in & remove the single item only
        $scope.model.value.splice(videoIndex, 1);
    };

    $scope.isInArray = function (videoId) {
        //See if we can find the item or not in the array
        var tryFindItem = $scope.model.value.map(function (e) { return e.id; }).indexOf(videoId);

        if (tryFindItem !== -1) {
            //Found it in the array
            return true;
        } else {
            //Could not find it in the array - was -1
            return false;
        }
    };
});
angular.module("umbraco").controller("YouTube.prevalue.channel.controller", function ($scope, YouTubeResource) {

    

    //Set to be default empty object or value saved if we have it
    $scope.model.value = $scope.model.value ? $scope.model.value : {};

    if($scope.model.value){
        //Have a value - so lets assume our JSON object is all good
        //Debug message
        console.log("Scope Model Value on init", $scope.model.value);

        //As we have JSON value on init
        //Let's set the textbox to the value of the querried username
        $scope.username = $scope.model.value.querriedUsername;
    }


    $scope.queryChannel = function(username) {

        //Debug info
        console.log("Query Channel Click", username);

        //Query this via our resource
        YouTubeResource.queryUsernameForChannel(username).then(function(response) {
            
            //Debug info
            console.log("Value back from query API", response);

            //Data we are interested is in
            //response.data.items[0]
            var channel = response.data.items[0];
            

            //Create new JSON object as we don't need full object from Google's API response     
            var newChannelObject = {
                "querriedUsername": username,
                "channelId": channel.id,
                "title": channel.snippet.title,
                "description": channel.snippet.description,
                "thumbnails": channel.snippet.thumbnails,
                "statistics": channel.statistics
            };
        
            //Set the value to be our new JSON object
            $scope.model.value = newChannelObject;


        });     
        
    };

});
angular.module('umbraco.resources').factory('YouTubeResource', function ($q, $http) {

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
});