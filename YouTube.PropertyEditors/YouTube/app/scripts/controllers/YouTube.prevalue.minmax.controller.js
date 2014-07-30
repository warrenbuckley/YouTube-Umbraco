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
    $scope.clearMinValue = function() {

        //If the enabled min is not checked/true
        if(!$scope.model.value.enableMin){
            //Then clear out the value in the textbox for min
            $scope.model.value.minValue = null;
        }
    };

    //This fires when a checkbox is clicked/toggled
    $scope.clearMaxValue = function() {

        //If the enabled max is not checked/true
        if(!$scope.model.value.enableMax){
            //Then clear out the value in the textbox for max
            $scope.model.value.maxValue = null;
        }
    };

});