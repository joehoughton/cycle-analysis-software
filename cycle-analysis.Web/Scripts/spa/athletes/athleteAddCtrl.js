(function (app) {
  'use strict';

  app.controller('athleteAddCtrl', athleteAddCtrl);

  athleteAddCtrl.$inject = ['$scope', '$rootScope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService'];

  function athleteAddCtrl($scope, $rootScope, $location, $routeParams, apiService, notificationService, fileUploadService) {

    $scope.pageClass = 'page-athletes';
    $scope.athlete = {};

    $scope.isReadOnly = false;
    $scope.addAthlete = addAthlete;
    $scope.prepareFiles = prepareFiles;
    $scope.currentUser = $rootScope.repository.loggedUser;

    var athleteImage = null;

    function addAthlete() {
      addAthleteModel();
    }

    function addAthleteModel() {
      apiService.post('/api/athletes/add', $scope.athlete,
        addAthleteSucceded,
        addAthleteFailed);
    }

    function prepareFiles($files) {
      athleteImage = $files;
    }

    function addAthleteSucceded(response) {
      $scope.athlete = response.data;

      notificationService.displaySuccess($scope.athlete.FirstName + $scope.athlete.LastName + ' has been registered');
      notificationService.displaySuccess('Check ' + $scope.athlete.UniqueKey + ' for reference number');

      if (athleteImage) {
        fileUploadService.uploadImage(athleteImage, $scope.athlete.Id, redirectToEdit);
      }
      else
        redirectToEdit();
    }

    function addAthleteFailed(response) {
      notificationService.displayError(response.statusText);
    }

    function redirectToEdit() {
      $location.url('athletes/edit/' + $scope.athlete.Id);
    }

  }

})(angular.module('cycleAnalysis'));
