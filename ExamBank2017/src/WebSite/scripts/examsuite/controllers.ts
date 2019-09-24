module app.examSuiteApp {
    'use strict';

    class inactiveExamSuiteViewController {
        public _examSuite: ExamSuite;
        private _editQuestion: Question;
        public _rejectConsiderations: Consideration[];

        static $inject = ['$scope', 'app.examSuiteApp.examSuiteService', '$stateParams', '$state'];
        constructor(private $scope, private examSuiteSvc: app.examSuiteApp.examSuiteService, private $stateParams, private $state) {
            this.examSuiteSvc.InactiveGetExamSuite(this.$stateParams.examsuiteid).then(es => {
                this._examSuite = es;
                console.log(es);
            });
        }

        public ConsiderQuestion(question: Question, isAccept: boolean, rejectComment: string) {
            var initIndex = 0;
            console.log(this._examSuite);
            //Hack: Fix username be admin
            var consideration = new Consideration('00', new Date(), rejectComment, isAccept, 'Admin', this._examSuite.id, this._examSuite.TitleCode, question.QuestionNumber);
            this.examSuiteSvc.QuestionInactiveConsiderQuestion(consideration).then(con => {
                this._examSuite.Questions.filter(q => q.id == question.id)[initIndex].Considerations.push(con);
            });
        }

        public DeleteExamSuite(ExamSuiteId: string) {
            this.examSuiteSvc.ConfigurationInactiveDeleteExamSuite(ExamSuiteId).then(() => {
                this.$state.go('inactivesubjectview', { subjectid: this._examSuite.SubjectId });
            });
        }

        private EditQuestionPreparation(question: Question): void {
            this._editQuestion = new Question(
                question.id,
                question.QuestionNumber,
                question.IsAllowRandomChoice,
                question.Detail,
                question.Choices.map(c => new Choice(c.id, c.Detail, c.IsCorrect, c.Voices)),
                question.Considerations,
                question.Voices,
                question.ExamSuiteId,
                this._examSuite.TitleCode);
        }

        public EditQuestion(question: Question) {
            var initIndex = 0;
            var qryQuestion = this._examSuite.Questions.filter(q => q.id == question.id)[initIndex];
            qryQuestion.IsAllowRandomChoice = question.IsAllowRandomChoice;
            qryQuestion.TitleCode = question.TitleCode;
            for (var index = 0; index < qryQuestion.Choices.length; index++) {
                qryQuestion.Choices[index] = question.Choices[index];
            }
            this.examSuiteSvc.QuestionInactiveEditQuestion(qryQuestion);
        }

        private ChangeChoiceCorrect(choice: Choice): void {
            this._editQuestion.Choices.forEach((c) => {
                if (c.id == choice.id) c.IsCorrect = true;
                else c.IsCorrect = false;
            });
        }

        private IsShowAccept(question: Question): boolean {
            var isQuestionNull = question == null;
            if (isQuestionNull) return null;

            var isConsiderationNull = question.Considerations == null;
            if (isConsiderationNull) return null;

            var minimumeAmount = 1;
            var lastLength = question.Considerations.length - 1;
            var isAmountLowerThanMinimum = question.Considerations.length < minimumeAmount;
            if (isAmountLowerThanMinimum) return null;

            var result = question.Considerations[lastLength].IsAccept;
            return result;
        }

        public ViewAllConsiderationHistory(question: Question): void {
            this._rejectConsiderations = question.Considerations;
        }

        public LimitCommentLength(text: string) {
            var limitLength = 15;
            if (text != null && text.length > limitLength) {
                return text.substr(0, limitLength) + " ...";
            }
            return text;
        }

        public Back() {
            this.$state.go('inactivesubjectview', { subjectid: this._examSuite.SubjectId });
        }
    }

    class activatedExamSuiteViewController {
        public _examSuite: ExamSuite;
        private _editQuestion: Question;

        static $inject = ['$scope', 'app.examSuiteApp.examSuiteService', '$stateParams', '$state'];
        constructor(private $scope, private examSuiteSvc: app.examSuiteApp.examSuiteService, private $stateParams, private $state) {
            this.examSuiteSvc.ActivatedGetExamSuite($stateParams.subjectid, $stateParams.examsuiteid).then(es => {
                this._examSuite = es;
            });
        }

        private EditQuestionPreparation(question: Question): void {
            this._editQuestion = new Question(
                question.id,
                question.QuestionNumber,
                question.IsAllowRandomChoice,
                question.Detail,
                question.Choices.map(c => new Choice(c.id, c.Detail, c.IsCorrect, c.Voices)),
                question.Considerations,
                question.Voices,
                question.ExamSuiteId,
                this._examSuite.TitleCode);
        }

        public EditQuestion(question: Question) {
            var initIndex = 0;
            var qryQuestion = this._examSuite.Questions.filter(q => q.id == question.id)[initIndex];
            qryQuestion.IsAllowRandomChoice = question.IsAllowRandomChoice;
            for (var index = 0; index < qryQuestion.Choices.length; index++) {
                qryQuestion.Choices[index] = question.Choices[index];
            }
            this.examSuiteSvc.QuestionActivateEditQuestion(qryQuestion);
        }

        private ChangeChoiceCorrect(choice: Choice): void {
            this._editQuestion.Choices.forEach((c) => {
                if (c.id == choice.id) c.IsCorrect = true;
                else c.IsCorrect = false;
            });
        }

        public Back() {
            this.$state.go('activatedsubjectview', { subjectid: this._examSuite.SubjectId });
        }
    }

    angular
        .module('app.examSuiteApp')
        .controller('app.examSuiteApp.inactiveExamSuiteViewController', inactiveExamSuiteViewController)
        .controller('app.examSuiteApp.activatedExamSuiteViewController', activatedExamSuiteViewController);
}