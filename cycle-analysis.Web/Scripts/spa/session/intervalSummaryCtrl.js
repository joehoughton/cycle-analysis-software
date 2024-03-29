﻿(function(app) {
  'use strict';

  app.controller('intervalSummaryCtrl', intervalSummaryCtrl);

  intervalSummaryCtrl.$inject = ['$scope', '$modalInstance', '$location', '$routeParams', 'apiService', 'notificationService'];

  function intervalSummaryCtrl($scope, $modalInstance, $location, $routeParams, apiService, notificationService) {
    $scope.intervalSummary = {};
    $scope.loadingIntervalSummary = true;
    $scope.sessionId = $scope.$parent.sessionId;
    $scope.units = $scope.$parent.units;
    $scope.selectedUnit = $scope.$parent.selectedUnit;
    $scope.selectedIntervalStart = $scope.$parent.selectedIntervalStart;
    $scope.selectedIntervalFinish = $scope.$parent.selectedIntervalFinish;
    $scope.selectedIntervalName = $scope.$parent.selectedIntervalName;
    $scope.loadIntervalSummary = loadIntervalSummary;
    $scope.cancel = cancel;

    function loadIntervalSummary(selected) {
      $scope.loadingIntervalSummary = true;

      if (undefined != selected) {
        $scope.selectedUnit = $scope.units[selected.index];
      }
      if (undefined != $scope.selectedUnit) {
        $scope.sessionDataSubsetDto.Unit = $scope.selectedUnit.index;
      }

      var intervalSummary = { 'SessionId': $scope.sessionId, 'Unit': $scope.selectedUnit.index }

      apiService.post('/api/sessions/intervalsummary', intervalSummary,
        loadIntervalSummaryCompleted,
        loadIntervalSummaryFailed);
    }

    function loadIntervalSummaryCompleted(result) {
      $scope.workoutSummary = result.data[0];
      $scope.restSummary = result.data[1];
      $scope.loadingIntervalSummary = false;
      checkNormalizedPowerForNullValue();
    }

    function checkNormalizedPowerForNullValue() {
      if ($scope.intervalSummary.NormalizedPower === 0) {
        notificationService.displayWarning("IF, NP and TSS require more data");
      }
    }

    function loadIntervalSummaryFailed() {
      notificationService.displayError("Failed to load interval summary");
    }

    function cancel() {
      $modalInstance.dismiss();
    }

    loadIntervalSummary();
  }

})(angular.module('cycleAnalysis'));
