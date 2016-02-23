(function (app) {
  'use strict';

  app.controller('sessionSummaryCtrl', sessionSummaryCtrl);

  sessionSummaryCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', '$timeout'];

    function sessionSummaryCtrl($scope, $location, $routeParams, apiService, notificationService, $timeout) {
      $scope.pageClass = 'page-session-summary';
      $scope.session = {};
      $scope.sessionData = {};
      $scope.chartConfig = {};
      $scope.loadingSummary = true;
      $scope.loadingGraph = true;
      $scope.isReadOnly = false;
      $scope.athleteId = $routeParams.athleteId;
      $scope.sessionId = $routeParams.sessionId;

    function loadSessionSummary() {
      $scope.loadingSummary = true;
      apiService.get('/api/sessions/summary/' + $scope.sessionId, null,
      loadSessionSummaryCompleted,
      loadSessionSummaryFailed);
    }

    function loadSessionSummaryCompleted(result) {
      $scope.session = result.data;
      $timeout(function () {
        $scope.loadingSummary = false;
      }, 1280);
    }

    function loadSessionSummaryFailed() {
      notificationService.displayError("Failed to load session summary");
    }

    $scope.distance = {
      measurement: " - " // value before radio button selected
    };

    function loadSessionData() {
      apiService.get('/api/sessions/data/' + $scope.sessionId, null,
      loadSessionDataCompleted,
      loadSessionDataFailed);
    }

    function loadSessionDataCompleted(result) {
      $scope.sessionData = result.data;
     
      $timeout(function () {
        initialiseGraph(); // configure the graph
        drawGraph();
      }, 1200);
    }

    function loadSessionDataFailed() {
      notificationService.displayError("Failed to load session data");
    }

    function collectionOfObjectsToArray(object, attributeToPlot) { // pass in the object to convert to array, and attribute to plot
      var array = [];
      var counter = 0; // start plotting points at 0

      object.forEach(function (object) { // loop through each json object and convert to array
        var plotPoint = object[attributeToPlot]; // plot the specified atttibute in object
        var data = [counter, plotPoint]; // add interval and plot point
        array.push(data);
        counter += $scope.sessionData.Interval; // plot each point at the correct interval
      });
      return array; // array to be passed to $scope.chartConfig.series
    }

    function initialiseGraph() {
      $scope.chartConfig = {
        options: {
          chart: {
            type: 'line',
            zoomType: 'x'
          },
          yAxis: {
            title: {
              text: ''
            }
          },
          xAxis: {
            title: {
              text: 'Time (seconds)'
            }
          }
        },
        series: [],
        title: {
          text: 'Session Summary'
        },
        xAxis: { currentMin: 0, currentMax: $scope.sessionData.XAxisScale, minRange: 1, minTickInterval: $scope.sessionData.Interval }, // currentMax - default x width, minTickInterval - interval of x axis
        yAxis: { currentMin: 0, currentMax: $scope.sessionData.YAxisScale, minRange: 1 }, // currentMax - y axis
        loading: true // display loading message before chart drawn
      }
    }

    function addSeries(array, name, colour) { // add line (series) by passing array, label and line colour
      $scope.chartConfig.series.push({ // can also pass lineWidth: 10
        data: array,
        name: name,
        color: colour
      });
    }
    
    function drawGraph() { // draws the lines and labels
      var heartRates = collectionOfObjectsToArray($scope.sessionData.HeartRates, "HeartRate"); // convert json response objects to array
      var speeds = collectionOfObjectsToArray($scope.sessionData.Speeds, "Speed");
      var altitudes = collectionOfObjectsToArray($scope.sessionData.Altitudes, "Altitude");
      var powers = collectionOfObjectsToArray($scope.sessionData.Powers, "Power");
      var cadences = collectionOfObjectsToArray($scope.sessionData.Cadences, "Cadence");

      addSeries(heartRates, "Heart Rate", "#DC143C"); // plot points on the graph
      addSeries(speeds, "Speed", "#40e0d0");
      addSeries(altitudes, "Altitude", "#f58902");
      addSeries(powers, "Power", "#a500ff");
      addSeries(cadences, "Cadence", "#00008B");

      $scope.loadingGraph = false;
      $scope.chartConfig.loading = false; // disable loading text
    }

    loadSessionSummary();
    loadSessionData();
  }

})(angular.module('cycleAnalysis'));