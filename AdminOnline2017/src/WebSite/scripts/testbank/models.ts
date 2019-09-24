module application.testBankApp {
    'use strict';
    export class TestBankVM {
        constructor(
            public ExamSheets: shared.ExamSheet[],
            public json: string
        ) { }
    }
    export class SubjectList {
        constructor(
            public SubjList: shared.SubjectDetail[],
            public Occupations: shared.Occupation[],
            public SubjectGroups: shared.SubjectGroup[],
            public LanguageSources: shared.LanguageSource[],
            public VoiceSources: shared.VoiceSource[]
        ) { }
    }
}