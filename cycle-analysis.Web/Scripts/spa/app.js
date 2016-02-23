(function () {
    'use strict';

  angular.module('cycleAnalysis', ['common.core', 'common.ui', 'highcharts-ng'])
        .config(config)
        .run(run);

    config.$inject = ['$routeProvider'];
    function config($routeProvider) {
      $routeProvider
        .when("/", {
          templateUrl: "scripts/spa/home/index.html",
          controller: "indexCtrl"
        })
        .when("/login", {
          templateUrl: "scripts/spa/account/login.html",
          controller: "loginCtrl"
        })
        .when("/register", {
          templateUrl: "scripts/spa/account/register.html",
          controller: "registerCtrl"
        })
        .when("/athletes", {
          templateUrl: "scripts/spa/athletes/athletes.html",
          controller: "athletesCtrl"
        })
        .when("/athletes/add", {
          templateUrl: "scripts/spa/athletes/add.html",
          controller: "athleteAddCtrl",
          resolve: { isAuthenticated: isAuthenticated }
        })
        .when("/athletes/:id", {
          templateUrl: "scripts/spa/athletes/details.html",
          controller: "athleteDetailsCtrl",
          resolve: { isAuthenticated: isAuthenticated }
        })
        .when("/athletes/edit/:id", {
          templateUrl: "scripts/spa/athletes/edit.html",
          controller: "athleteEditCtrl"
        })
        .when("/athletes/:athleteId/session/summary/:sessionId", {
          templateUrl: "scripts/spa/session/summary.html",
          controller: "sessionSummaryCtrl"
        });
    }

    run.$inject = ['$rootScope', '$location', '$cookieStore', '$http'];

    function run($rootScope, $location, $cookieStore, $http) {
        // handle page refreshes
        $rootScope.repository = $cookieStore.get('repository') || {};
        if ($rootScope.repository.loggedUser) {
            $http.defaults.headers.common['Authorization'] = $rootScope.repository.loggedUser.authdata;
        }

        $(document).ready(function () {
            $(".fancybox").fancybox({
                openEffect: 'none',
                closeEffect: 'none'
            });

            $('.fancybox-media').fancybox({
                openEffect: 'none',
                closeEffect: 'none',
                helpers: {
                    media: {}
                }
            });

            $('[data-toggle=offcanvas]').click(function () {
                $('.row-offcanvas').toggleClass('active');
            });
        });
    }

    isAuthenticated.$inject = ['membershipService', '$rootScope', '$location'];

    function isAuthenticated(membershipService, $rootScope, $location) {
        if (!membershipService.isUserLoggedIn()) {
            $rootScope.previousState = $location.path();
            $location.path('/login');
        }
    }

})();