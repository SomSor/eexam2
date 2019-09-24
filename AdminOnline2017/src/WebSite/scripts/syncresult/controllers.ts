module application.syncResultApp {
    'use strict';
    class syncResultController {
        public _syncResult: shared.TestRegistration[];
        public _passResult: shared.TestRegistration[];
        public _failResult: shared.TestRegistration[];
        public prepareData: boolean;
        public failToLoad: boolean;
        public tabStatus: string;
        static $inject = ['$scope', 'application.syncResultApp.syncResultService'];
        constructor(private $scope, private syncResultSvc: application.syncResultApp.syncResultService) {
            this.syncResultSvc.syncresultListResultTestRegis().then(re => {
                this._syncResult = re
                this._passResult = this._syncResult.filter(pass=> pass.Status == 'PASS');
                this._failResult = this._syncResult.filter(pass=> pass.Status == 'FAIL');
                this.prepareData = true;
            }, error => {
                console.log("Fail to loading data.");
                this.prepareData = true;
                this.failToLoad = true;
            });
        }
        public checkAtLeastOne(from: string): number {
            if (this._syncResult == null) { return; }
            if (from == 'syncAll') { return this._syncResult.filter(it=> it.Checking).length; }
            else if (from == 'syncPass') { return this._passResult.filter(it=> it.Checking).length; }
            else if (from == 'syncFail') { return this._failResult.filter(it=> it.Checking).length; }

        }
        public checkAllCheckBox(status: boolean, fromObj: string) {
            if (fromObj == 'allResult') {
                this._syncResult.forEach(res=> res.Checking = status);
            }
            else if (fromObj == 'passResult') {
                this._passResult.forEach(res=> res.Checking = status);
            }
            else if (fromObj == 'failResult') {
                this._failResult.forEach(res=> res.Checking = status);
            }
        }
        public sendDataToTirdParty(from: string) {
            if (from == 'syncAll') { this.syncResultSvc.syncresultSendToTirdParty(this._syncResult.filter(it=> it.Checking == true)); }
            else if (from == 'syncPass') { this.syncResultSvc.syncresultSendToTirdParty(this._passResult.filter(it=> it.Checking == true)); }
            else if (from == 'syncFail') { this.syncResultSvc.syncresultSendToTirdParty(this._failResult.filter(it=> it.Checking == true)); }

        }
    }

    angular
        .module('application.syncResultApp')
        .controller('application.syncResultApp.syncResultController', syncResultController);
}