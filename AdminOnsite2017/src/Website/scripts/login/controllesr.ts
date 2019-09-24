module application.loginApp {
    'use strict';

    class loginController {
        public _centerData: Center;
        public _messageRespone: MessageRespone;
        public _logoUrl: string;
        static $inject = ['$window', '$scope', 'application.loginApp.loginService', 'appConfig'];
        constructor(private $window, private $scope, private loginSvc: application.loginApp.loginService, appConfig) {
            this.loginSvc.LoginGetcenterData().then(re=> {
                this._centerData = re;
                this._logoUrl = appConfig.LogoUrl;
            });
        }
        public login(username: string, password: string) {
            this.loginSvc.Login(username, password, this._centerData._id).then(mes=> {
                this._messageRespone = mes;
                if (this._messageRespone.Code == 'SUCCESS') {
                    window.open('index.html#!/maintab', '_self');
                }
            });
        }

    }

    angular
        .module('application.loginApp')
        .controller('application.loginApp.loginController', loginController);
}