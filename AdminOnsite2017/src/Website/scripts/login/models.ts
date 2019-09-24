module application.loginApp {
    'use strict';

    export class Center {
        constructor(
            public _id: string,
            public NameTh: string,
            public NameEn: string,
            public CertDatas: CertData[],
            public SiteName: string,
            public SiteCode: string,
            public LogoPath: string
        ) { }
    }
    export class CertData {
        constructor(
            public CertNo: string,
            public CertYear: string,
            public UserCode: string
        ) { }
    }
    export class UserRequest {
        constructor(
            public User: string,
            public Pass: string,
            public CenterId: string
        ) { }
    }
    export class MessageRespone {
        constructor(
            public Code: string,
            public Message: string
        ) { }
    }
}