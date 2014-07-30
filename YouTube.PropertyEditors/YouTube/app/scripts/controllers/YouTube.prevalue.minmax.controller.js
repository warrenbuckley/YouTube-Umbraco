angular.module("umbraco").controller("YouTube.prevalue.minmax.controller", function ($scope, YouTubeResource, notificationsService, angularHelper, serverValidationManager) {
   
    console.log("Scope Model on init", $scope.model);


    //Set to be default empty object or value saved if we have it
    $scope.model.value = $scope.model.value ? $scope.model.value : null;

    if($scope.model.value){
        //Have a value - so lets assume our JSON object is all good
        //Debug message
        console.log("Scope Model Value on init", $scope.model.value);
    }


    //This fires when a checkbox is clicked/toggled
    $scope.clearValue = function(fieldToClear) {

        //TODO: When checkbox clicked check if it is false/not selected
        //If false (unchecked) then remove value on fieldToClear
        console.log("Scope Model Value", $scope.model.value);

        console.log("Item that was clicked? (this)", this);

        //Field may be pristine and be undefinied/null
        //Field to Clear is not the name of the field but the value :-(
        console.log("Function clearValue", fieldToClear);

        console.log("Field To Clear before null", $scope.model.fieldToClear);

        $scope.model.fieldToClear = null;

        console.log("Field To Clear after null", $scope.model.fieldToClear);

    };

});