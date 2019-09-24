module application.testingApp {
    'use strict';

    interface IListTestingResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ListTesting(data: T): T;
    }
    export class testingService {
        private TestingListTestingSvc: IListTestingResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            this.TestingListTestingSvc = <IListTestingResourceClass<any>>$resource(appConfig.TestingListTestingUrl, { 'centerid': '@centerid' }, { GetMainData: { method: 'GET' } });

        }
        public ListTesting(): ng.IPromise<any> {
            var centerid = application.mainApp.mainController.centerID;
            return this.TestingListTestingSvc.get({ 'centerid': centerid }).$promise;
        }

    }

    angular
        .module('application.testingApp')
        .service('application.testingApp.testingService', testingService);
}