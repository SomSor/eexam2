module application.mainApp {
    'use strict';

    class mainController {
        public _mainVm: MainVM;
        public static _appointDateData: Date[];
        public myDate: Date;
        public minDate: Date;
        public maxDate: Date;
        public prepareData: boolean;
        public failToLoad: boolean;
        static $inject = ['$scope', 'application.mainApp.mainService','application.shared.sharedService'];
        constructor(private $scope, private mainSvc: application.mainApp.mainService, private sharedSVC: application.shared.sharedService) {
            var currentDate = new Date();
            this.myDate = currentDate;
            this.minDate = currentDate;
            this.maxDate = currentDate;
            this.dataSettingByDate(currentDate);
        }
        private dataSettingByDate(currentDate: Date): void {
            this.mainSvc.mainGetTestRegisterationByDate(currentDate).then(re => {
                this.setReturnedTestRegisteration(re)
                this.prepareData = true;
            }, error => {
                console.log("Fail to loading data.");
                this.prepareData = true;
                this.failToLoad = true;
            });
        }
        private dataSettingBySearch(filter: string): void {
            this.mainSvc.mainGetTestRegisterationBySearch(filter).then(re => {
                if (re != null) {
                    this._mainVm.Testregistrations = re.Testregistrations;
                    this.prepareData = true;
                }
            }, error => {
                console.log("Fail to loading data.");
            });
        }
        public getTestRegisterationByDate() {
            this.prepareData = false;
            //TODO: do not change Appointed Date List
            this.dataSettingByDate(this.myDate);
        }
        public getTestRegisterationBySearch(filter: string) {
            this.prepareData = false;
            //must not change Appointed Date List
            this.dataSettingBySearch(filter);
        }
        public setReturnedTestRegisteration(re) {
            var currentDate = new Date();
            this._mainVm = re;

            var appointDates: Date[] = [];
            for (var date of re.AppointDates) {
                var dateArray = date.split('-');
                var newDate = new Date(parseInt(dateArray[0]), parseInt(dateArray[1]) - 1, parseInt(dateArray[2]));
                appointDates.push(newDate);
            }

            this._mainVm.AppointDates = appointDates;
            if (this._mainVm.AppointDates != null && this._mainVm.AppointDates.length > 0) {
                this.minDate = this._mainVm.AppointDates[0];
                this.maxDate = this._mainVm.AppointDates[this._mainVm.AppointDates.length - 1];
                mainController._appointDateData = appointDates;
            }
        }
        public onlyPredicate(date: Date) {
            if (mainController._appointDateData){
                return mainController._appointDateData.filter(x => x.toDateString() == date.toDateString()).length > 0;
            }
            
        }
    }

    angular
        .module('application.mainApp')
        .controller('application.mainApp.mainController', mainController);
}