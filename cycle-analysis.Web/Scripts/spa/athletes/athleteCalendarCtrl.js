(function (app) {
  'use strict';

  app.controller('athleteCalendarCtrl', athleteCalendarCtrl);

  athleteCalendarCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', '$compile', 'uiCalendarConfig'];

  function athleteCalendarCtrl($scope, $location, $routeParams, apiService, notificationService, $compile, uiCalendarConfig) {
    $scope.pageClass = 'page-athletes-calendar';
    $scope.athlete = { Id: $routeParams.id };
    $scope.loadingCalendar = true;
    $scope.isReadOnly = false;
    $scope.eventSources = []; // array of event objects to be added here
    $scope.events = { // to be passed into $scope.eventSources
      color: '#b7d331',
      textColor: 'black',
      events: [] // calendar events to be added here
    };

    function loadSessions() {
      $scope.loadingAthlete = true;
      apiService.get('/api/sessions/athlete/' + $routeParams.id + '/calendar', null,
      loadSessionsCompleted,
      loadSessionsFailed);
    }

    function loadSessionsCompleted(result) {
      $scope.sessions = result.data;
      notificationService.displayInfo(Object.keys($scope.sessions).length + " sessions recorded"); // display how many results found
      $scope.loadingCalendar = false; // remove loading message
     
      $scope.eventSources.slice(0, $scope.events.length); // remove existing events
      var events = convertSessionObjectsToArrayOfEvents($scope.sessions); // convert session objects to calendar events
      $scope.eventSources.push(events); // add events to calendar
    }

    function loadSessionsFailed() {
      notificationService.displayError("Failed to find calender events");
    }

    function convertSessionObjectsToArrayOfEvents(eventObjects) {
      var events = [];
      eventObjects.forEach(function (object) { // loop through each json object
        var startDateString = object['StartDate'];
        var startDate = new Date(Date.parse(startDateString)); // convert string to date 
        var startYear = startDate.getFullYear();
        var startMonth = startDate.getMonth();
        var startDay = startDate.getUTCDate();
        var startHour = startDate.getHours();
        var startMinutes = startDate.getMinutes();

        var endDateString = object['EndDate'];
        var endDate = new Date(Date.parse(endDateString)); // convert string to date 
        var endYear = endDate.getFullYear();
        var endMonth = endDate.getMonth();
        var endDay = endDate.getUTCDate();
        var endHour = endDate.getHours();
        var endMinutes = endDate.getMinutes();
        // ToDo: Add Title to sessions
        var event = { title: "Cool Session", start: new Date(startYear, startMonth, startDay, startHour, startMinutes), end: new Date(endYear, endMonth, endDay, endHour, endMinutes), allDay: false, sessionId: object['Id'] }; // create calendar event
        events.push(event); // add calendar event to array
      });

      return events;
    }

    // change view
    $scope.changeView = function (view, calendar) {
      uiCalendarConfig.calendars[calendar].fullCalendar('changeView', view);
    };

    // change view
    $scope.renderCalender = function (calendar) {
      if (uiCalendarConfig.calendars[calendar]) {
        uiCalendarConfig.calendars[calendar].fullCalendar('render');
      }
    };

    // render tooltip
    $scope.eventRender = function (event, element, view) {
      element.attr({
        'tooltip': event.title,
        'tooltip-append-to-body': true
      });
      $compile(element)($scope);
    };
    
    // event click action
    $scope.eventClickAction = function (selectedEvent, jsEvent, view) {
      var selectedSessionId = selectedEvent.sessionId; // get sessionId from selected event
      $location.url('athletes/' + $scope.athlete.Id + '/session/summary/' + selectedSessionId); // redirect to summary view
    };

    // config object
    $scope.uiConfig = {
      calendar: {
        height: 450,
        editable: false, // disable dragging events
        header: {
          left: 'title',
          center: '',
          right: 'today prev,next'
        },
        eventClick: $scope.eventClickAction,
        eventDrop: null,
        eventResize: null,
        eventRender: $scope.eventRender
      }
    }

    loadSessions(); // get sessions for calendar
  }

})(angular.module('cycleAnalysis'));