module application.shared {
    'use strict';

    export class sharedController {
        public _centerID: string;
        public _Obj: shared.CenterDataRequest;
        public prepareData: number = 0;
        static $inject = ['$scope', 'application.shared.sharedService'];
        constructor(private $scope, private sharedSVC: application.shared.sharedService) {
            
        }
        private alertDD(data: string) {
            this._Obj = angular.fromJson(data);
            this._centerID = this._Obj._id;
            this.sharedSVC.setValue(this._Obj);
            this.sharedSVC.UploadData().then(it => {
                this.prepareData = 1;
            }, error => {
                console.log("Fail to UploadData data.");
                this.prepareData = 2;
            });
        }
    }

    angular
        .module('application.shared')
        .controller('application.shared.sharedController', sharedController);
}