angular.module("umbraco").controller("YouTube.prevalue.minmax.controller", function ($scope) {

    //Default object if no value saved
    var defaultObject = {
        "enableMin": false,
        "minValue": null,
        "enableMax": false,
        "maxValue": null
    };

    //Set to be default empty object or value saved if we have it
    $scope.model.value = $scope.model.value ? $scope.model.value : defaultObject;

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