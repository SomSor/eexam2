module app.shared {
    'use strict';

    export class sharedController {
        public _centerID: string;
        public _Obj: shared.CenterDataRequest;
        public prepareData: number = 0;
        static $inject = ['$scope', 'app.shared.sharedService'];
        constructor(private $scope, private sharedSVC: app.shared.sharedService) {

        }
        private SetDate(data: string) {
            this._Obj = angular.fromJson(data);
            this._centerID = this._Obj._id;
            console.log("Date is ");
            console.log(this._Obj);
            this.sharedSVC.setValue(this._Obj);
        }
    }

    angular
        .module('app.shared')
        .controller('app.shared.sharedController', sharedController);
}