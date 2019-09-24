module app {
    'use strict';

    export interface IAppConfig {

        //Inactive
        InactiveListSubjectUrl: string;
        InactiveGetSubjectUrl: string;
        InactiveGetExamSuiteUrl: string;
        CreateInactiveSubjectUrl: string;
        UpdateInactiveSubjectUrl: string;
        DeleteInactiveSubjectUrl: string;

        //Activated
        ActivatedListSubjectUrl: string;
        ActivatedGetSubjectUrl: string;
        ActivatedGetExamSuiteUrl: string;

        //QuestionInActive
        QuestionInActiveConsiderQuestionUrl: string;
        QuestionInActiveEditQuestionUrl: string;

        //QuestionActivated
        QuestionActivatedEditQuestionUrl: string;

        //ConfigurationInActive
        ConfigurationInActiveActivateSubjectUrl: string;
        ConfigurationInActiveAddExamSuiteGruopUrl: string;
        ConfigurationInActiveDeleteExamSuiteGruopUrl: string;
        ConfigurationInActiveEditExamSuiteGroupUrl: string;
        ConfigurationInActiveEditExamSuiteGroupMapUrl: string;
        ConfigurationInActiveDeleteExamSuiteUrl: string;
        CreateExamSuiteUrl: string;
        UpdateExamSuiteUrl: string;
        DeleteExamSuiteUrl: string;
        CreateQuestionUrl: string;
        UpdateQuestionUrl: string;
        DeleteQuestionUrl: string;
        
        //ConfigurationActivated
        ConfigurationActivatedReversionSubjectUrl: string;
        ConfigurationActivatedDisableSubjectUrl: string;
        ConfigurationActivatedEnableSubjectUrl: string;
        ConfigurationActivatedEditExamSuiteGruopUrl: string;
        ConfigurationActivatedEditExamSuiteGroupMapUrl: string;
        ConfigurationActivatedAddExamSuiteGruopUrl: string;
        ConfigurationActivatedDeleteExamSuiteGruopUrl: string;
        ConfigurationActivatedSubjectVoiceUrl: string
    }

    export class AppConfig implements IAppConfig {

        //Inactive
        public InactiveListSubjectUrl: string;
        public InactiveGetSubjectUrl: string;
        public InactiveGetExamSuiteUrl: string;
        public CreateInactiveSubjectUrl: string;
        public UpdateInactiveSubjectUrl: string;
        public DeleteInactiveSubjectUrl: string;

        //Activated
        public ActivatedListSubjectUrl: string;
        public ActivatedGetSubjectUrl: string;
        public ActivatedGetExamSuiteUrl: string;

        //QuestionInActive
        public QuestionInActiveConsiderQuestionUrl: string;
        public QuestionInActiveEditQuestionUrl: string;

        //QuestionActivated
        public QuestionActivatedEditQuestionUrl: string;

        //ConfigurationInActive
        public ConfigurationInActiveActivateSubjectUrl: string;
        public ConfigurationInActiveAddExamSuiteGruopUrl: string;
        public ConfigurationInActiveDeleteExamSuiteGruopUrl: string;
        public ConfigurationInActiveEditExamSuiteGroupUrl: string;
        public ConfigurationInActiveEditExamSuiteGroupMapUrl: string;
        public ConfigurationInActiveDeleteExamSuiteUrl: string;
        public CreateExamSuiteUrl: string;
        public UpdateExamSuiteUrl: string;
        public DeleteExamSuiteUrl: string;
        public CreateQuestionUrl: string;
        public UpdateQuestionUrl: string;
        public DeleteQuestionUrl: string;

        //ConfigurationActivated
        public ConfigurationActivatedReversionSubjectUrl: string;
        public ConfigurationActivatedDisableSubjectUrl: string;
        public ConfigurationActivatedEnableSubjectUrl: string;
        public ConfigurationActivatedEditExamSuiteGruopUrl: string;
        public ConfigurationActivatedEditExamSuiteGroupMapUrl: string;
        public ConfigurationActivatedAddExamSuiteGruopUrl: string;
        public ConfigurationActivatedDeleteExamSuiteGruopUrl: string;
        public ConfigurationActivatedSubjectVoiceUrl: string

        static $inject = ['defaultUrl'];
        constructor(defaultUrl: string) {

            //Inactive
            this.InactiveListSubjectUrl = defaultUrl + '/api/InActive/ListSubject';
            this.InactiveGetSubjectUrl = defaultUrl + '/api/InActive/GetSubject/:subjectid';
            this.InactiveGetExamSuiteUrl = defaultUrl + '/api/InActive/GetExamSuite/:examsuiteid';
            this.CreateInactiveSubjectUrl = defaultUrl + '/api/InActive/CreateInactiveSubject';
            this.UpdateInactiveSubjectUrl = defaultUrl + '/api/InActive/UpdateInactiveSubject';
            this.DeleteInactiveSubjectUrl = defaultUrl + '/api/InActive/DeleteInactiveSubject/:subjectid';

            //Activated
            this.ActivatedListSubjectUrl = defaultUrl + '/api/Activated/ListSubject/:siteid';
            this.ActivatedGetSubjectUrl = defaultUrl + '/api/Activated/GetSubject/:subjectid';
            this.ActivatedGetExamSuiteUrl = defaultUrl + '/api/Activated/GetExamsuite/:subjectid/:examsuiteid';

            //QuestionInActive
            this.QuestionInActiveConsiderQuestionUrl = defaultUrl + '/api/QuestionInActive/ConsiderQuestion/:titleCode/:questionNumber';
            this.QuestionInActiveEditQuestionUrl = defaultUrl + '/api/QuestionInActive/EditQuestion/:titleCode/:questionId';

            //QuestionActivated
            this.QuestionActivatedEditQuestionUrl = defaultUrl + '/api/QuestionActivated/EditQuestion/:questionId';

            //ConfigurationInActive
            this.ConfigurationInActiveActivateSubjectUrl = defaultUrl + '/api/ConfigurationInActive/ActivateSubject/:subjectId/:activateOption';
            this.ConfigurationInActiveAddExamSuiteGruopUrl = defaultUrl + '/api/ConfigurationInActive/AddExamSuiteGroup/:subjectId';
            this.ConfigurationInActiveDeleteExamSuiteGruopUrl = defaultUrl + '/api/ConfigurationInActive/DeleteExamSuiteGroup/:subjectId/:examsuitegroupid';
            this.ConfigurationInActiveEditExamSuiteGroupUrl = defaultUrl + '/api/ConfigurationInActive/EditExamSuiteGroup/:subjectId/:examsuitegroupId';
            this.ConfigurationInActiveEditExamSuiteGroupMapUrl = defaultUrl + '/api/ConfigurationInActive/EditExamsuiteGroupMap/:subjectId/:examsuiteGroupMapId';
            this.ConfigurationInActiveDeleteExamSuiteUrl = defaultUrl + '/api/ConfigurationInActive/DeleteExamSuite/:examsuiteid';
            this.CreateExamSuiteUrl = defaultUrl + '/api/QuestionInActive/CreateExamSuite';
            this.UpdateExamSuiteUrl = defaultUrl + '/api/QuestionInActive/UpdateExamSuite';
            this.DeleteExamSuiteUrl = defaultUrl + '/api/QuestionInActive/DeleteExamSuite/:examsuiteid';
            this.CreateQuestionUrl = defaultUrl + '/api/QuestionInActive/CreateQuestion';
            this.UpdateQuestionUrl = defaultUrl + '/api/QuestionInActive/UpdateQuestion';
            this.DeleteQuestionUrl = defaultUrl + '/api/QuestionInActive/DeleteQuestion/:examsuiteid/:questionid';

            //ConfigurationActivated
            this.ConfigurationActivatedReversionSubjectUrl = defaultUrl + '/api/ConfigurationActivated/ReversionSubject/:subjectCode/:subjectId';
            this.ConfigurationActivatedDisableSubjectUrl = defaultUrl + '/api/ConfigurationActivated/DisableSubject/:subjectId';
            this.ConfigurationActivatedEnableSubjectUrl = defaultUrl + '/api/ConfigurationActivated/EnableSubject/:subjectId';
            this.ConfigurationActivatedSubjectVoiceUrl = defaultUrl + '/api/ConfigurationActivated/ActivedSubjectVoice/:subjectId';
            this.ConfigurationActivatedAddExamSuiteGruopUrl = defaultUrl + '/api/ConfigurationActivated/AddExamSuiteGroup/:subjectId';
            this.ConfigurationActivatedDeleteExamSuiteGruopUrl = defaultUrl + '/api/ConfigurationActivated/DeleteExamSuiteGroup/:subjectId/:examsuitegroupid';
            this.ConfigurationActivatedEditExamSuiteGruopUrl = defaultUrl + '/api/ConfigurationActivated/EditExamSuiteGroup/:subjectId/:examsuitegroupId';
            this.ConfigurationActivatedEditExamSuiteGroupMapUrl = defaultUrl + '/api/ConfigurationActivated/EditExamsuiteGroupMap/:subjectId/:examsuiteGroupMapId';
        }
    }
    
    // HACK: Change the host Url
    angular
        .module('app')
        //.constant('defaultUrl', 'http://eexambankex.azurewebsites.net')
        //.constant('defaultUrl', 'http://localhost:50273')
        .constant('defaultUrl', 'http://http://10.10.2.251:8080')
        //.constant('defaultUrl', 'http://150.95.27.173/DSD#!/')
        //.constant('defaultUrl', 'http://150.95.27.173:80')
        .service('appConfig', AppConfig);
}