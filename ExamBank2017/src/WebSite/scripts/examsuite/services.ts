module app.examSuiteApp {
    'use strict';

    interface IGetInactiveExamSuiteResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }
    interface IGetActivatedExamSuiteResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }
    interface IInactiveConsiderQuestionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ConsiderQuestion(data: T): T;
    }
    interface IInactiveEditQuestionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        EditQuestion(data: T): T;
    }
    interface IActivatedEditQuestionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        EditQuestion(data: T): T;
    }
    interface IInactiveDeleteExamSuiteResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        DeleteExamSuite(data: T): T;
    }
    interface ICreateExamSuiteResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Create(data: T): T;
    }
    interface IUpdateExamSuiteResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Update(data: T): T;
    }
    interface IDeleteExamSuiteResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Delete(data: T): T;
    }

    export class examSuiteService {
        private inactiveGetExamSuiteSvc: IGetInactiveExamSuiteResourceClass<any>;
        private activatedGetExamSuiteSvc: IGetActivatedExamSuiteResourceClass<any>;
        private questioninactiveConsiderQuestionSvc: IInactiveConsiderQuestionResourceClass<any>;
        private questioninactiveEditQuestionSvc: IInactiveEditQuestionResourceClass<any>;
        private questionActivateEditQuestionSvc: IActivatedEditQuestionResourceClass<any>;
        private configurationInActiveDeleteExamSuiteSvc: IInactiveDeleteExamSuiteResourceClass<any>;
        private createExamSuiteSvc: ICreateExamSuiteResourceClass<any>;
        private updateExamSuiteSvc: IUpdateExamSuiteResourceClass<any>;
        private deleteExamSuiteSvc: IDeleteExamSuiteResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {

            //Inactive
            this.inactiveGetExamSuiteSvc = <IGetInactiveExamSuiteResourceClass<any>>$resource(appConfig.InactiveGetExamSuiteUrl, { 'examsuiteid': '@examsuiteid' }, { GetContent: { method: 'GET' } });

            //Activated
            this.activatedGetExamSuiteSvc = <IGetActivatedExamSuiteResourceClass<any>>$resource(appConfig.ActivatedGetExamSuiteUrl, { 'subjectid': '@subjectid', 'examsuiteid': '@examsuiteid' }, { GetContent: { method: 'GET' } });

            //QuestionInActive
            this.questioninactiveConsiderQuestionSvc = <IInactiveConsiderQuestionResourceClass<any>>$resource(appConfig.QuestionInActiveConsiderQuestionUrl, { 'titleCode': '@TitleCode', 'questionNumber': '@QuestionNumber' }, { ConsiderQuestion: { method: 'POST' } });
            this.questioninactiveEditQuestionSvc = <IInactiveEditQuestionResourceClass<any>>$resource(appConfig.QuestionInActiveEditQuestionUrl, { 'titleCode': '@TitleCode', 'questionId': '@id' }, { EditQuestion: { method: 'PUT' } });

            //QuestionActivate
            this.questionActivateEditQuestionSvc = <IActivatedEditQuestionResourceClass<any>>$resource(appConfig.QuestionActivatedEditQuestionUrl, { 'questionId': '@id' }, { EditQuestion: { method: 'PUT' } });

            //ConfigurationInActive
            this.configurationInActiveDeleteExamSuiteSvc = <IInactiveDeleteExamSuiteResourceClass<any>>$resource(appConfig.ConfigurationInActiveDeleteExamSuiteUrl, { 'examsuiteid': '@examsuiteid' }, { DeleteExamSuite: { method: 'GET' } });

            this.createExamSuiteSvc = <ICreateExamSuiteResourceClass<any>>$resource(appConfig.CreateExamSuiteUrl, {}, { Create: { method: 'POST' } });
            this.updateExamSuiteSvc = <IUpdateExamSuiteResourceClass<any>>$resource(appConfig.UpdateExamSuiteUrl, {}, { Update: { method: 'PUT' } });
            this.deleteExamSuiteSvc = <IDeleteExamSuiteResourceClass<any>>$resource(appConfig.DeleteExamSuiteUrl, { 'examsuiteid': '@examsuiteid' }, { Delete: { method: 'DELETE' } });
        }

        public InactiveGetExamSuite(examsuiteid: string): ng.IPromise<any> {
            console.log(examsuiteid);
            return this.inactiveGetExamSuiteSvc.get({ 'examsuiteid': examsuiteid }).$promise;
        }

        public ActivatedGetExamSuite(subjectid: string, examsuiteid: string): ng.IPromise<any> {
            return this.activatedGetExamSuiteSvc.get({ 'subjectid': subjectid, 'examsuiteid': examsuiteid }).$promise;
        }

        public QuestionInactiveConsiderQuestion(consideration: Consideration): ng.IPromise<any> {
            return this.questioninactiveConsiderQuestionSvc.ConsiderQuestion(consideration).$promise;
        }

        public QuestionInactiveEditQuestion(question: Question): void {
            this.questioninactiveEditQuestionSvc.EditQuestion(question);
        }

        public QuestionActivateEditQuestion(question: Question): void {
            this.questionActivateEditQuestionSvc.EditQuestion(question);
        }

        public ConfigurationInactiveDeleteExamSuite(examsuiteid: string): ng.IPromise<any> {
            return this.configurationInActiveDeleteExamSuiteSvc.DeleteExamSuite({ examsuiteid: examsuiteid }).$promise;
        }

        public Create(examSuite: app.subjectApp.ExamSuiteDetail): ng.IPromise<any> {
            console.log(examSuite);
            return this.createExamSuiteSvc.Create(examSuite).$promise;
        }

        public Update(examSuite: app.subjectApp.ExamSuiteDetail): ng.IPromise<any> {
            return this.updateExamSuiteSvc.Update(examSuite).$promise;
        }

        public Delete(examsuiteid: string): ng.IPromise<any> {
            return this.deleteExamSuiteSvc.Delete({ examsuiteid: examsuiteid }).$promise;
        }

    }

    angular
        .module('app.examSuiteApp')
        .service('app.examSuiteApp.examSuiteService', examSuiteService);
}