module application.testBankApp {
    'use strict';

    class testBankController {
        public _sendJson: TestBankVM;
        public _syncTestRegisVM: TestBankVM;
        public _subjects: SubjectList;
        public prepareData: boolean;
        public failToLoad: boolean;
        public testing: string[];

        public showAlertOkBtn: boolean = false;
        public alertMessage: string;

        static $inject = ['$scope', 'application.testBankApp.testBankService'];
        constructor(private $scope, private testBankSvc: application.testBankApp.testBankService) {
            var prepareExamSheet: boolean;
            var prepareSubject: boolean;
            this.testBankSvc.testBankListSubject().then(re => {
                this._subjects = re;
                console.log(re);
                this.testBankSvc.syncTestRegisListExamSheet().then(re => {
                    this._syncTestRegisVM = re;
                    this.prepareData = true;
                }, error => {
                    console.log("Fail to loading ExamSheet");
                    this.prepareData = true;
                    this.failToLoad = true;
                });
            }, error => {
                console.log("Fail to loading ListSubjec");
                this.prepareData = true;
                this.failToLoad = true;
            });
        }
        public _subjectGroupsFilter(Oc: string): shared.SubjectGroup[] {
            return this._subjects.SubjectGroups.filter(re => re.id == Oc);
        }
        public SubmitTestRegis(subject: string, language: string, voice: string, quatity: number) {
            this.prepareData = false;
            var newQuatity: number;
            var version = this._subjects.SubjList.filter(re => re.SubjectCode == subject)[0].Version;

            this.showAlertOkBtn = false;
            this.alertMessage = "กำลังดาวน์โหลดข้อสอบ ...";

            //------------------------Check version------------------------------------
            this.testBankSvc.CheckSubjectVersion(subject, version).then(re => {
                console.log(quatity);
                //console.log(re.Code);
                //newQuatity = quatity + parseInt(re.Code);
                //------------------------Submit ExamSheet------------------------------------
                this.testBankSvc.syncTestRegisSubmitExamSheet(subject, language, voice, quatity).then(re => {
                    this._sendJson = re;
                    if (re == null) { console.log("result is null"); return; }
                    //------------------------Create examsheet------------------------------------
                    this.testBankSvc.syncTestCreateexamsheet(this._sendJson).then(() => {
                        //------------------------Refresh------------------------------------
                        this.testBankSvc.syncTestRegisListExamSheet().then(re => {
                            this._syncTestRegisVM = re;
                            this.alertMessage = "ดาวน์โหลดข้อสอบสำเร็จ";
                            this.showAlertOkBtn = true;
                            this.prepareData = true;
                        }, error => {
                            console.log("Fail to loading Examsheet");
                            this.prepareData = true;
                            this.failToLoad = true;
                        });
                    }, error => {
                        console.log("Fail to Createexamsheet");
                        this.prepareData = true;
                        this.failToLoad = true;
                    });
                }, error => {
                    console.log("Fail to SubmitExamSheet");
                    this.prepareData = true;
                    this.failToLoad = true;
                });
            }, error => {
                console.log("Fail to check version");
                this.prepareData = true;
                this.failToLoad = true;
            });
        }
    }

    angular
        .module('application.testBankApp')
        .controller('application.testBankApp.testBankController', testBankController);
}