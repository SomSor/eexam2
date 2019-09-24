module application.mainpageApp {
    'use strict';

    interface IListTestregistrationResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ListTestregistrationData(data: T): T;
    }

    interface ISearchTestRegisResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SearchTestRegisData(data: T): T;
    }

    interface IChangeUserLanguageResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ChangeUserLanguage(data: T): T;
    }

    export class mainpageService {
        private MainpageListTestregistrationSvc: IListTestregistrationResourceClass<any>;
        private MainpageSearchTestRegisnSvc: ISearchTestRegisResourceClass<any>;
        private MainpageChangeUserLanguageSvc: IChangeUserLanguageResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            this.MainpageListTestregistrationSvc = <IListTestregistrationResourceClass<any>>$resource(appConfig.MainpageListTestregistrationUrl, { 'centerid': '@centerid' }, { ListTestregistrationData: { method: 'GET' } });
            this.MainpageSearchTestRegisnSvc = <ISearchTestRegisResourceClass<any>>$resource(appConfig.MainpageSearchTestRegisUrl, { 'txt': '@txt', 'centerid': '@centerid' }, { SearchTestRegisData: { method: 'GET' } });
            this.MainpageChangeUserLanguageSvc = <IChangeUserLanguageResourceClass<any>>$resource(appConfig.MainpageChangeUserLanguage, null, { ChangeUserLanguage: { method: 'POST' } });
        }
        public ListTestregistration(): ng.IPromise<any> {
            var centerid = application.mainApp.mainController.centerID;
            return this.MainpageListTestregistrationSvc.get({ 'centerid': centerid }).$promise;
        }

        public SearchTestRegisn(txt: string): ng.IPromise<any> {
            var centerid = application.mainApp.mainController.centerID;
            return this.MainpageSearchTestRegisnSvc.get({ 'txt': txt,'centerid': centerid}).$promise;
        }

        public ChangeUserLanguage(userDate: shared.TestRegistrationRespone): ng.IPromise<any> {
            return this.MainpageChangeUserLanguageSvc.ChangeUserLanguage(userDate).$promise;
        }

    }

    angular
        .module('application.mainpageApp')
        .service('application.mainpageApp.mainpageService', mainpageService);
}