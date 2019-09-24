module application.searchresultApp {
    'use strict';

    class searchresultController {
        CenterID: string;
        public _getResult: CheckResultVM;
        static $inject = ['$scope', 'application.searchresultApp.searchresultService'];
        constructor(private $scope, private searchresultSvc: application.searchresultApp.searchresultService) {
            this.CenterID =application.mainApp.mainController.centerID
        }
        public searchResult(pid: string) {
            this.searchresultSvc.GetResult(pid).then(re=> {
                this._getResult = re;
            });
        }
        public goPrintSheet(pid: string, sheetid: string) {
            window.open('print.html#!/testresult/' + pid + '/' + sheetid, '_blank');
        }

    }
    class PrintResultController {
        public _getTestResult: shared.Result;
        public logoUrl;
        public ExamPicUrl;
        public SheetId;
        static $inject = ['$scope', 'application.shared.sharedService', '$stateParams', 'appConfig',];
        constructor(private $scope, private sharedSvc: application.shared.sharedService, private $stateParams, appConfig: IAppConfig) {
            this.logoUrl = appConfig.LogoUrl;
            this.ExamPicUrl = appConfig.ExamPicUrl;
            this.SheetId = this.$stateParams.sheetid;
            this.sharedSvc.GetTestResult(this.$stateParams.pid, this.$stateParams.sheetid).then(re => {
                this._getTestResult = re;
                if (this._getTestResult.Status == 'PASS')
                { this._getTestResult.Status = 'ผ่าน' }
                else if (this._getTestResult.Status == 'FAIL')
                { this._getTestResult.Status = 'ไม่ผ่าน' }

            })
        }
    }
    angular
        .module('application.searchresultApp')
        .controller('application.searchresultApp.PrintResultController', PrintResultController)
        .controller('application.searchresultApp.searchresultController', searchresultController);
}