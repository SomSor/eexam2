module application.exambankApp {
    'use strict';

    export class ExamDataVM {
        constructor(
            public ExamSheets: shared.ExamSheetOnSiteRespone[]
        ) { }
    }
    export class DownloadRequest {
        constructor(
            public SubjectCode: string,
            public ExamLanguage: string,
            public VoiceLanguage: string,
            public Quantity: number,
            public CenterId: string
        ) { }
    }


}   