module application.testResult {
    'use strict';

    interface IGetTestResultByDateResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetTestResultByDate(data: T): T;
    }
    export class testResultService {
        private testResultGetTestResultByDateSvc: IGetTestResultByDateResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'application.shared.sharedService'];
        constructor(appConfig: IAppConfig, $resource, private sharedSVC: application.shared.sharedService) {
            this.testResultGetTestResultByDateSvc = <IGetTestResultByDateResourceClass<any>>$resource(appConfig.GetTestResultByDateUrl, { 'centerid': '@centerid', 'testdate': '@testdate' }, { GetTestResultByDate: { method: 'GET' } });
        }
        public GetTestResultByDate(testdate: Date): ng.IPromise<any> {
            var centerid = this.sharedSVC._centerData._id;
            var dateStr = testdate.getFullYear() + "-" + (testdate.getMonth() + 1) + "-" + testdate.getDate();
            return this.testResultGetTestResultByDateSvc.get({ 'centerid': centerid, 'testdate': dateStr }).$promise;
        }
    }

    angular
        .module('application.testResult')
        .service('application.testResult.testResultService', testResultService);
}