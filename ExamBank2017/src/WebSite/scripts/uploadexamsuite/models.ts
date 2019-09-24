module app.uploadExamSuiteApp {
    'use strict';

    export class UploadExamSuite {
        constructor(
            public TempFileName: string,
            public OccupationGroupName: string,
            public SubjectGroupName: string,
            public SubjectCode: string,
            public SubjectName: string,
            public TitleCode: string,
            public TitleName: string,
            public QuestionCount: string,
            public Content: string,
            public Language: string
        ) { }
    }
}