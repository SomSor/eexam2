module application.printqrApp {
    'use strict';

    interface IGetInfoForPrintQRResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetInfoForPrintQR(data: T): T;
    }
    export class printqrService {
        private GetInfoForPrintQRSvc: IGetInfoForPrintQRResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            this.GetInfoForPrintQRSvc = <IGetInfoForPrintQRResourceClass<any>>$resource(appConfig.PrintqrGetInfoForPrintQRUrl, { 'pid': '@pid' }, { GetInfoForPrintQR: { method: 'GET' } });
        }

        public GetInfoForPrintQR(pid:string): ng.IPromise<any> {
            return this.GetInfoForPrintQRSvc.get({ 'pid': pid }).$promise;
        }
    }

    angular
        .module('application.printqrApp')
        .service('application.printqrApp.printqrService', printqrService);
}