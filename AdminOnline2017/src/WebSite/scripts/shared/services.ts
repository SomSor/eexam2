module application.shared {
    'use strict';

    interface IUploadDataResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        UploadData(data: T): T;
    }
    export class sharedService {
        private sharedUploadDataSvc: IUploadDataResourceClass<any>;
        public _centerData: shared.CenterDataRequest;
        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
            //start
            this.sharedUploadDataSvc = <IUploadDataResourceClass<any>>$resource(appConfig.UploadDataUrl, null, { UploadData: { method: 'POST' } });
        }

        public setValue(centerData: shared.CenterDataRequest) {
            this._centerData = centerData;
        }

        public UploadData(): ng.IPromise<any> {
            return this.sharedUploadDataSvc.UploadData(this._centerData).$promise;
        }
    }

    angular
        .module('application.shared')
        .service('application.shared.sharedService', sharedService);
}