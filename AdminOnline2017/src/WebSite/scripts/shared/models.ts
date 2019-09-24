module application.shared {
    'use strict';
    export class TestRegistration {
        constructor(
            public _id: string,
            public Title: string,
            public FirstName: string,
            public LastName: string,
            public SubjectCode: string,
            public SubjectName: string,
            public ExamLanguage: string,
            public VoiceLanguage: string,
            public RegDate: Date,
            public ExpiredDate: Date,
            public SiteId: string,
            public CenterId: string,
            public ForTestSystem: boolean,
            public ForPractice: boolean,
            public Status: string,
            public PID: string,
            public ExamNumber: string,
            public ExamStatus: string,
            public ExamPeriod: string,
            public AppointDate: Date,
            public MaxCount: number,
            public SheetId: string,
            public TestCount: string,
            public Checking: boolean
        ) { }
    }
    export class ExamSheet {
        constructor(
            public id: string,
            public SubjectCode: string,
            public SubjectName: string,
            public OccupationName: string,
            public ExamLanguage: string,
            public VoiceLanguage: string,
            public Quantity: number,
            public Book: number,
            public Version: string
        ) { }
    }
    export class ExamSheetForTestResult {
        constructor(
            public id: string,
            public LatestStatus: string,
            public TestCount: number,
            public Score: number,
            public TestRegis: TestRegistration,
            public ExamDateTime: Date
        ) { }
    }
    export class ExamSheetRequest {
        constructor(
            public SubjectCode: string,
            public ExamLanguage: string,
            public VoiceLanguage: string,
            public Quatity: number,
            public CenterId: string
        ) { }
    }
    export class SubjectDetail {
        constructor(
            public id: string,
            public SubjectCode: string,
            public SubjectName: string,
            public ExamSuiteCount: number,
            public QuestionCount: number,
            public Version: string,
            public IsDisabled: boolean,
            public SubjectGroupId: string
        ) { }
    }
    export class Occupation {
        constructor(
            public id: string,
            public Name: string,
            public SubjectGroups: SubjectGroup[]
        ) { }
    }
    export class SubjectGroup {
        constructor(
            public id: string,
            public Name: string,
            public OccupationId: string
        ) { }
    }
    export class Subject {
        constructor(
            public id: string,
            public Name: string,
            public Code: string
        ) { }
    }
    export class LanguageSource {
        constructor(
            public SubjectCode: string,
            public Detail: string,
            public Code: string
        ) { }
    }
    export class VoiceSource {
        constructor(
            public SubjectCode: string,
            public Detail: string,
            public Code: string
        ) { }
    }

    export class CenterDataRequest {
        constructor(
            public _id: string,
            public NameTh: string,
            public NameEn: string,
            public CertDatas: CertData[],
            public Address: string,
            public Mobile: string,
            public LatestUser: string,
            public LatestPass: string,
            public UpdateDateTime: Date,
            public SiteName: string,
            public SiteId: string,
            public IsAutoSyncScore: boolean,
            public IsAutoSyncResult: boolean,
            public IsShowAnswer: boolean
        ) { }
    }

    export class CertData {
        constructor(
            public UserCode: string,
            public CertNo: string,
            public CertYear: string
        ) { }
    }
}