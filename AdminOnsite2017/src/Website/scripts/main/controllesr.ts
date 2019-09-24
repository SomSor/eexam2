module application.mainApp {
    'use strict';

    export class mainController {
        public _moreInfo: MainVM;
        public validDate;
        public LoadComplete: boolean = false;
        public _endHourTime: number;
        public _endMinuteTime: number;
        public _disableActive: boolean = true;
        public prepareData: boolean = true;
        public static centerID: string;
        static $inject = ['$scope', 'application.mainApp.mainService', '$window'];
        constructor(private $scope, private mainSvc: application.mainApp.mainService, private $window) {
            this.validDate = false;
            this.mainSvc.GetMainInfo().then(info=> {
                this._moreInfo = info;
                this._endHourTime = new Date(this._moreInfo.ActiveThruDateTime.toLocaleString()).getHours();
                this._endMinuteTime = new Date(this._moreInfo.ActiveThruDateTime.toLocaleString()).getMinutes();
                this.cannotActive();
            });
            this.mainSvc.GetcenterData().then(re=> {
                mainController.centerID = re._id
                this.LoadComplete = true;
            });
        }
        public Active(expriedHours: number, expriedMinutes: number) {
            this.prepareData = false;
            var timeToActive = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), expriedHours, expriedMinutes, 0);
            this.mainSvc.Active(timeToActive).then(re => {
                this.prepareData = true;
                this.$window.location.reload();
            }, error => {
                this.prepareData = true;
            });
        }
        public CloseExamData() {
            this.mainSvc.CloseExamData();
        }
        public Refresh() {
            this.$window.location.reload();
        }
        public cannotActive() {
            var activeDate = new Date(this._moreInfo.ActiveThruDateTime.toLocaleString());
            if (this._moreInfo != null) {
                if (activeDate < new Date()) {
                    this._endHourTime = null;
                    this._endMinuteTime = null;
                    this._disableActive = false
                }
                else { this._disableActive = true }
            }
            else this._disableActive = true
        }
    }

    angular
        .module('application.mainApp')
        .controller('application.mainpageApp.mainController', mainController);
}