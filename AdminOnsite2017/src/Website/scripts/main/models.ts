module application.mainApp {
    'use strict';

    export class MainVM {
        constructor(
            public TestregisCount: number,
            public ActiveThruDateTime: Date,
            public IsExamEnough: boolean
        ) { }
    }
    export class ActiveRequest {
        constructor(
            public ActiveThruDateTime: Date,
            public CenterId: string
        ) { }
    }

    
}