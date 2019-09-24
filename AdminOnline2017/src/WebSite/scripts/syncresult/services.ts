module application.syncResultApp {
    'use strict';

    interface IListResultTestRegisResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }
    interface ISendToTirdPartyResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SendResult(data: T): T;
    }
    export class syncResultService {
        private syncresultListResultTestRegisSvc: IListResultTestRegisResourceClass<any>;
        private syncresultSendToTirdPartySvc: ISendToTirdPartyResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            //syncresultall
            this.syncresultListResultTestRegisSvc = <IListResultTestRegisResourceClass<any>>$resource(appConfig.ListResultTestRegisUrl, { 'centerid': '@centerid' }, { GetContent: { method: 'GET' } });
            this.syncresultSendToTirdPartySvc = <ISendToTirdPartyResourceClass<any>>$resource(appConfig.ListSendToTirdPartyUrl, null, { SendResult: { method: 'PUT' } });
        }

        public syncresultListResultTestRegis(): ng.IPromise<any> {
            // HACK: Fix centerid syncresult
            var centerid = "1";
            return this.syncresultListResultTestRegisSvc.query({ 'centerid': centerid }).$promise;
        }
        // TODO: implement syncResult
        public syncresultSendToTirdParty(testRegistrations: shared.TestRegistration[]): ng.IPromise<any> {
            console.log(testRegistrations[0].FirstName);
            // HACK: Fix centerid send to tird party
            return this.syncresultSendToTirdPartySvc.SendResult(testRegistrations).$promise;
        }
    }

    angular
        .module('application.syncResultApp')
        .service('application.syncResultApp.syncResultService', syncResultService);
}