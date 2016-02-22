(function (app) {
  'use strict';

  app.controller('sessionSummaryCtrl', sessionSummaryCtrl);

  sessionSummaryCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService'];

  function sessionSummaryCtrl($scope, $location, $routeParams, apiService, notificationService) {
    $scope.pageClass = 'page-session-summary';
    $scope.session = {};
    $scope.loadingSummary = true;
    $scope.isReadOnly = false;
    $scope.athleteId = $routeParams.athleteId;
    $scope.sessionId = $routeParams.sessionId;

    function loadsession() {
      $scope.loadingSummary = true;
      apiService.get('/api/sessions/summary/' + $scope.sessionId, null,
      sessionLoadCompleted,
      sessionLoadFailed);
    }

    function sessionLoadCompleted(result) {
      $scope.session = result.data;
      $scope.loadingSummary = false;
    }

    function sessionLoadFailed(response) {
      notificationService.displayError("Failed to load session summary");
    }
    // value before radio button selected
    $scope.distance = {
      measurement: " - "
    };

    loadsession();
  }

})(angular.module('cycleAnalysis'));
