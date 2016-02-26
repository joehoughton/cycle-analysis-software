(function (app) {
  'use strict';

  app.controller('athleteEditCtrl', athleteEditCtrl);

  athleteEditCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService'];

  function athleteEditCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService) {
    $scope.pageClass = 'page-athletes';
    $scope.athlete = {};
    $scope.loadingAthlete = true;
    $scope.isReadOnly = false;
    $scope.UpdateAthlete = updateAthlete;
    $scope.prepareFiles = prepareFiles;
    $scope.athleteImage = null;

    function loadAthlete() {
      $scope.loadingAthlete = true;

      apiService.get('/api/athletes/details/' + $routeParams.id, null,
      athleteLoadCompleted,
      athleteLoadFailed);
    }

    function athleteLoadCompleted(result) {
      $scope.athlete = result.data;
      $scope.loadingAthlete = false;
    }

    function athleteLoadFailed(response) {
      notificationService.displayError(response.data);
    }

    function updateAthlete() {
      if ($scope.athleteImage) {
        fileUploadService.uploadImage($scope.athleteImage, $scope.athlete.Id, updateAthleteModelImage);
      }
      else
        updateAthleteModel();
    }

    function updateAthleteModelImage(result) {
      $scope.athlete.Image = result.Image;
      updateAthleteModel();
      $scope.athleteImage = null;
    }

    function updateAthleteModel() {
      apiService.post('/api/athletes/update', $scope.athlete,
      updateAthleteSucceded,
      updateAthleteFailed);
    }

    function prepareFiles($files) {
      $scope.athleteImage = $files;
    }

    function updateAthleteSucceded(response) {
      notificationService.displaySuccess($scope.athlete.FirstName +' '+ $scope.athlete.LastName + ' has been updated');
      $scope.athlete = response.data;
      $scope.athleteImage = null;
    }

    function updateAthleteFailed() {
      notificationService.displayError("Failed to update athlete, try again");
    }

    loadAthlete();
  }

})(angular.module('cycleAnalysis'));
