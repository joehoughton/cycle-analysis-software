(function (app) {
  'use strict';

  app.controller('athletesCtrl', athletesCtrl);

  athletesCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

  function athletesCtrl($scope, apiService, notificationService) {
    $scope.pageClass = 'page-athletes';
    $scope.loadingAthletes = true;
    $scope.page = 0;
    $scope.pagesCount = 0;

    $scope.Athletes = [];

    $scope.search = search;
    $scope.clearSearch = clearSearch;

    function search(page) {
      page = page || 0;

      $scope.loadingAthletes = true;

      var config = {
        params: {
          page: page,
          pageSize: 6,
          filter: $scope.filterAthletes
        }
      };

      apiService.get('/api/athletes/', config,
      athletesLoadCompleted,
      athletesLoadFailed);
    }

    function athletesLoadCompleted(result) {
      $scope.Athletes = result.data.Items;
      $scope.page = result.data.Page;
      $scope.pagesCount = result.data.TotalPages;
      $scope.totalCount = result.data.TotalCount;
      $scope.loadingAthletes = false;

      if ($scope.filterAthletes && $scope.filterAthletes.length) {
        notificationService.displayInfo(result.data.Items.length + ' athletes found');
      }

    }

    function athletesLoadFailed(response) {
      notificationService.displayError(response.data);
    }

    function clearSearch() {
      $scope.filterAthletes = '';
      search();
    }

    $scope.search();
  }

})(angular.module('cycleAnalysis'));
