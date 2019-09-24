module application.printqrApp {
    'use strict';

    class printqrController {
        public _printqr: PrintQRVM;
        static $inject = ['$scope', 'application.printqrApp.printqrService'];
        constructor(private $scope, private printqrSvc: application.printqrApp.printqrService) {
        }
        public printQR(pid: string) {
            this.printqrSvc.GetInfoForPrintQR(pid).then(re=> {
                this._printqr = re;
            });
        }
        public goPrintQRCode(pid: string) {
            window.open('print.html#!/qrcode/' + pid, '_blank');
        }
    }
    class printqrForPrintQrController {
        public _printqr: PrintQRVM;
        public _pid: string;
        static $inject = ['$scope', 'application.printqrApp.printqrService', '$stateParams'];
        constructor(private $scope, private printqrSvc: application.printqrApp.printqrService, private $stateParams) {
            this._pid = this.$stateParams.pid;
            this.printqrSvc.GetInfoForPrintQR(this._pid).then(qr=> {
                this._printqr = qr;
                setTimeout(function () { window.print(); }, 10);
            });
        }
    }

    angular
        .module('application.printqrApp')
        .controller('application.printqrApp.printqrController', printqrController)
        .controller('application.printqrApp.printqrForPrintQrController', printqrForPrintQrController);
}