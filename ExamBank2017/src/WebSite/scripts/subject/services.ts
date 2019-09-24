module app.subjectApp {
    'use strict';

    interface IGetInactiveSubjectResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }
    interface IGetActivatedSubjectResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }
    interface IInactiveSubjectActivateSubjectResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ActivateSubject(data: T): T;
    }
    interface IInactiveSubjectAddExamSuiteGroupResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        AddExamSuiteGroup(data: T): T;
    }
    interface IInactiveSubjectDeleteExamSuiteGroupResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        DeleteExamSuiteGroup(data: T): T;
    }
    interface IInactiveSubjectEditExamSuiteGroupResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        EditExamSuiteGroup(data: T): T;
    }
    interface IInactiveSubjectEditExamSuiteGroupMapResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        EditExamSuiteGroupMap(data: T): T;
    }
    interface IActivateSubjectReversionSubjectResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ReversionSubject(data: T): T;
    }
    interface IActivateSubjectDisableSubjectResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        DisableSubject(data: T): T;
    }
    interface IActivateSubjectEnableSubjectResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        EnableSubject(data: T): T;
    }
    interface IActivateSubjectVoiceResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ActivedSubjectVoice(data: T): T;
    }
    interface IActiveSubjectAddExamSuiteGroupResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        AddExamSuiteGroup(data: T): T;
    }
    interface IActivatedSubjectDeleteExamSuiteGroupResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        DeleteExamSuiteGroup(data: T): T;
    }
    interface IActivateSubjectEditExamSuiteGroupResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        EditExamSuiteGroup(data: T): T;
    }
    interface IActivateSubjectEditExamSuiteGroupMapResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        EditExamSuiteGroupMap(data: T): T;
    }

    export class subjectService {
        private inactiveGetSubjectSvc: IGetInactiveSubjectResourceClass<any>;
        private activatedGetSubjectSvc: IGetActivatedSubjectResourceClass<any>;
        private configurationInActiveActiveSubjectSvc: IInactiveSubjectActivateSubjectResourceClass<any>;
        private configurationInActiveAddExamSuiteGroupSvc: IInactiveSubjectAddExamSuiteGroupResourceClass<any>;
        private configurationInActiveDeleteExamSuiteGroupSvc: IInactiveSubjectDeleteExamSuiteGroupResourceClass<any>;
        private configurationInActiveEditExamSuiteGroupSvc: IInactiveSubjectEditExamSuiteGroupResourceClass<any>;
        private configurationInActiveEditExamSuiteGroupMapSvc: IInactiveSubjectEditExamSuiteGroupMapResourceClass<any>;
        private configurationActivatedReversionSubjectSvc: IActivateSubjectReversionSubjectResourceClass<any>;
        private configurationActivatedDisableSubjectSvc: IActivateSubjectDisableSubjectResourceClass<any>;
        private configurationActivatedEnableSubjectSvc: IActivateSubjectEnableSubjectResourceClass<any>;
        private configurationActivatedVoiceSvc: IActivateSubjectVoiceResourceClass<any>;
        private configurationActivatedAddExamSuiteGroupSvc: IActiveSubjectAddExamSuiteGroupResourceClass<any>;
        private configurationActivatedDeleteExamSuiteGroupSvc: IActivatedSubjectDeleteExamSuiteGroupResourceClass<any>;
        private configurationActivatedEditExamSuiteGroupSvc: IActivateSubjectEditExamSuiteGroupResourceClass<any>;
        private configurationActivatedExamSuiteGroupMapSvc: IActivateSubjectEditExamSuiteGroupMapResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {

            //Inactive
            this.inactiveGetSubjectSvc = <IGetInactiveSubjectResourceClass<any>>$resource(appConfig.InactiveGetSubjectUrl, { 'subjectid': '@subjectid' }, { GetContent: { method: 'GET' } });

            //Activated
            this.activatedGetSubjectSvc = <IGetActivatedSubjectResourceClass<any>>$resource(appConfig.ActivatedGetSubjectUrl, { 'subjectid': '@subjectid' }, { GetContent: { method: 'GET' } });

            //ConfigurationInActive
            this.configurationInActiveActiveSubjectSvc = <IInactiveSubjectActivateSubjectResourceClass<any>>$resource(appConfig.ConfigurationInActiveActivateSubjectUrl, { 'subjectId': '@subjectId', 'activateOption': '@activateOption' }, { ActivateSubject: { method: 'GET' } });
            this.configurationInActiveAddExamSuiteGroupSvc = <IInactiveSubjectAddExamSuiteGroupResourceClass<any>>$resource(appConfig.ConfigurationInActiveAddExamSuiteGruopUrl, { 'subjectId': '@SubjectId' }, { AddExamSuiteGroup: { method: 'PUT' } });
            this.configurationInActiveDeleteExamSuiteGroupSvc = <IInactiveSubjectDeleteExamSuiteGroupResourceClass<any>>$resource(appConfig.ConfigurationInActiveDeleteExamSuiteGruopUrl, { 'subjectId': '@SubjectId', 'examsuitegroupid': '@examsuitegroupId' }, { DeleteExamSuiteGroup: { method: 'GET' } });
            this.configurationInActiveEditExamSuiteGroupSvc = <IInactiveSubjectEditExamSuiteGroupResourceClass<any>>$resource(appConfig.ConfigurationInActiveEditExamSuiteGroupUrl, { 'subjectId': '@SubjectId', 'examsuitegroupId': '@id' }, { EditExamSuiteGroup: { method: 'PUT' } });
            this.configurationInActiveEditExamSuiteGroupMapSvc = <IInactiveSubjectEditExamSuiteGroupMapResourceClass<any>>$resource(appConfig.ConfigurationInActiveEditExamSuiteGroupMapUrl, { 'subjectId': '@SubjectId', 'examsuiteGroupMapId': '@id' }, { EditExamSuiteGroupMap: { method: 'PUT' } });

            //ConfigurationActivated
            this.configurationActivatedReversionSubjectSvc = <IActivateSubjectReversionSubjectResourceClass<any>>$resource(appConfig.ConfigurationActivatedReversionSubjectUrl, { 'subjectCode': '@subjectCode', 'subjectId': '@subjectId' }, { ReversionSubject: { method: 'GET' } });
            this.configurationActivatedDisableSubjectSvc = <IActivateSubjectDisableSubjectResourceClass<any>>$resource(appConfig.ConfigurationActivatedDisableSubjectUrl, { 'subjectId': '@subjectId' }, { DisableSubject: { method: 'GET' } });
            this.configurationActivatedEnableSubjectSvc = <IActivateSubjectEnableSubjectResourceClass<any>>$resource(appConfig.ConfigurationActivatedEnableSubjectUrl, { 'subjectId': '@subjectId' }, { EnableSubject: { method: 'GET' } });
            this.configurationActivatedVoiceSvc = <IActivateSubjectVoiceResourceClass<any>>$resource(appConfig.ConfigurationActivatedSubjectVoiceUrl, { 'subjectId': '@id' }, { ActivedSubjectVoice: { method: 'PUT' } });
            this.configurationActivatedAddExamSuiteGroupSvc = <IActiveSubjectAddExamSuiteGroupResourceClass<any>>$resource(appConfig.ConfigurationActivatedAddExamSuiteGruopUrl, { 'subjectId': '@SubjectId' }, { AddExamSuiteGroup: { method: 'PUT' } });
            this.configurationActivatedDeleteExamSuiteGroupSvc = <IActivatedSubjectDeleteExamSuiteGroupResourceClass<any>>$resource(appConfig.ConfigurationActivatedDeleteExamSuiteGruopUrl, { 'subjectId': '@SubjectId', 'examsuitegroupid': '@examsuitegroupId' }, { DeleteExamSuiteGroup: { method: 'GET' } });
            this.configurationActivatedEditExamSuiteGroupSvc = <IActivateSubjectEditExamSuiteGroupResourceClass<any>>$resource(appConfig.ConfigurationActivatedEditExamSuiteGruopUrl, { 'subjectId': '@SubjectId', 'examsuitegroupId': '@id' }, { EditExamSuiteGroup: { method: 'PUT' } });
            this.configurationActivatedExamSuiteGroupMapSvc = <IActivateSubjectEditExamSuiteGroupMapResourceClass<any>>$resource(appConfig.ConfigurationActivatedEditExamSuiteGroupMapUrl, { 'subjectId': '@SubjectId', 'examsuiteGroupMapId': '@id' }, { EditExamSuiteGroupMap: { method: 'PUT' } });
        }

        public GetInactiveSubject(subjectid: string): ng.IPromise<any> {
            return this.inactiveGetSubjectSvc.get({ 'subjectid': subjectid }).$promise;
        }

        public GetActivatedSubject(subjectid: string): ng.IPromise<any> {
            return this.activatedGetSubjectSvc.get({ 'subjectid': subjectid }).$promise;
        }

        //public InactiveSubjectActiveSubject(subjectId: string, activateOption: string): boolean {
        //    return this.configurationInActiveActiveSubjectSvc.ActivateSubject({ subjectId: subjectId, activateOption: activateOption });
        //}

        public InactiveSubjectActiveSubject(subjectId: string, activateOption: string): ng.IPromise<any> {
            return this.configurationInActiveActiveSubjectSvc.ActivateSubject({ subjectId: subjectId, activateOption: activateOption }).$promise;
        }

        public InactiveSubjectAddExamSuiteGroup(subject: ExamSuiteGroup): ng.IPromise<any> {
            return this.configurationInActiveAddExamSuiteGroupSvc.AddExamSuiteGroup(subject).$promise;
        }

        public InactiveSubjectDeleteExamSuiteGroup(subjectId: string, examsuitegroupid: string): ng.IPromise<any> {
            return this.configurationInActiveDeleteExamSuiteGroupSvc.DeleteExamSuiteGroup({ subjectId: subjectId, examsuitegroupid: examsuitegroupid }).$promise;
        }

        public InactiveSubjectEditExamSuiteGroup(examsuiteGroup: ExamSuiteGroup): void {
            this.configurationInActiveEditExamSuiteGroupSvc.EditExamSuiteGroup(examsuiteGroup);
        }

        public InactiveSubjectEditExamSuiteGroupMap(examsuiteGroupMap: ExamSuiteGroupMap): void {
            this.configurationInActiveEditExamSuiteGroupMapSvc.EditExamSuiteGroupMap(examsuiteGroupMap);
        }

        public ActivatedSubjectReversionSubject(subjectCode: string, subjectId: string): ng.IPromise<any> {
            return this.configurationActivatedReversionSubjectSvc.ReversionSubject({ subjectCode: subjectCode, subjectId: subjectId }).$promise;
        }

        public ActivatedSubjectDisableSubject(subjectId: string): ng.IPromise<any> {
            return this.configurationActivatedDisableSubjectSvc.DisableSubject({ subjectId: subjectId }).$promise;
        }

        public ActivatedSubjectEnableSubject(subjectId: string): ng.IPromise<any> {
            return this.configurationActivatedEnableSubjectSvc.EnableSubject({ subjectId: subjectId }).$promise;
        }

        public ActivatedSubjectVoices(subject: Subject): void {
            this.configurationActivatedVoiceSvc.ActivedSubjectVoice(subject);
        }

        public ActivateSubjectAddExamSuiteGroup(subject: ExamSuiteGroup): ng.IPromise<any> {
            return this.configurationActivatedAddExamSuiteGroupSvc.AddExamSuiteGroup(subject).$promise;
        }

        public ActivateSubjectDeleteExamSuiteGroup(subjectId: string, examsuitegroupid: string): void {
            return this.configurationActivatedDeleteExamSuiteGroupSvc.DeleteExamSuiteGroup({ subjectId: subjectId, examsuitegroupid: examsuitegroupid });
        }

        public ActivateSubjectEditExamSuiteGroup(examsuiteGroup: ExamSuiteGroup): void {
            this.configurationActivatedEditExamSuiteGroupSvc.EditExamSuiteGroup(examsuiteGroup);
        }

        public ActivateSubjectEditExamSuiteGroupMap(examsuiteGroupMap: ExamSuiteGroupMap): void {
            this.configurationActivatedExamSuiteGroupMapSvc.EditExamSuiteGroupMap(examsuiteGroupMap);
        }
    }

    angular
        .module('app.subjectApp')
        .service('app.subjectApp.subjectService', subjectService);
}