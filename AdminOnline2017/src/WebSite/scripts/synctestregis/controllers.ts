module application.syncTestRegisApp {
    'use strict';

    class syncTestRegisController {
        public _syncTestRegisVM: SyncTestRegisVM;
        public approvedRegis: shared.TestRegistration[];
        public missRegis: shared.TestRegistration[];
        public failRegis: shared.TestRegistration[];
        public prepareData: boolean;
        public failToLoad: boolean;
        public toDay = new Date();
        public minDate: Date;
        public ApproveLanguage: string;
        public ApproveVoice: string;
        public ApproveDate: Date;
        public FailLanguage: string;
        public FailVoice: string;
        public FailDate: Date;
        public MissLanguage: string;
        public MissVoice: string;
        public MissDate: Date;

        public showAlertOkBtn: boolean = false;
        public alertMessage: string;

        static $inject = ['$scope', 'application.syncTestRegisApp.syncTestRegisService'];
        constructor(private $scope, private syncTestRegisSvc: application.syncTestRegisApp.syncTestRegisService) {
            this.FilterData();
            this.minDate = this.toDay;
        }
        private FilterData(): void {
            this.syncTestRegisSvc.syncTestRegisListTestRegisForApproved().then(re => {
                this._syncTestRegisVM = re;
                this.approvedRegis = this._syncTestRegisVM.TestRegistrations.filter(reg => reg.Status == "APPROVED");
                this.missRegis = this._syncTestRegisVM.TestRegistrations.filter(reg => reg.Status == "MISS");
                this.failRegis = this._syncTestRegisVM.TestRegistrations.filter(reg => reg.Status == "FAIL");
                this.prepareData = true;
            }, error => {
                console.log("Fail to loading data.");
                this.prepareData = true;
                this.failToLoad = true;
            });
        }

        public checkAllCheckBox(status: boolean, fromObj: string) {
            if (fromObj == "APPROVED") {
                this.approvedRegis.forEach(res => res.Checking = status);
            }
            else if (fromObj == "MISS") {
                this.missRegis.forEach(res => res.Checking = status);
            }
            else if (fromObj == "FAIL") {
                this.failRegis.forEach(res => res.Checking = status);
            }
        }

        public checkAtLeastOne(from: string): number {
            if (this._syncTestRegisVM == null) { return; }
            if (from == 'approved') { return this.approvedRegis.filter(it => it.Checking).length; }
            else if (from == 'miss') { return this.missRegis.filter(it => it.Checking).length; }
            else if (from == 'fail') { return this.failRegis.filter(it => it.Checking).length; }
        }

        public SubmitTestRegis(type: string) {
            this.prepareData = false;

            this.showAlertOkBtn = false;
            this.alertMessage = "กำลังนัดสอบ ...";

            if (type == 'approved') {
                var testRegistrations = this.approvedRegis.filter(testRegis => testRegis.Checking == true);
                testRegistrations.forEach(testRegis => { testRegis.ExamLanguage = this.ApproveLanguage, testRegis.VoiceLanguage = this.ApproveVoice, testRegis.AppointDate = this.ApproveDate });
                this.syncTestRegisSvc.syncTestRegisSubmitTestRegis(testRegistrations).then(re => {
                    var _sendJson: SyncTestRegisVM = re;
                    for (var i = 0; i < testRegistrations.length; i++) {
                        this.approvedRegis = this.approvedRegis.filter(reg => reg._id != testRegistrations[i]._id);
                    }
                    this.syncTestRegisSvc.syncTestRegisCreateTestRegistration(_sendJson).then(re2 => {
                        this.alertMessage = "นัดสอบสำเร็จ";
                        this.showAlertOkBtn = true;
                        this.prepareData = true;
                    });
                });
            }
            else if (type == 'miss') {
                var testRegistrations = this.missRegis.filter(x => x.Checking == true);
                testRegistrations.forEach(testRegis => { testRegis.ExamLanguage = this.MissLanguage, testRegis.VoiceLanguage = this.MissVoice, testRegis.AppointDate = this.MissDate });
                this.syncTestRegisSvc.syncTestRegisSubmitTestRegis(testRegistrations).then(re => {
                    var _sendJson: SyncTestRegisVM = re;
                    for (var i = 0; i < testRegistrations.length; i++) {
                        this.missRegis = this.missRegis.filter(reg => reg._id != testRegistrations[i]._id);
                    }
                    this.syncTestRegisSvc.syncTestRegisCreateTestRegistration(_sendJson).then(re2 => {
                        this.alertMessage = "นัดสอบสำเร็จ";
                        this.showAlertOkBtn = true;
                        this.prepareData = true;
                    });
                });
            }
            else if (type == 'fail') {
                var testRegistrations = this.failRegis.filter(x => x.Checking == true);
                testRegistrations.forEach(testRegis => { testRegis.ExamLanguage = this.FailLanguage, testRegis.VoiceLanguage = this.FailVoice, testRegis.AppointDate = this.FailDate });
                this.syncTestRegisSvc.syncTestRegisSubmitTestRegis(testRegistrations).then(re => {
                    var _sendJson: SyncTestRegisVM = re;
                    for (var i = 0; i < testRegistrations.length; i++) {
                        this.failRegis = this.failRegis.filter(reg => reg._id != testRegistrations[i]._id);
                    }
                    this.syncTestRegisSvc.syncTestRegisCreateTestRegistration(_sendJson).then(re2 => {
                        this.alertMessage = "นัดสอบสำเร็จ";
                        this.showAlertOkBtn = true;
                        this.prepareData = true;
                    });
                });
            }
        }
    }

    angular
        .module('application.syncTestRegisApp')
        .controller('application.syncTestRegisApp.syncTestRegisController', syncTestRegisController);
}