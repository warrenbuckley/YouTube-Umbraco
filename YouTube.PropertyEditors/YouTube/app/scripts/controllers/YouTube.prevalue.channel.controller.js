angular.module("umbraco").controller("YouTube.prevalue.channel.controller", function ($scope, YouTubeResource, angularHelper, serverValidationManager) {


    //Set to be default empty object or value saved if we have it
    $scope.model.value = $scope.model.value ? $scope.model.value : null;

    if($scope.model.value){
        //Have a value - so lets assume our JSON object is all good
        //Debug message
        console.log("Scope Model Value on init", $scope.model.value);
    }


    //This is run when the button is clicked
    $scope.queryChannel = function(username) {

        //Default flag for validity
        var isThisValid = false;

        //Query this via our resource
        YouTubeResource.queryUsernameForChannel(username).then(function(response) {

            //Only do this is we have a result back from the API
            if(response.data.items.length > 0){
                //Data we are interested is in
                //response.data.items[0]
                var channel = response.data.items[0];


                //Create new JSON object as we don't need full object from Google's API response
                var newChannelObject = {
                    "username": username,
                    "channelId": channel.id,
                    "title": channel.snippet.title,
                    "description": channel.snippet.description,
                    "thumbnails": channel.snippet.thumbnails,
                    "statistics": channel.statistics
                };

                //Set the value to be our new JSON object
                $scope.model.value.youtube = newChannelObject;

                //Set our flag to true
                isThisValid = true;
            }
            else {
                
                //Set the value to be empty
                $scope.model.value.youtube = null;

                //Ensure flag is set to false
                isThisValid = false;
            }


            //Call our isUsernameValid function
            //With our bool if form is valid or not
            //Will show or hide valdiation if needed
            isUsernameValid(isThisValid);
        });

    };

    //Watch our $scope.model.value
    //When data changes - revalidate...
    $scope.$watch(function() {
        return $scope.model.value;
    }, function(newVal, oldVal) {

        console.log("Old Value", oldVal);
        console.log("New Value", newVal);

        //Call our validation method
        isThisValid();

    }, true);

    function isThisValid(){
        console.log("Is this valid?");

        //Always default to is valid & set to false when we know not valid
        var isValid = true;

        //Check we have a username object
        //May be that a username was querried that does not exist
        if($scope.model.value.youtube !== null && $scope.model.value.youtube.username !== null){
            
            if($scope.model.value.querriedUsername !== $scope.model.value.youtube.username){
                //The username in the textbox is not the same as we have saved in YouTube
                //So means user has typed in a new name but not pressed the query button

                //User has not rechecked new name in textbox
                isValid = false;
            }
        }

        //Run validation to show or remove existing errors
        hasUserRechecked(isValid);

    }

    function isUsernameValid(isValid) {
        //Get the form with Umbraco's helper of this $scope
        //The form is wrapped just around this single prevalue editor
        var form = angularHelper.getCurrentForm($scope);

        //Inside the form we have our input field with the name/id of username
        //Set this field to be valid or invalid based on our flag
        form.username.$setValidity('YouTubeChannel', isValid);


        if(!isValid){
            //Property Alias, Field name (ID/name of text box), Error Message
            serverValidationManager.addPropertyError($scope.model.alias, "username", "The channel/user could not be found on YouTube");
        }
        else {
            //Property Alias, Field name (ID/name of text box)
            serverValidationManager.removePropertyError($scope.model.alias, "username");
        }
    }

    function hasUserRechecked(isValid) {
        //Get the form with Umbraco's helper of this $scope
        //The form is wrapped just around this single prevalue editor
        var form = angularHelper.getCurrentForm($scope);

        //Inside the form we have our input field with the name/id of username
        //Set this field to be valid or invalid based on our flag
        form.username.$setValidity('notchecked', isValid);
    }

});
