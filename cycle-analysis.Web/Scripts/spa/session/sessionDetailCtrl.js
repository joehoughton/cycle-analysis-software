(function (app) {
  'use strict';

  app.controller('sessionDetailCtrl', sessionDetailCtrl);

  sessionDetailCtrl.$inject = ['$scope', '$modalInstance', '$location', '$routeParams', 'apiService', 'notificationService', 'selectedSessionId'];

  function sessionDetailCtrl($scope, $modalInstance, $location, $routeParams, apiService, notificationService, selectedSessionId) {
    $scope.session = {};
    $scope.loadingSessions = true;
    $scope.isReadOnly = false;
    $scope.sessionId = selectedSessionId;
    $scope.clearSearch = clearSearch;
    $scope.cancel = cancel;

    function loadSession() {
      $scope.loadingSessions = true;
      apiService.get('/api/sessions/details/' + $scope.sessionId, null,
      sessionLoadCompleted,
      sessionLoadFailed);
    }

    function sessionLoadCompleted(result) {
      $scope.session = result.data;
      $scope.loadingSessions = false;
    }

    function sessionLoadFailed() {
      notificationService.displayError("Failed to load session");
    }

    function clearSearch() {
      $scope.filterSessions = '';
    }

    function cancel() {
      $modalInstance.dismiss();
    }

    loadSession();
  }

})(angular.module('cycleAnalysis'));
