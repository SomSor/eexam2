module app.createInactiveSubjectApp {
    'use strict';

    class createInactiveSubjectController {
        public _subjectList: app.subjectlistApp.SubjectList;
        public _subject: app.subjectlistApp.SubjectDetail = new subjectlistApp.SubjectDetail("", "", "", "TH", "0", "0", "0", "0", "1.0.0", false, "");

        static $inject = ['$scope', 'app.subjectlistApp.subjectListService', 'app.createInactiveSubjectApp.createInactiveSubjectService', '$state'];
        constructor(private $scope, private _subLSvc: app.subjectlistApp.subjectListService, private _subSvc: app.createInactiveSubjectApp.createInactiveSubjectService, private $state) {
            this._subLSvc.InactiveGetSubject().then(data => {
                this._subjectList = data;
            });
        }

        Show(id) {
            this._subject = this._subjectList.SubjList.filter(x => x.id == id)[0];
            document.getElementById('txtSubjectCode').focus();
        }

        Clear() {
            this._subject = new subjectlistApp.SubjectDetail("", "", "", "TH", "0", "0", "0", "0", "1.0.0", false, "");
        }

        Create() {
            if (this._subject.id == '' && confirm("ต้องการสร้างวิชา " + this._subject.SubjectName + "?")) {
                this._subSvc.Create(this._subject).then(data => {
                    this._subLSvc.InactiveGetSubject().then(data2 => {
                        this._subjectList = data2;
                        this.Clear();
                    });
                    alert(data.Message + ", SubjectId: " + data.SubjectId);
                });
            } else if (this._subject.id != '' && confirm("ต้องการแก้ไขวิชา " + this._subject.SubjectName + "?")) {
                this._subSvc.Update(this._subject).then(data => {
                    this._subLSvc.InactiveGetSubject().then(data2 => {
                        this._subjectList = data2;
                        this.Clear();
                    });
                    alert(data.Message + ", SubjectId: " + data.SubjectId);
                });
            }
        }

        Delete() {
            if (this._subject && this._subject.id) {
                if (confirm("ต้องการลบวิชา " + this._subject.SubjectName + "?")) {
                    console.log(this._subject.id);
                    this._subSvc.Delete(this._subject.id).then(data => {
                        this._subLSvc.InactiveGetSubject().then(data2 => {
                            this._subjectList = data2;
                            this.Clear();
                        });
                        alert(data.Message + ", SubjectId: " + data.SubjectId);
                    });
                }
            } else {
                alert("กรุณาเลือกรายวิชา");
            }
        }
    }

    angular
        .module('app.createInactiveSubjectApp', [])
        .controller('app.createInactiveSubjectApp.createInactiveSubjectController', createInactiveSubjectController)
        ;
}