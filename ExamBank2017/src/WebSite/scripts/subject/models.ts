module app.subjectApp {
    'use strict';

    export class Subject {
        constructor(
            public id: string,
            public SubjectCode: string,
            public SubjectName: string,
            public Version: string,
            public IsDisabled: boolean,
            public ContentLanguage: string,
            public ExamSuites: ExamSuiteDetail[],
            public ExamSuiteGroups: ExamSuiteGroup[],
            public VersionList: SubjectVersion[],
            public VoiceLanguageList: VoiceLanguage[],
            public QuestionCount: number
        ) { }
    }

    export class ExamSuiteGroup {
        constructor(
            public id: string,
            public ExamSuiteGroupName: string,
            public IsUsed: boolean,
            public PassScore: string,
            public ExamDuration: string,
            public QuestionCount: string,
            public SumRandomCountCount: number,
            public ExamSuiteGroupMaps: ExamSuiteGroupMap[],
            public SubjectId: string
        ) { }
    }

    export class ExamSuiteGroupMap {
        constructor(
            public id: string,
            public ExamSuiteId: string,
            public RandomCount: string,
            public ExamSuiteGroupId: string,
            public SubjectId: string
        ) { }
    }

    export class ExamSuiteDetail {
        constructor(
            public id: string,
            public TitleCode: string,
            public TitleName: string,
            public QuestionCount: string,
            public ConsiderationStatus: string
        ) { }
    }

    export class SubjectVersion {
        constructor(
            public id: string,
            public CreateDateTime: Date,
            public VersionText: string,
            public IsUsed: boolean,
            public SubjectId: string
        ) { }
    }

    export class VoiceLanguage {
        constructor(
            public id: string,
            public Language: string,
            public LanguageCode: string,
            public IsUsed: boolean
        ) { }
    }

    export class InactiveApiResponse {
        constructor(
            public Message: string,
            public Code: string
        ) { }
    }
}