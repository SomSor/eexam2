module application.testBankApp {
    'use strict';

    interface IListExamSheetResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }
    interface ISubmitExamSheetResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SubmitExamSheet(data: T): T;
    }
    interface IListSubjectResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ListSubject(data: T): T;
    }
    interface ICreateexamsheetResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Createexamsheet(data: T): T;
    }
    interface ICheckSubjectVersionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        CheckSubjectVersion(data: T): T;
    }
    export class testBankService {
        private testBankListExamSheetSvc: IListExamSheetResourceClass<any>;
        private testBankSubmitExamSheetSvc: ISubmitExamSheetResourceClass<any>;
        private testBankListSubjectSvc: IListSubjectResourceClass<any>;
        private syncTestRegisCreateexamsheetSvc: ICreateexamsheetResourceClass<any>;
        private syncTestRegisCheckSubjectVersionSvc: ICheckSubjectVersionResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'application.shared.sharedService'];
        constructor(appConfig: IAppConfig, $resource, private sharedSVC: application.shared.sharedService)  {
            //syncTestRegis
            this.testBankListExamSheetSvc = <IListExamSheetResourceClass<any>>$resource(appConfig.TestBankListExamSheetUrl, { 'centerid': '@centerid' }, { GetContent: { method: 'GET' } });
            this.testBankSubmitExamSheetSvc = <ISubmitExamSheetResourceClass<any>>$resource(appConfig.TestBankSubmitExamSheetUrl, { 'centerid': '@centerid', 'subjectcode': '@subjectcode', 'examlanguage': '@examlanguage', 'voicelanguage': '@voicelanguage', 'quantity': '@quantity' }, { SubmitExamSheet: { method: 'POST' } });
            this.testBankListSubjectSvc = <IListSubjectResourceClass<any>>$resource(appConfig.ListSubjectUrl, null, { ListSubject: { method: 'GET' } });
            this.syncTestRegisCreateexamsheetSvc = <ICreateexamsheetResourceClass<any>>$resource(appConfig.CreateExamSheet, null, { Createexamsheet: { method: 'POST' } });
            this.syncTestRegisCheckSubjectVersionSvc = <ICheckSubjectVersionResourceClass<any>>$resource(appConfig.TestBankCheckSubjectVersion, { 'centerid': '@centerid', 'subjectcode': '@subjectcode', 'version': '@version' }, { CheckSubjectVersion: { method: 'GET' } });
        }

        public syncTestRegisListExamSheet(): ng.IPromise<any> {
            var centerid = this.sharedSVC._centerData._id;
            return this.testBankListExamSheetSvc.get({ 'centerid': encodeURI(centerid) }).$promise;
        }
        public syncTestRegisSubmitExamSheet(SubjectCode: string, ExamLanguage: string, VoiceLanguage: string, Quatity: number): ng.IPromise<any> {
            var centerid = this.sharedSVC._centerData._id;
            return this.testBankSubmitExamSheetSvc.SubmitExamSheet({ 'centerid': centerid, 'subjectcode': SubjectCode, 'examlanguage': ExamLanguage, 'voicelanguage': VoiceLanguage, 'quantity': Quatity }).$promise;
        }
        public testBankListSubject(): ng.IPromise<any> {
            var centerid = this.sharedSVC._centerData._id;
            return this.testBankListSubjectSvc.get({ 'siteid': this.sharedSVC._centerData.SiteId}).$promise;
        }
        public syncTestCreateexamsheet(json: TestBankVM): ng.IPromise<any> {
            return this.syncTestRegisCreateexamsheetSvc.Createexamsheet(json).$promise;
        }
        public CheckSubjectVersion(subjectcode: string, version:string): ng.IPromise<any> {
            var centerid = this.sharedSVC._centerData._id;
            return this.syncTestRegisCheckSubjectVersionSvc.get({ 'centerid': centerid, 'subjectcode': subjectcode,'version':version}).$promise;
        }
    }1

    angular
        .module('application.testBankApp')
        .service('application.testBankApp.testBankService', testBankService);
}