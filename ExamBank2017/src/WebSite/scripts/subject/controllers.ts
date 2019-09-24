module app.subjectApp {
    'use strict';

    class inactiveSubjectViewController {
        public _subject: Subject;
        public _newExamSuiteGroup: ExamSuiteGroup;
        public _editExamSuiteGroup: ExamSuiteGroup;
        public _editExamSuiteGroupMap: ExamSuiteGroupMap;
        public _oldExamSuiteGroupMapRandomCount: string;
        public IsLoading: boolean = true;
        public responseMessage: string = "Loading ...";

        static $inject = ['$scope', 'app.subjectApp.subjectService', '$stateParams', '$state'];
        constructor(private $scope, private _subSvc: app.subjectApp.subjectService, private $stateParams, private $state) {
            this._subSvc.GetInactiveSubject(this.$stateParams.subjectid).then(sub => {
                this._subject = sub;

                var QuestionCount: number = 0;
                for (var i = 0; i < sub.ExamSuites.length; i++) {
                    QuestionCount += isNaN(sub.ExamSuites[i].QuestionCount) ? 0 : parseInt(sub.ExamSuites[i].QuestionCount);
                }
                this._subject.QuestionCount = QuestionCount;

                for (var i = 0; i < sub.ExamSuiteGroups.length; i++) {
                    var SumRandomCountCount: number = 0;
                    for (var j = 0; j < sub.ExamSuiteGroups[i].ExamSuiteGroupMaps.length; j++) {
                        SumRandomCountCount += isNaN(sub.ExamSuiteGroups[i].ExamSuiteGroupMaps[j].RandomCount) ? 0 : parseInt(sub.ExamSuiteGroups[i].ExamSuiteGroupMaps[j].RandomCount);
                    }
                    sub.ExamSuiteGroups[i].SumRandomCountCount = SumRandomCountCount;
                }
            });
        }

        public ActivateSubject(activateOption: string) {
            if (activateOption == null || activateOption == "") {
                this.responseMessage = "กรุณาเลือกวิธีการ Activate";
                this.IsLoading = false;
                return;
            }

            this.IsLoading = true;
            this.responseMessage = "Loading ...";
            this._subSvc.InactiveSubjectActiveSubject(this._subject.id, activateOption).then(result => {
                this.responseMessage = result.Message;
                this.IsLoading = false;
                if (result.Code == 0) {
                    this.$state.go('activatedsubjectlist');
                }
            }).catch(() => {
                this.responseMessage = "error";
                this.IsLoading = false;
            });
        }

        public EditExamSuiteGroupPreparation(examsuiteGroup: ExamSuiteGroup) {
            this._editExamSuiteGroup = new ExamSuiteGroup(
                examsuiteGroup.id,
                examsuiteGroup.ExamSuiteGroupName,
                examsuiteGroup.IsUsed,
                examsuiteGroup.PassScore,
                examsuiteGroup.ExamDuration,
                "", 0, null, ""
            );
        }

        public EditExamSuiteGroup(examsuiteGroup: ExamSuiteGroup) {
            var initIndex = 0;
            var editedExamsuiteGroup = this._subject.ExamSuiteGroups.filter(esg => esg.id == examsuiteGroup.id)[initIndex];
            editedExamsuiteGroup.SubjectId = this._subject.id;
            editedExamsuiteGroup.ExamSuiteGroupName = examsuiteGroup.ExamSuiteGroupName;
            editedExamsuiteGroup.PassScore = examsuiteGroup.PassScore;
            editedExamsuiteGroup.ExamDuration = examsuiteGroup.ExamDuration;
            editedExamsuiteGroup.IsUsed = examsuiteGroup.IsUsed;

            this._subSvc.InactiveSubjectEditExamSuiteGroup(editedExamsuiteGroup);
        }

        public EditExamSuiteGroupMapPreparation(examSuiteGroupMap: ExamSuiteGroupMap) {
            this._editExamSuiteGroupMap = new ExamSuiteGroupMap(
                examSuiteGroupMap.id,
                examSuiteGroupMap.ExamSuiteId,
                examSuiteGroupMap.RandomCount,
                examSuiteGroupMap.ExamSuiteGroupId,
                examSuiteGroupMap.SubjectId
            );
            this._oldExamSuiteGroupMapRandomCount = examSuiteGroupMap.RandomCount;
        }

        public EditExamSuiteGroupMap(examsuiteGroupMap: ExamSuiteGroupMap) {
            var initIndex = 0;
            var examSuiteGroup = this._subject.ExamSuiteGroups.filter(esg => esg.id == examsuiteGroupMap.ExamSuiteGroupId)[0];
            var editedExamSuiteGroupMap = examSuiteGroup.ExamSuiteGroupMaps.filter(esgm => esgm.id == examsuiteGroupMap.id)[initIndex];

            editedExamSuiteGroupMap.SubjectId = this._subject.id;
            editedExamSuiteGroupMap.RandomCount = examsuiteGroupMap.RandomCount;

            var SumRandomCountCount: number = 0;
            for (var i = 0; i < examSuiteGroup.ExamSuiteGroupMaps.length; i++) {
                SumRandomCountCount += parseInt(examSuiteGroup.ExamSuiteGroupMaps[i].RandomCount);
            }
            examSuiteGroup.SumRandomCountCount = SumRandomCountCount;

            this._subSvc.InactiveSubjectEditExamSuiteGroupMap(editedExamSuiteGroupMap);
        }

        public AddExamSuiteGroup() {
            if (this._subject.ExamSuiteGroups.filter(esg => esg.ExamSuiteGroupName.trim() == this._newExamSuiteGroup.ExamSuiteGroupName.trim()).length > 0) {
                alert("ไม่สามารถเพิ่มชุดข้อสอบชื่อซ้ำกันได้");
                return null;
            }

            var newId = "";
            var newExamGroup = new ExamSuiteGroup(
                newId,
                this._newExamSuiteGroup.ExamSuiteGroupName.trim(),
                this._newExamSuiteGroup.IsUsed,
                this._newExamSuiteGroup.PassScore,
                this._newExamSuiteGroup.ExamDuration,
                this._newExamSuiteGroup.QuestionCount,
                this._newExamSuiteGroup.SumRandomCountCount,
                this._newExamSuiteGroup.ExamSuiteGroupMaps,
                this._subject.id
            );
            this._subSvc.InactiveSubjectAddExamSuiteGroup(newExamGroup).then(esg => {
                if (esg == null) {
                    alert('เกิดข้อผิดพลาดระหว่างการเพิ่มชุดข้อสอบ');
                } else {
                    esg.SumRandomCountCount = 0;
                    esg.SubjectId = this._subject.id;

                    this._subject.ExamSuiteGroups.push(esg);
                }
            });
        }

        public DeleteExamSuiteGroup(): void {
            var initIndex = 0;
            var index = this._subject.ExamSuiteGroups.indexOf(this._subject.ExamSuiteGroups.filter(esg => esg.id == this._editExamSuiteGroup.id)[0]);
            this._subSvc.InactiveSubjectDeleteExamSuiteGroup(this._subject.id, this._editExamSuiteGroup.id).then(() => {
                this._subject.ExamSuiteGroups.splice(index, 1);
            });
        }

        private GetExamSuiteTitleCode(examsuiteGroupMap: ExamSuiteGroupMap): string {
            return this._subject.ExamSuites.filter(es => es.id == examsuiteGroupMap.ExamSuiteId)[0].TitleCode;
        }

        private GetExamSuiteTitleName(examsuiteGroupMap: ExamSuiteGroupMap): string {
            return this._subject.ExamSuites.filter(es => es.id == examsuiteGroupMap.ExamSuiteId)[0].TitleName;
        }

        private GetExamSuiteQuestionCount(examsuiteGroupMap: ExamSuiteGroupMap): string {
            return this._subject.ExamSuites.filter(it => it.id == examsuiteGroupMap.ExamSuiteId)[0].QuestionCount;
        }
    }

    class activatedSubjectViewController {
        public _subject: Subject;
        public _activatedSubjectId: string;
        public _newExamSuiteGroup: ExamSuiteGroup;
        public _editExamSuiteGroup: ExamSuiteGroup;
        public _editExamSuiteGroupMap: ExamSuiteGroupMap;
        public _oldExamSuiteGroupMapRandomCount: string;
        public _editVoices: VoiceLanguage[];

        static $inject = ['$scope', 'app.subjectApp.subjectService', '$stateParams', '$state'];
        constructor(private $scope, private _subSvc: app.subjectApp.subjectService, private $stateParams, private $state) {
            this._subSvc.GetActivatedSubject(this.$stateParams.subjectid).then(sub => {
                this._subject = sub;
                this.PrepareSubject(sub);
            });
        }

        public ReversionSubject() {
            this._subSvc.ActivatedSubjectReversionSubject(this._subject.SubjectCode, this._activatedSubjectId).then(() => {
                this.$state.go('activatedsubjectview', { 'subjectid': this._activatedSubjectId });
            });
        }

        public BackReversionSubject() {
            this._activatedSubjectId = this._subject.id;
        }

        public DisableSubject() {
            this._subSvc.ActivatedSubjectDisableSubject(this._subject.id).then(() => {
                this._subject.IsDisabled = true;
            });;
        }

        public EnableSubject() {
            this._subSvc.ActivatedSubjectEnableSubject(this._subject.id).then(() => {
                this._subject.IsDisabled = false;
            });;
        }

        public ActivedSubjectVoice() {
            var subject = new Subject(this._subject.id, null, null, null, this._subject.IsDisabled, null, null, null, null, this._editVoices, this._subject.QuestionCount);
            this._subSvc.ActivatedSubjectVoices(subject);
        }

        public CancelChangedVoiceLanguage() {
            this._editVoices = [];
            for (var voice in this._subject.VoiceLanguageList) {
                //this._editVoices.push(new VoiceLanguage(voice.id, voice.Language, voice.LanguageCode, voice.IsUsed));
            }
        }

        public EditExamSuiteGroupPreparation(examsuiteGroup: ExamSuiteGroup) {
            this._editExamSuiteGroup = new ExamSuiteGroup(
                examsuiteGroup.id,
                examsuiteGroup.ExamSuiteGroupName,
                examsuiteGroup.IsUsed,
                examsuiteGroup.PassScore,
                examsuiteGroup.ExamDuration,
                "", 0, null, ""
            );
        }

        public EditExamSuiteGroup(examsuiteGroup: ExamSuiteGroup) {
            var initIndex = 0;
            var editedExamsuiteGroup = this._subject.ExamSuiteGroups.filter(esg => esg.id == examsuiteGroup.id)[initIndex];
            editedExamsuiteGroup.SubjectId = this._subject.id;
            editedExamsuiteGroup.ExamSuiteGroupName = examsuiteGroup.ExamSuiteGroupName;
            editedExamsuiteGroup.PassScore = examsuiteGroup.PassScore;
            editedExamsuiteGroup.ExamDuration = examsuiteGroup.ExamDuration;
            editedExamsuiteGroup.IsUsed = examsuiteGroup.IsUsed;

            this._subSvc.ActivateSubjectEditExamSuiteGroup(editedExamsuiteGroup);
        }

        public EditExamSuiteGroupMapPreparation(examSuiteGroupMap: ExamSuiteGroupMap) {
            this._editExamSuiteGroupMap = new ExamSuiteGroupMap(
                examSuiteGroupMap.id,
                examSuiteGroupMap.ExamSuiteId,
                examSuiteGroupMap.RandomCount,
                examSuiteGroupMap.ExamSuiteGroupId,
                examSuiteGroupMap.SubjectId
            );
            this._oldExamSuiteGroupMapRandomCount = examSuiteGroupMap.RandomCount;
        }

        public EditExamSuiteGroupMap(examsuiteGroupMap: ExamSuiteGroupMap) {
            var initIndex = 0;
            var examSuiteGroup = this._subject.ExamSuiteGroups.filter(esg => esg.id == examsuiteGroupMap.ExamSuiteGroupId)[0];
            var editedExamSuiteGroupMap = examSuiteGroup.ExamSuiteGroupMaps.filter(esgm => esgm.id == examsuiteGroupMap.id)[initIndex];

            editedExamSuiteGroupMap.SubjectId = this._subject.id;
            editedExamSuiteGroupMap.RandomCount = examsuiteGroupMap.RandomCount;

            var SumRandomCountCount: number = 0;
            for (var i = 0; i < examSuiteGroup.ExamSuiteGroupMaps.length; i++) {
                SumRandomCountCount += parseInt(examSuiteGroup.ExamSuiteGroupMaps[i].RandomCount);
            }
            examSuiteGroup.SumRandomCountCount = SumRandomCountCount;

            this._subSvc.ActivateSubjectEditExamSuiteGroupMap(editedExamSuiteGroupMap);
        }

        public AddExamSuiteGroup() {
            if (this._subject.ExamSuiteGroups.filter(esg => esg.ExamSuiteGroupName.trim() == this._newExamSuiteGroup.ExamSuiteGroupName.trim()).length > 0) {
                alert("ไม่สามารถเพิ่มชุดข้อสอบชื่อซ้ำกันได้");
                return null;
            }

            var newId = "";
            var newExamGroup = new ExamSuiteGroup(
                newId,
                this._newExamSuiteGroup.ExamSuiteGroupName.trim(),
                this._newExamSuiteGroup.IsUsed,
                this._newExamSuiteGroup.PassScore,
                this._newExamSuiteGroup.ExamDuration,
                this._newExamSuiteGroup.QuestionCount,
                this._newExamSuiteGroup.SumRandomCountCount,
                this._newExamSuiteGroup.ExamSuiteGroupMaps,
                this._subject.id
            );
            this._subSvc.ActivateSubjectAddExamSuiteGroup(newExamGroup).then(esg => {
                if (esg == null) {
                    alert('เกิดข้อผิดพลาดระหว่างการเพิ่มชุดข้อสอบ');
                } else {
                    esg.SumRandomCountCount = 0;
                    esg.SubjectId = this._subject.id;

                    this._subject.ExamSuiteGroups.push(esg);
                }
            });
        }

        public DeleteExamSuiteGroup(): void {
            var initIndex = 0;
            var index = this._subject.ExamSuiteGroups.indexOf(this._subject.ExamSuiteGroups.filter(esg => esg.id == this._editExamSuiteGroup.id)[0]);
            this._subSvc.ActivateSubjectDeleteExamSuiteGroup(this._subject.id, this._editExamSuiteGroup.id);
            this._subject.ExamSuiteGroups.splice(index, 1);
        }

        private GetExamSuiteTitleCode(examsuiteGroupMap: ExamSuiteGroupMap): string {
            return this._subject.ExamSuites.filter(es => es.id == examsuiteGroupMap.ExamSuiteId)[0].TitleCode;
        }

        private GetExamSuiteTitleName(examsuiteGroupMap: ExamSuiteGroupMap): string {
            return this._subject.ExamSuites.filter(es => es.id == examsuiteGroupMap.ExamSuiteId)[0].TitleName;
        }

        private GetExamSuiteQuestionCount(examsuiteGroupMap: ExamSuiteGroupMap): string {
            return this._subject.ExamSuites.filter(es => es.id == examsuiteGroupMap.ExamSuiteId)[0].QuestionCount;
        }

        private PrepareSubject(sub) {
            this._activatedSubjectId = this._subject.id;

            var QuestionCount: number = 0;
            for (var i = 0; i < sub.ExamSuites.length; i++) {
                QuestionCount += isNaN(sub.ExamSuites[i].QuestionCount) ? 0 : parseInt(sub.ExamSuites[i].QuestionCount);
            }
            this._subject.QuestionCount = QuestionCount;

            for (var i = 0; i < sub.ExamSuiteGroups.length; i++) {
                var SumRandomCountCount: number = 0;
                for (var j = 0; j < sub.ExamSuiteGroups[i].ExamSuiteGroupMaps.length; j++) {
                    SumRandomCountCount += isNaN(sub.ExamSuiteGroups[i].ExamSuiteGroupMaps[j].RandomCount) ? 0 : parseInt(sub.ExamSuiteGroups[i].ExamSuiteGroupMaps[j].RandomCount);
                }
                sub.ExamSuiteGroups[i].SumRandomCountCount = SumRandomCountCount;
            }

            this._editVoices = [];
            for (var voice in sub.VoiceLanguageList) {
                //this._editVoices.push(new VoiceLanguage(voice.id, voice.Language, voice.LanguageCode, voice.IsUsed));
            }
        }
    }

    angular
        .module('app.subjectApp')
        .controller('app.subjectApp.inactiveSubjectViewController', inactiveSubjectViewController)
        .controller('app.subjectApp.activatedSubjectViewController', activatedSubjectViewController);
}