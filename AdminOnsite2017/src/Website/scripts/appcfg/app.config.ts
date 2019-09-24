module application {
    'use strict';

    export interface IAppConfig {
        //Login
        LoginUrl: string;
        LoginGetCenterDataUrl: string;//use for main
        //Main
        MainGetMainInfoUrl: string;
        MainAcitveUrl: string;
        MainCancelTestUrl: string;
        MainEndTestUrl: string;
        MainResumeTestUrl: string;
        MainCloseExamDataUrl: string;
        //Mainpage
        MainpageListTestregistrationUrl: string;
        MainpageSearchTestRegisUrl: string;
        MainpageChangeUserLanguage: string;
        //TestRegis
        TestingListTestingUrl: string;
        //PrintQR
        PrintqrGetInfoForPrintQRUrl: string;
        //Result
        ResultGetResultUrl: string;
        //ExamData
        ExamDataListExamDataUrl: string;
        ExamDataDownloadUrl: string;
        //Shared
        GetTestResultUrl: string;
        //logo
        LogoUrl: string;
        ExamPicUrl: string;
    }

    export class AppConfig implements IAppConfig {
        //Login
        public LoginUrl: string;
        public LoginGetCenterDataUrl: string;
        //Main
        public MainGetMainInfoUrl: string;
        public MainAcitveUrl: string;
        public MainCancelTestUrl: string;
        public MainEndTestUrl: string;
        public MainResumeTestUrl: string;
        public MainCloseExamDataUrl: string;
        //Mainpage
        public MainpageListTestregistrationUrl: string;
        public MainpageSearchTestRegisUrl: string;
        public MainpageChangeUserLanguage: string;
        //TestRegis
        public TestingListTestingUrl: string;
        //PrintQR
        public PrintqrGetInfoForPrintQRUrl: string;
        //Result
        public ResultGetResultUrl: string;
        //ExamData
        public ExamDataListExamDataUrl: string;
        public ExamDataDownloadUrl: string;
        //Shared
        public GetTestResultUrl: string;
        //Logo
        public LogoUrl: string;
        public ExamPicUrl: string;

        static $inject = ['defaultUrl', 'sharedUrl', 'localip'];
        constructor(defaultUrl: string, sharedUrl: string, localip: string) {
            //Login
            this.LoginUrl = defaultUrl + '/api/Onsite/Login';
            this.LoginGetCenterDataUrl = defaultUrl + '/api/Onsite/GetCenterData';
            //Main
            this.MainGetMainInfoUrl = defaultUrl + '/api/Onsite/GetMainInfo/:centerid';
            this.MainAcitveUrl = defaultUrl + '/api/Onsite/Active';
            this.MainCancelTestUrl = defaultUrl + '/api/Onsite/Cancel';
            this.MainEndTestUrl = defaultUrl + '/api/Onsite/EndTest';
            this.MainResumeTestUrl = defaultUrl + '/api/Onsite/Resume';
            this.MainCloseExamDataUrl = defaultUrl + '/api/Onsite/CloseExamData';
            //Mainpage
            this.MainpageListTestregistrationUrl = defaultUrl + '/api/Onsite/ListTesrRegistration/:centerid';
            this.MainpageSearchTestRegisUrl = defaultUrl + '/api/Onsite/SearchTestRegis/:txt/:centerid';
            this.MainpageChangeUserLanguage = '/api/Onsite/ChangeLanguage';
            //TestRegis
            this.TestingListTestingUrl = defaultUrl + '/api/Onsite/ListTesting/:centerid';
            //PrintQR
            this.PrintqrGetInfoForPrintQRUrl = defaultUrl + '/api/Onsite/GetInfoForPrintQR/:pid';
            //Result
            this.ResultGetResultUrl = defaultUrl + '/api/Onsite/GetResult/:pid';
            //ExamData
            this.ExamDataListExamDataUrl = defaultUrl + '/api/Onsite/ListExamData/:centerid';
            this.ExamDataDownloadUrl = defaultUrl + '/api/Onsite/Download';
            //Shared
            this.GetTestResultUrl = defaultUrl + '/api/Onsite/GetTestResult/:pid/:sheetid';
            this.LogoUrl = localip + '/examconfig/logo.png';
            this.ExamPicUrl = localip + '/exampic';
        }
    }
    angular
        .module('application')
        //.constant('defaultUrl', 'http://192.168.0.99:9090')
        .constant('defaultUrl', 'http://192.168.9.99:8080')
        //.constant('defaultUrl', 'http://192.168.5.88:8080')
        //.constant('defaultUrl', 'http://localhost:9143')
        //.constant('localip', 'http://10.93.77.199')
        //.constant('localip', 'http://192.168.5.88')
        .constant('localip', 'http://192.168.9.99')
        //.constant('sharedUrl', 'http://10.93.77.199/localdb')
        .constant('sharedUrl', 'http://192.168.9.99/localdb')
        //.constant('sharedUrl', 'http://192.168.5.88/localdb')
        //.constant('sharedUrl', 'http://localhost:9143/')
        .service('appConfig', AppConfig);
}