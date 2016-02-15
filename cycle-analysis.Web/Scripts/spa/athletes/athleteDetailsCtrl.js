(function (app) {
  'use strict';

  app.controller('athleteDetailsCtrl', athleteDetailsCtrl);

  athleteDetailsCtrl.$inject = ['$scope', '$location', '$routeParams', '$modal', 'apiService', 'notificationService'];

  function athleteDetailsCtrl($scope, $location, $routeParams, $modal, apiService, notificationService) {
    $scope.pageClass = 'page-athletes';
    $scope.athlete = {};
    $scope.loadingAthlete = true;
    $scope.loadingSessions = true;
    $scope.isReadOnly = true;
    $scope.openSessionDialog = openSessionDialog;
    $scope.sessionHistory = [];
    $scope.clearSearch = clearSearch;
    $scope.viewSession = viewSession;

    function loadAthlete() {
      $scope.loadingAthlete = true;
      apiService.get('/api/athletes/details/' + $routeParams.id, null,
      athleteLoadCompleted,
      athleteLoadFailed);
    }

    function loadSessions() {
      $scope.loadingSessions = true;
      apiService.get('/api/sessions/' + $routeParams.id + '/sessionhistory', null,
      sessionHistoryLoadCompleted,
      sessionHistoryLoadFailed);
    }

    function loadAthleteDetails() {
      loadAthlete();
      loadSessions();
    }

    function clearSearch() {
      $scope.filterSessions = '';
    }

    function athleteLoadCompleted(result) {
      $scope.athlete = result.data;
      $scope.loadingAthlete = false;
    }

    function athleteLoadFailed(response) {
      notificationService.displayError(response.data);
    }

    function sessionHistoryLoadCompleted(result) {
      $scope.sessionHistory = result.data;
      $scope.loadingSessions = false;
    }

    function sessionHistoryLoadFailed(response) {
      notificationService.displayError(response);
    }

    // view session modal
    function viewSession(selectedSessionId) {
      $modal.open({
        templateUrl: 'scripts/spa/session/sessionDetailModal.html',
        controller: 'sessionDetailCtrl',
        scope: $scope,
        resolve: {
          selectedSessionId: function () { return selectedSessionId; }
        },
        windowClass: 'app-modal-window'
      }).result.then(function ($scope) {
        loadSessionDetails();
      }, function () {
      });
    }

    // add session modal
    function openSessionDialog() {
      $modal.open({
        templateUrl: 'scripts/spa/session/addSessionModal.html',
        controller: 'addSessionCtrl',
        scope: $scope,
        windowClass: 'app-modal-window'
      }).result.then(function ($scope) {
        loadAthleteDetails();
      }, function () {
      });
    }

    loadAthleteDetails();
  }

})(angular.module('cycleAnalysis'));
