module app.createQuestionApp {
    'use strict';

    class createQuestionController {
        public _examSuite: app.examSuiteApp.ExamSuite = new examSuiteApp.ExamSuite("", "", "", "", "", "", []);
        public _question: app.examSuiteApp.Question;
        public _correctAnswer: string = "1";
        public _choices: string[] = ["1", "2", "3", "4"];

        static $inject = ['$scope', 'app.examSuiteApp.examSuiteService', 'app.createQuestionApp.createQuestionService', '$stateParams', '$state'];
        constructor(private $scope, private _examSuiteSvc: app.examSuiteApp.examSuiteService, private _questionSvc: app.createQuestionApp.createQuestionService, private $stateParams, private $state) {
            this._examSuiteSvc.InactiveGetExamSuite(this.$stateParams.examsuiteid).then(data => {
                this._examSuite = data;
                this.Clear();
            });
        }

        Show(id) {
            this._question = this._examSuite.Questions.filter(x => x.id == id)[0];
            this._correctAnswer = this._question.Choices.filter(x => x.IsCorrect)[0].id;
            document.getElementById('txtQuestion').focus();
        }

        Clear() {
            this._question = new app.examSuiteApp.Question("", (this._examSuite.Questions.length + 1).toString(), true, "", [
                new app.examSuiteApp.Choice("1", "", true, []),
                new app.examSuiteApp.Choice("2", "", false, []),
                new app.examSuiteApp.Choice("3", "", false, []),
                new app.examSuiteApp.Choice("4", "", false, [])
            ], [], [], this._examSuite.id, this._examSuite.TitleCode);
            this._correctAnswer = "1";
        }

        Create() {
            if (this._question.id == '' && confirm("ต้องการสร้างข้อสอบ " + this._question.Detail + "?")) {
                for (var i = 0; i < this._question.Choices.length; i++) {
                    this._question.Choices[i].IsCorrect = this._question.Choices[i].id == this._correctAnswer;
                }

                this._questionSvc.Create(this._question).then(data => {
                    this._examSuiteSvc.InactiveGetExamSuite(this.$stateParams.examsuiteid).then(data => {
                        this._examSuite = data;
                        this.Clear();
                    });
                    alert(data.Message + ", QuestionId: " + data.QuestionId);
                });
            } else if (this._question.id != '' && confirm("ต้องการแก้ข้อสอบ " + this._question.Detail + "?")) {
                for (var i = 0; i < this._question.Choices.length; i++) {
                    this._question.Choices[i].IsCorrect = this._question.Choices[i].id == this._correctAnswer;
                }

                this._questionSvc.Update(this._question).then(data => {
                    this._examSuiteSvc.InactiveGetExamSuite(this.$stateParams.examsuiteid).then(data => {
                        this._examSuite = data;
                        this.Clear();
                    });
                    alert(data.Message + ", QuestionId: " + data.QuestionId);
                });
            }
        }

        Delete() {
            if (this._examSuite && this._examSuite.id) {
                if (confirm("ต้องการลบข้อสอบ " + this._question.Detail + "?")) {
                    this._questionSvc.Delete(this._examSuite.id, this._question.id).then(data => {
                        this._examSuiteSvc.InactiveGetExamSuite(this.$stateParams.examsuiteid).then(data => {
                            this._examSuite = data;
                            this.Clear();
                        });
                        alert(data.Message + ", QuestionId: " + data.QuestionId);
                    });
                }
            } else {
                alert("กรุณาเลือกข้อสอบ");
            }
        }
    }

    angular
        .module('app.createQuestionApp', [])
        .controller('app.createQuestionApp.createQuestionController', createQuestionController)
        ;
}