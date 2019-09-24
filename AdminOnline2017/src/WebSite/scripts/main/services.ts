module application.mainApp {
    'use strict';

    interface IGetTestRegisterationByDateResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }
    interface IGetTestRegisterationBySerachResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }

    export class mainService {
        private mainGetTestRegisterationByDateSvc: IGetTestRegisterationByDateResourceClass<any>;
        private mainGetTestRegisterationBySearchSvc: IGetTestRegisterationBySerachResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'application.shared.sharedService'];
        constructor(appConfig: IAppConfig, $resource, private sharedSVC: application.shared.sharedService) {
            //Main
            this.mainGetTestRegisterationByDateSvc = <IGetTestRegisterationByDateResourceClass<any>>$resource(appConfig.MainListAppointTestRegisByDateUrl, { 'centerid': '@centerid', 'date': '@date' }, { GetContent: { method: 'GET' } });
            this.mainGetTestRegisterationBySearchSvc = <IGetTestRegisterationBySerachResourceClass<any>>$resource(appConfig.MainSerachAppointTestRegisUrl, { 'centerid': '@centerid', 'filter': '@filter' }, { GetContent: { method: 'GET' } });
        }

        public mainGetTestRegisterationByDate(date: Date): ng.IPromise<any> {
            var centerid = this.sharedSVC._centerData._id;
            var dateStr = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
            return this.mainGetTestRegisterationByDateSvc.get({ 'centerid': encodeURI(centerid), 'date': encodeURI(dateStr) }).$promise;
        }
        public mainGetTestRegisterationBySearch(filter: string): ng.IPromise<any> {
            var centerid = this.sharedSVC._centerData._id;
            return this.mainGetTestRegisterationBySearchSvc.get({ 'centerid': encodeURI(centerid), 'filter': filter }).$promise;
        }
    }

    angular
        .module('application.mainApp')
        .service('application.mainApp.mainService', mainService);
}