module application.mainApp {
    'use strict';

    export class MainVM {
        constructor(
            public Testregistrations: shared.TestRegistration[],
            public AppointDates: Date[]
        ) { }
    }
}