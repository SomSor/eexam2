module application.searchresultApp {
    'use strict';

    interface IGetResultResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetResult(data: T): T;
    }
    export class searchresultService {
        private GetResultQRSvc: IGetResultResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            this.GetResultQRSvc = <IGetResultResourceClass<any>>$resource(appConfig.ResultGetResultUrl, { 'pid': '@pid' }, { GetResult: { method: 'GET' } });
        }

        public GetResult(pid: string): ng.IPromise<any> {
            return this.GetResultQRSvc.get({ 'pid': pid }).$promise;
        }
    }

    angular
        .module('application.searchresultApp')
        .service('application.searchresultApp.searchresultService', searchresultService);
}