(function (app) {
  'use strict';

  app.controller('addSessionCtrl', addSessionCtrl);

  addSessionCtrl.$inject = ['$scope', '$modalInstance', '$location', 'apiService', 'notificationService'];

  function addSessionCtrl($scope, $modalInstance, $location, apiService, notificationService) {
    $scope.Title = $scope.athlete.Title;
    $scope.session = {};
    $scope.session.AthleteId = $scope.athlete.Id;
    $scope.addSession = addSession;
    $scope.cancel = cancel;
    $scope.isEnabled = false;
    $scope.checkFormIsValid = checkFormIsValid;

    function addSession() {
      apiService.post('/api/sessions/add', $scope.session,
      addSessionSucceeded,
      addSessionFailed);
    }

    function addSessionSucceeded() {
      notificationService.displaySuccess('Session added successfully');
      $modalInstance.close();
    }

    function addSessionFailed() {
      notificationService.displayError("Adding session failed");
    }

    function cancel() {
      $scope.session = null;
      $scope.isEnabled = false;
      $modalInstance.dismiss();
    }

    function checkFormIsValid() {
      if ($scope.session.HRMFile && $scope.session.Title) {
        $scope.isEnabled = true;
      }
      else {
        $scope.isEnabled = false;
      }
    }

  }

})(angular.module('cycleAnalysis'));