module app.subjectlistApp {
    'use strict';

    interface IGetInactiveSubjectListResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }
    interface IGetActivatedSubjectListResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }

    export class subjectListService {
        private inactiveGetSubjectListSvc: IGetInactiveSubjectListResourceClass<any>;
        private activatedGetSubjectListSvc: IGetActivatedSubjectListResourceClass<any>;

        static $inject = ['appConfig', '$resource','app.shared.sharedService'];
        constructor(appConfig: IAppConfig, $resource, private sharedSVC : app.shared.sharedService) {

            //Inactive
            this.inactiveGetSubjectListSvc = <IGetInactiveSubjectListResourceClass<any>>$resource(appConfig.InactiveListSubjectUrl, {});
            
            //Activated
            this.activatedGetSubjectListSvc = <IGetActivatedSubjectListResourceClass<any>>$resource(appConfig.ActivatedListSubjectUrl, { 'siteid': '@siteid' }, { GetContent: { method: 'GET' } });

        }

        public InactiveGetSubject(): ng.IPromise<any> {
            return this.inactiveGetSubjectListSvc.get().$promise;
        }

        public ActivatedGetSubject(): ng.IPromise<any> {
            var siteid = this.sharedSVC._centerData.SiteId;
            return this.activatedGetSubjectListSvc.get({ siteid : siteid}).$promise;
        }
    }

    angular
        .module('app.subjectlistApp')
        .service('app.subjectlistApp.subjectListService', subjectListService);
}