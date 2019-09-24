module app.createInactiveSubjectApp {
    'use strict';

    interface ICreateInactiveSubjectResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Create(data: T): T;
    }
    interface IUpdateInactiveSubjectResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Update(data: T): T;
    }
    interface IDeleteInactiveSubjectResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Delete(data: T): T;
    }

    export class createInactiveSubjectService {
        private createInactiveSubjectSvc: ICreateInactiveSubjectResourceClass<any>;
        private updateInactiveSubjectSvc: IUpdateInactiveSubjectResourceClass<any>;
        private deleteInactiveSubjectSvc: IDeleteInactiveSubjectResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(private appConfig: IAppConfig, $resource) {
            this.createInactiveSubjectSvc = <ICreateInactiveSubjectResourceClass<any>>$resource(appConfig.CreateInactiveSubjectUrl, {}, { Create: { method: 'POST' } });
            this.updateInactiveSubjectSvc = <IUpdateInactiveSubjectResourceClass<any>>$resource(appConfig.UpdateInactiveSubjectUrl, {}, { Update: { method: 'PUT' } });
            this.deleteInactiveSubjectSvc = <IDeleteInactiveSubjectResourceClass<any>>$resource(appConfig.DeleteInactiveSubjectUrl, { 'subjectid': '@subjectid' }, { Delete: { method: 'DELETE' } });
        }

        public Create(subjectDetail: app.subjectlistApp.SubjectDetail): ng.IPromise<any> {
            return this.createInactiveSubjectSvc.Create(subjectDetail).$promise;
        }

        public Update(subjectDetail: app.subjectlistApp.SubjectDetail): ng.IPromise<any> {
            return this.updateInactiveSubjectSvc.Update(subjectDetail).$promise;
        }

        public Delete(subjectid: string): ng.IPromise<any> {
            return this.deleteInactiveSubjectSvc.Delete({ 'subjectid': subjectid }).$promise;
        }
    }

    angular
        .module('app.createInactiveSubjectApp')
        .service('app.createInactiveSubjectApp.createInactiveSubjectService', createInactiveSubjectService);
}