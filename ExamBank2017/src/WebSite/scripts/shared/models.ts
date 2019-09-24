module app.shared {
    'use strict';

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