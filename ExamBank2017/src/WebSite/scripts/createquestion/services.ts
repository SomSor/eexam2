module app.createQuestionApp {
    'use strict';

    interface ICreateQuestionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Create(data: T): T;
    }
    interface IUpdateQuestionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Update(data: T): T;
    }
    interface IDeleteQuestionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Delete(data: T): T;
    }

    export class createQuestionService {
        private createQuestionSvc: ICreateQuestionResourceClass<any>;
        private updateQuestionSvc: IUpdateQuestionResourceClass<any>;
        private deleteQuestionSvc: IDeleteQuestionResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            this.createQuestionSvc = <ICreateQuestionResourceClass<any>>$resource(appConfig.CreateQuestionUrl, {}, { Create: { method: 'POST' } });
            this.updateQuestionSvc = <IUpdateQuestionResourceClass<any>>$resource(appConfig.UpdateQuestionUrl, {}, { Update: { method: 'PUT' } });
            this.deleteQuestionSvc = <IDeleteQuestionResourceClass<any>>$resource(appConfig.DeleteQuestionUrl, { 'examsuiteid': '@examsuiteid', 'questionid': '@questionid' }, { Delete: { method: 'DELETE' } });
        }

        public Create(question: app.examSuiteApp.Question): ng.IPromise<any> {
            return this.createQuestionSvc.Create(question).$promise;
        }

        public Update(question: app.examSuiteApp.Question): ng.IPromise<any> {
            return this.updateQuestionSvc.Update(question).$promise;
        }

        public Delete(examsuiteid: string, questionid: string): ng.IPromise<any> {
            return this.deleteQuestionSvc.Delete({ examsuiteid: examsuiteid, questionid: questionid }).$promise;
        }
    }

    angular
        .module('app.createQuestionApp')
        .service('app.createQuestionApp.createQuestionService', createQuestionService);
}