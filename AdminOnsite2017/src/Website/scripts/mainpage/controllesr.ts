module application.mainpageApp {
    'use strict';

    class mainpageController {
        public centerid: string;
        public _testregistration: DisplayAllVM;
        public _showInModal: shared.TestRegistrationRespone;
        public userPID: string;
        public userSheetID: string;
        public userStatus: string;
        public prepareData: boolean = true;
        public userFullName: string;
        static $inject = ['$scope', 'application.mainpageApp.mainpageService', 'application.shared.sharedService', '$window'];
        constructor(private $scope, private mainpageSvc: application.mainpageApp.mainpageService, private shareSvc: application.shared.sharedService, private $window) {
            this.mainpageSvc.ListTestregistration().then(te => {
                this._testregistration = te;
            });
        }
        public searchTestRegis(txt: string) {
            this.mainpageSvc.SearchTestRegisn(txt).then(re => {
                this._testregistration = re
            });
        }
        public goPrintQRCode(pid: string) {
            window.open('print.html#!/qrcode/' + pid, '_blank');
        }
        public goPrintSheet(pid: string, sheetid: string) {
            window.open('print.html#!/testresult/' + pid + '/' + sheetid, '_blank');
        }
        public CancelTest(pid: string, sheetid: string) {
            this.prepareData = false;
            this.shareSvc.Cancel(pid, sheetid).then(re => {
                this.prepareData = true;
                this.$window.location.reload();
            }, error => {
                this.prepareData = true;
            });
        }
        public EndTest(pid: string, sheetid: string) {
            this.prepareData = false;
            this.shareSvc.EndTest(pid, sheetid).then(re => {
                this.prepareData = true;
                this.$window.location.reload();
            }, error => {
                this.prepareData = true;
            });
        }
        public ResumeTest(pid: string, sheetid: string) {
            this.prepareData = false;
            this.shareSvc.Resume(pid, sheetid).then(re => {
                this.prepareData = true;
                this.$window.location.reload();
            }, error => {
                this.prepareData = true;
            });
        }
        public ChangeUserLanguage(langeuage: string) {
            this.prepareData = false;
            this._showInModal.ExamLanguage = langeuage;
            this.mainpageSvc.ChangeUserLanguage(this._showInModal).then(re => {
                this.prepareData = true;
                this.$window.location.reload();
            }, error => {
                this.prepareData = true;
            });
        }
    }

    angular
        .module('application.mainpageApp')
        .controller('application.mainpageApp.mainpageController', mainpageController);
}