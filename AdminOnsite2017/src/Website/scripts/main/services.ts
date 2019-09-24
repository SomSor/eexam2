module application.mainApp {
    'use strict';

    interface IGetMainInfoResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetMainData(data: T): T;
    }
    interface IActiveResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Active(data: T): T;
    }
    interface ICloseExamDataResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        CloseExamData(data: T): T;
    }
    interface IGetcenterDataResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetData(data: T): T;
    }

    export class mainService {
        private MainGetMainInfoSvc: IGetMainInfoResourceClass<any>;
        private MainActiveSvc: IActiveResourceClass<any>;
        private MainCloseExamDataSvc: ICloseExamDataResourceClass<any>;
        private MainGetCenterDataSvc: IGetcenterDataResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            this.MainGetMainInfoSvc = <IGetMainInfoResourceClass<any>>$resource(appConfig.MainGetMainInfoUrl, { 'centerid': '@centerid' }, { GetMainData: { method: 'GET' } });
            this.MainActiveSvc = <IActiveResourceClass<any>>$resource(appConfig.MainAcitveUrl, null, { Active: { method: 'POST' } });
            this.MainCloseExamDataSvc = <ICloseExamDataResourceClass<any>>$resource(appConfig.MainCloseExamDataUrl, null, { CloseExamData: { method: 'GET' } });
            this.MainGetCenterDataSvc = <IGetcenterDataResourceClass<any>>$resource(appConfig.LoginGetCenterDataUrl, null, { GetData: { method: 'GET' } });
        }
        public GetMainInfo(): ng.IPromise<any> {
            //HACK : fix centerId
            var centerid = '1';
            return this.MainGetMainInfoSvc.get({ 'centerid': centerid }).$promise;
        }
        public Active(time: Date): ng.IPromise<any> {
            //HACK : fix centerId
            var centerid = '1';
            var activeRequest = new ActiveRequest(time, centerid);
            return this.MainActiveSvc.Active(activeRequest).$promise;
        }
        public CloseExamData(): ng.IPromise<any> {
            return this.MainCloseExamDataSvc.get().$promise;
        }
        public GetcenterData(): ng.IPromise<any> {
            return this.MainGetCenterDataSvc.get().$promise;
        }
    }

    angular
        .module('application.mainApp')
        .service('application.mainApp.mainService', mainService);
}