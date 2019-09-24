module app.shared {
    'use strict';

    export class sharedService {
        public _centerData: shared.CenterDataRequest;
        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource) {
        }
        public setValue(centerData: shared.CenterDataRequest) {
            this._centerData = centerData;
        }
    }

    angular
        .module('app.shared')
        .service('app.shared.sharedService', sharedService);
}