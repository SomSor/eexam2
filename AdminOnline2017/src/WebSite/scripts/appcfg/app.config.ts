module application {
    'use strict';

    export interface IAppConfig {
        //Main
        MainListAppointTestRegisByDateUrl: string;
        MainSerachAppointTestRegisUrl: string;

        //syncTestRegis
        SyncTestRegisListTestRegisForApprovedUrl: string;
        SyncTestRegisSubmitTestRegisUrl: string;

        //syncTestRegis
        TestBankListExamSheetUrl: string;
        TestBankSubmitExamSheetUrl: string;
        TestBankCheckSubjectVersion: string;

        //syncresult
        ListResultTestRegisUrl: string;
        ListSendToTirdPartyUrl: string;

        //testResult
        GetTestResultByDateUrl: string;

        //call to ExamBlank
        ListSubjectUrl: string;

        //call to 10.93.77.199
        CreateTestRegistration: string;
        CreateExamSheet: string;
        UploadDataUrl: string;

    }

    export class AppConfig implements IAppConfig {
        //Main
        public MainListAppointTestRegisByDateUrl: string;
        public MainSerachAppointTestRegisUrl: string;

        //syncTestRegis
        public SyncTestRegisListTestRegisForApprovedUrl: string;
        public SyncTestRegisSubmitTestRegisUrl: string;

        //syncTestRegis
        public TestBankListExamSheetUrl: string;
        public TestBankSubmitExamSheetUrl: string;
        public TestBankCheckSubjectVersion: string;

        //syncresult
        public ListResultTestRegisUrl: string;
        public ListSendToTirdPartyUrl: string;

        //testResult
        public GetTestResultByDateUrl: string;

        //call to ExamBlank
        public ListSubjectUrl: string;

        //call to 10.93.77.199
        public CreateTestRegistration: string;
        public CreateExamSheet: string;
        public UploadDataUrl: string;

        static $inject = ['defaultUrl', 'examBankUrl','calStaticURl'];
        constructor(defaultUrl: string, examBankUrl: string, calStaticURl: string) {
            //Main
            this.MainListAppointTestRegisByDateUrl = defaultUrl + '/api/TestRegistration/ListAppointTestRegisByDate/:centerid/:date';
            this.MainSerachAppointTestRegisUrl = defaultUrl + '/api/TestRegistration/SerachAppointTestRegis/:centerid/:filter';

            //syncTestRegis
            this.SyncTestRegisListTestRegisForApprovedUrl = defaultUrl + '/api/TestRegistration/ListTestRegisForApproved/:centerid';
            this.SyncTestRegisSubmitTestRegisUrl = defaultUrl + '/api/TestRegistration/SubmitTestRegis';
            this.CreateTestRegistration = calStaticURl + '/api/shared/CreateTestRegistration';

            //testBank
            //this.TestBankListExamSheetUrl = defaultUrl + '/api/ExamSheet/ListExamLanguage/:centerid';
            this.TestBankListExamSheetUrl = calStaticURl + '/api/shared/GetLocalExamInfo/:centerid';
            this.TestBankSubmitExamSheetUrl = defaultUrl + '/api/ExamSheet/SubmitExamSheet/:centerid/:subjectcode/:examlanguage/:voicelanguage/:quantity';
            this.CreateExamSheet = calStaticURl + '/api/shared/CreateExamSheet';
            this.TestBankCheckSubjectVersion = calStaticURl + '/api/shared/CheckSubjectVersion/:centerid/:subjectcode/:version';

            //syncresult
            this.ListResultTestRegisUrl = defaultUrl + '/api/TestRegistration/ListResultTestRegis/:centerid';
            this.ListSendToTirdPartyUrl = defaultUrl + '/api/ExamSheet/SendTo3ndParty';

            //testResult
            this.GetTestResultByDateUrl = defaultUrl + '/api/ExamSheet/ExamReport/:centerid/:testdate';

            //call to ExamBlank
            this.ListSubjectUrl = examBankUrl + '/api/Activated/ListSubject/:siteid';

            //shared
            this.UploadDataUrl= calStaticURl + "/api/Shared/LoginToLocalDB/";

        }
    }
    angular
        .module('application')
        .constant('defaultUrl', 'http://localhost:50659')
        //.constant('defaultUrl', 'http://eexamthaiex.azurewebsites.net')
        //.constant('examBankUrl', 'http://eexambankex.azurewebsites.net')
        .constant('examBankUrl', 'http://150.95.27.173:8080')
        //.constant('examBankUrl', 'http://localhost:50659')
        //.constant('calStaticURl', 'http://192.168.9.99/localdb')
        .constant('calStaticURl', 'http://150.95.27.173:8083')
        .service('appConfig', AppConfig);
}