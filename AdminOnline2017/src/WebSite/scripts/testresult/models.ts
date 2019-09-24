module application.testResult {
    'use strict';
    export class TestResultVM {
        constructor(
            public id: string,
            public totalsTest: number,
            public totalsTestPerson: number,
            public totalsPassTest: number,
            public totalsFailTest: number,
            public percentagePassTest: number,
            public percentageFailTest: number,
            public normalTest: number,
            public retest: number,
            public centerName: string,
            public examsheets: shared.ExamSheetForTestResult[]
        ) { }
    }
}