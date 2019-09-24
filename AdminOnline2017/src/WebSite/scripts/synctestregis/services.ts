module application.syncTestRegisApp {
    'use strict';

    interface IListTestRegisForApprovedResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }
    interface ISubmitTestRegisResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SubmitTestRegis(data: T): T;
    }
    interface ISubmitTestRegisResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        CreateTestRegistration(data: T): T;
    }

    export class syncTestRegisService {
        private syncTestRegisListTestRegisForApprovedSvc: IListTestRegisForApprovedResourceClass<any>;
        private syncTestRegisSubmitTestRegisSvc: ISubmitTestRegisResourceClass<any>;
        private syncTestRegisCreateTestRegistrationSvc: ISubmitTestRegisResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'application.shared.sharedService'];
        constructor(appConfig: IAppConfig, $resource, private sharedSVC: application.shared.sharedService)  {
            //syncTestRegis
            this.syncTestRegisListTestRegisForApprovedSvc = <IListTestRegisForApprovedResourceClass<any>>$resource(appConfig.SyncTestRegisListTestRegisForApprovedUrl, { 'centerid': '@centerid' }, { GetContent: { method: 'GET' } });
            this.syncTestRegisSubmitTestRegisSvc = <ISubmitTestRegisResourceClass<any>>$resource(appConfig.SyncTestRegisSubmitTestRegisUrl, null, { SubmitTestRegis: { method: 'POST' } });
            this.syncTestRegisCreateTestRegistrationSvc = <ISubmitTestRegisResourceClass<any>>$resource(appConfig.CreateTestRegistration, null, { CreateTestRegistration: { method: 'POST' } });
        }

        public syncTestRegisListTestRegisForApproved(): ng.IPromise<any> {
            var centerid = this.sharedSVC._centerData._id;
            return this.syncTestRegisListTestRegisForApprovedSvc.get({ 'centerid': encodeURI(centerid) }).$promise;
        }
        public syncTestRegisSubmitTestRegis(testRegistrations: shared.TestRegistration[]): ng.IPromise<any> {
            return this.syncTestRegisSubmitTestRegisSvc.SubmitTestRegis(testRegistrations).$promise;
        }
        public syncTestRegisCreateTestRegistration(testRegistrations: SyncTestRegisVM): ng.IPromise<any> {
            return this.syncTestRegisCreateTestRegistrationSvc.CreateTestRegistration(testRegistrations).$promise;
        }
    }

    angular
        .module('application.syncTestRegisApp')
        .service('application.syncTestRegisApp.syncTestRegisService', syncTestRegisService);
}