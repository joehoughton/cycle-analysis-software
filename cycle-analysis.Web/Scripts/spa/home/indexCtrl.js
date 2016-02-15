(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$timeout'];

    function indexCtrl($scope, apiService, notificationService, $timeout) {
      $scope.pageClass = 'page-home';
      $scope.isReadOnly = true;
      $scope.loadingDiagrams = true;

      $timeout(function () { $scope.loadingDiagrams = false }, 1500); // load images after 1.5 second timeout
    }
    
})(angular.module('cycleAnalysis'));
