angular.module("umbraco").controller("YouTube.channel.controller", function ($scope, YouTubeResource, angularHelper) {

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
    $scope.hasVideos        = false;
    $scope.notFoundVideos   = false;

    //Debug message
    debug("Scope Model on init", $scope.model);

    debug("Scope Model Config minmax", $scope.model.config.minmax);

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
            $scope.hasVideos        = true;
            $scope.notFoundVideos   = false;
        }
        else {
            //No videos - may be searching & found no results
            $scope.notFoundVideos   = true;
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

        //Check validity of min & max items
        var minValid = isMinValid();
        var maxValid = isMaxValid();

        //Check to add or remove item
        if (tryFindItem !== -1) {
            
            //Found the item in the array

            //Lets remove it at the index we found it at & remove the single item only
            $scope.model.value.splice(tryFindItem, 1);

        }
        else {

            //Adding item to the collection
            //Item does not exist in the array, let's add it & all OK with validation :)
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
                $scope.hasVideos        = true;
                $scope.notFoundVideos   = false;
            }
            else {
                //No videos - may be searching & found no results
                $scope.notFoundVideos   = true;
            }

        });
    };


    $scope.removeVideo = function(videoIndex) {
        debug("Remove video at index",videoIndex);

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


    //Watch our $scope.model.value of items
    //When items get added or removed - validate...
    $scope.$watch(function() {
        return $scope.model.value;
    }, function(newVal, oldVal) {

        debug("Old Value", oldVal);
        debug("New Value", newVal);

        //Call our validation methods
        isMinValid();
        isMaxValid();

    }, true);

    function isMaxValid() {
        var isMaxEnabled = $scope.model.config.minmax.enableMax;

        if(isMaxEnabled){
            //If it's enabled let's check to see if we reached total items
            var maxItems = parseInt($scope.model.config.minmax.maxValue);

            //Get the current form
            var currentForm = angularHelper.getCurrentForm($scope);
            
            if($scope.model.value.length > maxItems){                    

                //The hidden field in the view set its validity
                currentForm.maxerror.$setValidity('youtubemax', false);

                //Not Valid - False
                return false;
            }
            else {
                //The hidden field in the view set its validity
                //It's valid & OK
                currentForm.maxerror.$setValidity('youtubemax', true);

                //It is valid - True
                return true;
            }
        }

        //Flag not enabled to check for Max items, so always valid
        return true;
    }

    function isMinValid() {
        //Is Min Enabled?
        var isMinEnabled = $scope.model.config.minmax.enableMin;

        if(isMinEnabled){
            //If it's enabled let's check to see if we reached total items
            var minItems = parseInt($scope.model.config.minmax.minValue);

            //Get the current form
            var currentForm = angularHelper.getCurrentForm($scope);

            //If number of items we have is less than minItems we want
            if($scope.model.value.length < minItems){

                //The hidden field in the view set its validity
                currentForm.minerror.$setValidity('youtubemin', false);

                //Not Valid - False
                return false;

            }
            else {
                //The hidden field in the view set its validity
                //All is OK (Have more or equal to minimum number)
                currentForm.minerror.$setValidity('youtubemin', true);

                //It is valid - True
                return true;
            }
        }

        //Flag not enabled to check for Min items, so always valid
        return true;
    }

});
