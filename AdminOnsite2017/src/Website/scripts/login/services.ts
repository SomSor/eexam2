module application.loginApp {
    'use strict';

    interface IGetcenterDataResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetData(data: T): T;
    }
    interface ILoginResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Login(data: T): T;
    }
    export class loginService {
        private LoginGetCenterDataSvc: IGetcenterDataResourceClass<any>;
        private LoginSvc: ILoginResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            this.LoginGetCenterDataSvc = <IGetcenterDataResourceClass<any>>$resource(appConfig.LoginGetCenterDataUrl, null, { GetData: { method: 'GET' } });
            this.LoginSvc = <ILoginResourceClass<any>>$resource(appConfig.LoginUrl, null, { Login: { method: 'POST' } });
        }
        public LoginGetcenterData(): ng.IPromise<any> {
            return this.LoginGetCenterDataSvc.get().$promise;
        }
        public Login(username: string, password: string,centerid:string): ng.IPromise<any> {
            var userRequest = new UserRequest(username, password, centerid);
            console.log(userRequest.User);
            return this.LoginSvc.Login(userRequest).$promise;
        }

    }

    angular
        .module('application.loginApp')
        .service('application.loginApp.loginService', loginService);
}