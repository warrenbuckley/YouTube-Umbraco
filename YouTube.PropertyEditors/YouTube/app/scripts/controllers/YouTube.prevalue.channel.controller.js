angular.module("umbraco").controller("YouTube.prevalue.channel.controller", function ($scope, YouTubeResource, notificationsService) {

    

    //Set to be default empty object or value saved if we have it
    $scope.model.value = $scope.model.value ? $scope.model.value : null;

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
            console.log("Items length", response.data.items.length);


            //Only do this is we have a result back from the API
            if(response.data.items.length > 0){
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
            }
            else {
                //Fire a notification - saying user can not be found
                 notificationsService.error("YouTube User Lookup","The channel/user '" + username + "' could not be found on YouTube");

                 //Set the value to be empty
                 $scope.model.value = null;
            }

        });     
        
    };

});