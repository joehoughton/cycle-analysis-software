﻿/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/Vendors/modernizr.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendors").Include(
                "~/Scripts/Vendors/jquery.js",
                "~/Scripts/Vendors/bootstrap.js",
                "~/Scripts/Vendors/toastr.js",
                "~/Scripts/Vendors/jquery.raty.js",
                "~/Scripts/Vendors/respond.src.js",
                "~/Scripts/Vendors/angular.js",
                "~/Scripts/Vendors/angular-route.js",
                "~/Scripts/Vendors/angular-cookies.js",
                "~/Scripts/Vendors/angular-validator.js",
                "~/Scripts/Vendors/angular-base64.js",
                "~/Scripts/Vendors/angular-file-upload.js",
                "~/Scripts/Vendors/angucomplete-alt.min.js",
                "~/Scripts/Vendors/ui-bootstrap-tpls-0.13.1.js",
                "~/Scripts/Vendors/moment.js",
                "~/Scripts/Vendors/underscore.js",
                "~/Scripts/Vendors/raphael.js",
                "~/Scripts/Vendors/morris.js",
                "~/Scripts/Vendors/jquery.fancybox.js",
                "~/Scripts/Vendors/jquery.fancybox-media.js",
                "~/Scripts/Vendors/loading-bar.js",
                "~/Scripts/Vendors/shCore.js",
                "~/Scripts/Vendors/shBrushCSharp.js",
                "~/Scripts/Vendors/highcharts.js",
                "~/Scripts/Vendors/highcharts-ng.js",
                "~/Scripts/Vendors/fullcalendar.js",
                "~/Scripts/Vendors/gcal.js",
                "~/Scripts/Vendors/calendar.js"));

            bundles.Add(new ScriptBundle("~/bundles/spa").Include(
                "~/Scripts/spa/modules/common.core.js",
                "~/Scripts/spa/modules/common.ui.js",
                "~/Scripts/spa/app.js",
                "~/Scripts/spa/services/apiService.js",
                "~/Scripts/spa/services/notificationService.js",
                "~/Scripts/spa/services/membershipService.js",
                "~/Scripts/spa/services/fileUploadService.js",
                "~/Scripts/spa/layout/topBar.directive.js",
                "~/Scripts/spa/layout/sideBar.directive.js",
                "~/Scripts/spa/layout/customPager.directive.js",
                "~/Scripts/spa/directives/textareaResize.directive.js",
                "~/Scripts/spa/account/loginCtrl.js",
                "~/Scripts/spa/account/registerCtrl.js",
                "~/Scripts/spa/home/rootCtrl.js",
                "~/Scripts/spa/home/indexCtrl.js",
                "~/Scripts/spa/athletes/athletesCtrl.js",
                "~/Scripts/spa/athletes/athleteAddCtrl.js",
                "~/Scripts/spa/athletes/athleteDetailsCtrl.js",
                "~/Scripts/spa/athletes/athleteEditCtrl.js",
                "~/Scripts/spa/athletes/athleteCalendarCtrl.js",
                "~/Scripts/spa/session/sessionCtrl.js",
                "~/Scripts/spa/session/addSessionCtrl.js",
                "~/Scripts/spa/session/sessionSummaryCtrl.js",
                "~/Scripts/spa/session/sessionDetailCtrl.js",
                "~/Scripts/spa/session/singleIntervalSummaryCtrl.js",
                "~/Scripts/spa/session/intervalSummaryCtrl.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/content/css/site.css",
                "~/content/css/bootstrap.css",
                "~/content/css/bootstrap-theme.css",
                "~/content/css/font-awesome.css",
                "~/content/css/morris.css",
                "~/content/css/toastr.css",
                "~/content/css/jquery.fancybox.css",
                "~/content/css/loading-bar.css",
                "~/content/css/shCore.css",
                "~/content/css/shCoreDefault.css",
                "~/content/css/shThemeDefault.css",
                "~/content/css/normalize.css",
                "~/content/css/fullcalendar.css",
                "~/content/css/calendar.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}