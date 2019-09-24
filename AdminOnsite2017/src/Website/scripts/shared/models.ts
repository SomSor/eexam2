module application.shared {
    'use strict';
    export class TestRegistrationRespone {
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
            public ExamStatus: string,
            public PID: string,
            public ExamNumber: string,
            public ExamPeriod: string,
            public AppointDate: Date,
            public Address: string,
            public MaxCount: number,
            public LatestCount: number,
            public EndExamThruTime: Date,
            public SheetId: string,
            public IsSync: boolean,
            public CertData: CertData,
            public UserCode: string,
            public CertNo: string,
            public CertYear: string
        ) { }
    }
    export class ExamSheetOnSiteRespone {
        constructor(
            public SubjectCode: string,
            public SubjectName: string,
            public ExamLanguage: string,
            public VoiceLanguage: string,
            public Quantity: number,
            public Book: number,
            public Version: string,
            public IsExamEnough: boolean
        ) { }
    }
    export class Result {
        constructor(
            public _id: string,
            public Title: string,
            public Firstname: string,
            public LastName: string,
            public SubjectCode: string,
            public SubjectName: string,
            public Status: string,
            public TestCount: number,
            public PID: string,
            public ExamNumber: string,
            public CorrectCount: number,
            public InCorrectCount: number,
            public ExamDateTime: Date,
            public CenterNameTH: string
        ) { }
    }
    export class CertData {
        constructor(
            public UserCode: string,
            public CertNo: string,
            public CertYear: string
        ) { }
    }
    export class ActionSheetRequest {
        constructor(
            public pid: string,
            public sheetId: string
        ) { }
    }
}