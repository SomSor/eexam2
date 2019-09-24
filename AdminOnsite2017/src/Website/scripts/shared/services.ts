module application.shared {
    'use strict';

    interface ICancelTestResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        CancelTest(data: T): T;
    }
    interface IEndTestResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        EndTest(data: T): T;
    }
    interface IResumeTestResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ResumeTest(data: T): T;
    }
    interface IGetTestResultResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetTestResult(data: T): T;
    }
    interface IGetcenterDataResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetData(data: T): T;
    }
    export class sharedService {
        private MainpageCancelTestSvc: ICancelTestResourceClass<any>;
        private MainpageEndTestSvc: IEndTestResourceClass<any>;
        private MainpageResumeTestSvc: IResumeTestResourceClass<any>;
        private GetTestResultSvc: IGetTestResultResourceClass<any>;
        private SharedGetCenterDataSvc: IGetcenterDataResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            this.MainpageCancelTestSvc = <ICancelTestResourceClass<any>>$resource(appConfig.MainCancelTestUrl, null, { CancelTest: { method: 'POST' } });
            this.MainpageEndTestSvc = <IEndTestResourceClass<any>>$resource(appConfig.MainEndTestUrl, null, { EndTest: { method: 'POST' } });
            this.MainpageResumeTestSvc = <IResumeTestResourceClass<any>>$resource(appConfig.MainResumeTestUrl, null, { ResumeTest: { method: 'POST' } });
            this.GetTestResultSvc = <IGetTestResultResourceClass<any>>$resource(appConfig.GetTestResultUrl, { 'pid': '@pid', 'sheetid': '@sheetid'  }, { GetInfoForPrintQR: { method: 'GET' } });
            this.SharedGetCenterDataSvc = <IGetcenterDataResourceClass<any>>$resource(appConfig.LoginGetCenterDataUrl, null, { GetData: { method: 'GET' } });
        }

        public Cancel(pid: string, sheetid: string): ng.IPromise<any> {
            var actionSheetRequest = new ActionSheetRequest(pid, sheetid);
            return this.MainpageCancelTestSvc.CancelTest(actionSheetRequest).$promise;
        }
        public EndTest(pid: string, sheetid: string): ng.IPromise<any> {
            var actionSheetRequest = new ActionSheetRequest(pid, sheetid);
            return this.MainpageEndTestSvc.EndTest(actionSheetRequest).$promise;
        }
        public Resume(pid: string, sheetid: string): ng.IPromise<any> {
            var actionSheetRequest = new ActionSheetRequest(pid, sheetid);
            return this.MainpageResumeTestSvc.ResumeTest(actionSheetRequest).$promise;
        }
        public GetTestResult(pid: string, sheetid: string): ng.IPromise<any> {
            return this.GetTestResultSvc.get({ 'pid': pid, 'sheetid': sheetid }).$promise;
        }
        public GetcenterData(): ng.IPromise<any> {
            return this.SharedGetCenterDataSvc.get().$promise;
        }
    }

    angular
        .module('application.shared')
        .service('application.shared.sharedService', sharedService);
}