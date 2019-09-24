module app.subjectlistApp {
    'use strict';

    export class SubjectList {
        constructor(
            public SubjList: SubjectDetail[],
            public Occupations: Occupation[],
            public SubjectGroups: SubjectGroup[]
        ) { }
    }

    export class SubjectDetail {
        constructor(
            public id: string,
            public SubjectCode: string,
            public SubjectName: string,
            public ContentLanguage: string,
            public ExamSuiteCount: string,
            public QuestionCount: string,
            public ExamSuiteAcceptCount: string,
            public ExamSuiteRejectCount: string,
            public Version: string,
            public IsDisabled: boolean,
            public SubjectGroupId: string
        ) { }
    }

    export class Occupation {
        constructor(
            public id: string,
            public Name: string
        ) { }
    }

    export class SubjectGroup {
        constructor(
            public id: string,
            public Name: string,
            public OccupationId: string
        ) { }
    }
}