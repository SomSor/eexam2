module app.examSuiteApp {
    'use strict';

    export class ExamSuite {
        constructor(
            public id: string,
            public SubjectCode: string,
            public SubjectName: string,
            public TitleCode: string,
            public TitleName: string,
            public SubjectId: string,
            public Questions: Question[]
            //public QuestionGroups: QuestionGroup[]
        ) { }
    }

    export class Question {
        constructor(
            public id: string,
            public QuestionNumber: string,
            public IsAllowRandomChoice: boolean,
            public Detail: string,
            public Choices: Choice[],
            public Considerations: Consideration[],
            public Voices: VoiceSource[],
            public ExamSuiteId: string,
            public TitleCode: string
        ) { }
    }

    export class Choice {
        constructor(
            public id: string,
            public Detail: string,
            public IsCorrect: boolean,
            public Voices: VoiceSource[]
        ) { }
    }

    export class Consideration {
        constructor(
            public id: string,
            public CreateDateTime: Date,
            public RejectComment: string,
            public IsAccept: boolean,
            public UserName: string,
            public ExamSuiteId: string,
            public TitleCode: string,
            public QuestionNumber: string
        ) { }
    }

    export class VoiceSource {
        constructor(
            public id: string,
            public Language: string,
            public URL: string
        ) { }
    }
}