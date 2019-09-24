module application.syncTestRegisApp {
    'use strict';
    export class SyncTestRegisVM {
        constructor(
            public TestRegistrations: shared.TestRegistration[],
            public LanguageSource: shared.LanguageSource[],
            public json: string
        ) { }
    }
}