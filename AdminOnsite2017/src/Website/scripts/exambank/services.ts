module application.exambankApp {
    'use strict';

    interface IListExamDataResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ListExamData(data: T): T;
    }
    interface IDownloadResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Download(data: T): T;
    }
    export class exambankService {
        private ListExamDatagSvc: IListExamDataResourceClass<any>;
        private DownloadSvc: IDownloadResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            this.ListExamDatagSvc = <IListExamDataResourceClass<any>>$resource(appConfig.ExamDataListExamDataUrl, { 'centerid': '@centerid' }, { ListExamData: { method: 'GET' } });
            this.DownloadSvc = <IDownloadResourceClass<any>>$resource(appConfig.ExamDataDownloadUrl, null, { Download: { method: 'POST' } });
        }
        public ListExamData(): ng.IPromise<any> {
            var centerid = application.mainApp.mainController.centerID;
            return this.ListExamDatagSvc.get({ 'centerid': centerid }).$promise;
        }
        public Download(subjectcode: string, examlanguage: string, voicelanguage: string, quatity: number): ng.IPromise<any> {
            var centerid = application.mainApp.mainController.centerID;
            var _downloadRequrst = new DownloadRequest(subjectcode, examlanguage, voicelanguage, quatity, centerid);
            return this.DownloadSvc.Download(_downloadRequrst).$promise;
        }

    }

    angular
        .module('application.exambankApp')
        .service('application.exambankApp.exambankService', exambankService);
}