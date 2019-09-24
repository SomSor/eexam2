module app.creatEexamSuiteApp {
    'use strict';

    class creatEexamSuiteController {
        public _subject: app.subjectApp.Subject;
        public _examSuite: app.subjectApp.ExamSuiteDetail = new subjectApp.ExamSuiteDetail("", "", "", "", "");

        static $inject = ['$scope', 'app.subjectApp.subjectService', 'app.examSuiteApp.examSuiteService', '$stateParams', '$state'];
        constructor(private $scope, private _subSvc: app.subjectApp.subjectService, private _examSuiteSvc: app.examSuiteApp.examSuiteService, private $stateParams, private $state) {
            this._subSvc.GetInactiveSubject(this.$stateParams.subjectid).then(sub => {
                this._subject = sub;
            });
        }

        Show(id) {
            this._examSuite = this._subject.ExamSuites.filter(x => x.id == id)[0];
            document.getElementById('txtTitleCode').focus();
        }

        Clear() {
            this._examSuite = new subjectApp.ExamSuiteDetail("", "", "", "", "");
        }

        Create() {
            if (this._examSuite.id == '' && confirm("ต้องการสร้างหมวด " + this._examSuite.TitleName + "?")) {
                this._examSuite.QuestionCount = "0";
                this._examSuite.ConsiderationStatus = this._subject.id;
                this._examSuiteSvc.Create(this._examSuite).then(data => {
                    this._subSvc.GetInactiveSubject(this.$stateParams.subjectid).then(sub => {
                        this._subject = sub;
                        this.Clear();
                    });
                    alert(data.Message + ", ExamSuiteId: " + data.ExamSuiteId);
                });
            } else if (this._examSuite.id != '' && confirm("ต้องการแก้ไขหมวด " + this._examSuite.TitleName + "?")) {
                this._examSuiteSvc.Update(this._examSuite).then(data => {
                    this._subSvc.GetInactiveSubject(this.$stateParams.subjectid).then(sub => {
                        this._subject = sub;
                        this.Clear();
                    });
                    alert(data.Message + ", ExamSuiteId: " + data.ExamSuiteId);
                });
            }
        }

        Delete() {
            if (this._examSuite && this._examSuite.id) {
                if (confirm("ต้องการลบหมวด " + this._examSuite.TitleName + "?")) {
                    this._examSuiteSvc.Delete(this._examSuite.id).then(data => {
                        this._subSvc.GetInactiveSubject(this.$stateParams.subjectid).then(sub => {
                            this._subject = sub;
                            this.Clear();
                        });
                        alert(data.Message + ", ExamSuiteId: " + data.ExamSuiteId);
                    });
                }
            } else {
                alert("กรุณาเลือกหมวด");
            }
        }
    }

    angular
        .module('app.creatEexamSuiteApp', [])
        .controller('app.creatEexamSuiteApp.creatEexamSuiteController', creatEexamSuiteController)
        ;
}