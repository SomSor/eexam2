/// <binding AfterBuild='build' Clean='clean' />
"use strict";

var $ = require('gulp-load-plugins')();
var argv = require('yargs').argv;
var ts = require('gulp-typescript');
var gulp = require("gulp"),
    rimraf = require("rimraf"),
    //concat = require("gulp-concat"),
    //cssmin = require("gulp-cssmin"),
    //uglify = require("gulp-uglify"),
    router = require('front-router'),
    sequence = require('run-sequence');

// fix problems with undefined Promise class
// http://stackoverflow.com/questions/32490328/gulp-autoprefixer-throwing-referenceerror-promise-is-not-defined
require('es6-promise').polyfill();

// Check for --production flag
var isProduction = !!(argv.production);

// Ref tsconfig.json for typescript compiling
var tsProject = ts.createProject('scripts/tsconfig.json');

var paths = {
    webroot: "./wwwroot/",
    assets: [
      './client/**/*.*',
      '!./client/templates/**/*.*',
      '!./client/assets/{scss,js}/**/*.*'
    ],
    // Sass will check these folders for files when you use @import.
    sass: [
      'client/assets/scss',
      'wwwroot/lib/foundation-apps/scss'
    ],
    // These files include Foundation for Apps and its dependencies
    foundationJS: [
      'wwwroot/lib/fastclick/lib/fastclick.js',
      'wwwroot/lib/viewport-units-buggyfill/viewport-units-buggyfill.js',
      'wwwroot/lib/tether/tether.js',
      'wwwroot/lib/hammerjs/hammer.js',
      'wwwroot/lib/angular/angular.js',
      'wwwroot/lib/angular-animate/angular-animate.js',
      'wwwroot/lib/angular-ui-router/release/angular-ui-router.js',
      'wwwroot/lib/foundation-apps/js/vendor/**/*.js',
      'wwwroot/lib/foundation-apps/js/angular/**/*.js',
      '!wwwroot/lib/foundation-apps/js/angular/app.js'
    ],
    // These files are for your app's JavaScript
    appJS: [
      'client/assets/js/app.js'
    ]
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

paths.genTmpl = paths.webroot + "templates";
paths.genAssets = paths.webroot + "assets";
paths.genJsDest = paths.webroot + "assets/js";

// Cleans the build directory
gulp.task('clean:assets', function (cb) {
    rimraf(paths.genAssets, cb);
});

gulp.task('clean:tmpl', function (cb) {
    rimraf(paths.genTmpl, cb);
});

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:assets", "clean:tmpl", "clean:js", "clean:css"]);

// Default asp.net5 min
//gulp.task("min:js", function () {
//    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
//        .pipe($.concat(paths.concatJsDest))
//        .pipe($.uglify())
//        .pipe(gulp.dest("."));
//});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe($.concat('app.min.css'))
        .pipe($.cssmin())
        .pipe(gulp.dest('./wwwroot/assets/css/'));
});

//gulp.task("min", ["min:js", "min:css"]);
gulp.task("min", ["min:css"]);

// Copies everything in the client folder except templates, Sass, and JS
gulp.task('copy', function () {
    return gulp.src(paths.assets, {
        base: './client/'
    })
      .pipe(gulp.dest(paths.webroot))
    ;
});

// Copies your app's page templates and generates URLs for them
gulp.task('copy:templates', function () {
    return gulp.src('./client/templates/**/*.html')
      .pipe(router({
          path: 'wwwroot/js/routes.js',
          root: 'client'
      }))
      .pipe(gulp.dest(paths.genTmpl))
    ;
});

// Compiles the Foundation for Apps directive partials into a single JavaScript file
gulp.task('copy:foundation', function (cb) {
    gulp.src('wwwroot/lib/foundation-apps/js/angular/components/**/*.html')
      .pipe($.ngHtml2js({
          prefix: 'components/',
          moduleName: 'foundation',
          declareModule: false
      }))
      //.pipe($.uglify())
      .pipe($.concat('templates.js'))
      .pipe(gulp.dest('./wwwroot/js'))
    ;

    // Iconic SVG icons
    gulp.src('./wwwroot/lib/foundation-apps/iconic/**/*')
      .pipe(gulp.dest('./wwwroot/assets/img/iconic/'))
    ;

    cb();
});

// Compiles Sass
gulp.task('sass', function () {
    //var minifyCss = $.if(isProduction, $.cssmin());

    return gulp.src('client/assets/scss/app.scss')
      .pipe($.sass({
          includePaths: paths.sass,
          outputStyle: (isProduction ? 'compressed' : 'nested'),
          errLogToConsole: true
      }))
      .pipe($.autoprefixer({
          browsers: ['last 2 versions', 'ie 10']
      }))
      //.pipe($.cssmin())
      .pipe(gulp.dest('./wwwroot/css/'))
    ;
});

// Compile TypeScript
gulp.task('compile:ts', function () {
    var sourcemaps = require('gulp-sourcemaps');

    var tsResult = tsProject.src() // instead of gulp.src(...) 
        .pipe(sourcemaps.init())
		.pipe(ts(tsProject));

    return tsResult.js
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('.'));
});

// Compiles and copies the Foundation for Apps JavaScript, as well as your app's custom JS
gulp.task('uglify', ['uglify:foundation', 'uglify:app'])

gulp.task('uglify:foundation', function (cb) {
    var uglify = $.uglify()
      .on('error', function (e) {
          console.log(e);
      });

    return gulp.src(paths.foundationJS)
      .pipe(uglify)
      .pipe($.concat('foundation.min.js'))
      .pipe(gulp.dest(paths.genJsDest))
    ;
});

gulp.task('uglify:app', function () {
    var uglify = $.uglify()
      .on('error', function (e) {
          console.log(e);
      });

    return gulp.src([paths.js, "!" + paths.minJs])
      .pipe(uglify)
      .pipe($.concat('app.min.js'))
      .pipe(gulp.dest(paths.genJsDest))
    ;
});

// Builds your entire app once, without starting a server
gulp.task('build', function (cb) {
    sequence('compile:ts', ['copy', 'copy:foundation', 'sass', 'min', 'uglify'], 'copy:templates', cb);
});

// Starts a test server, which you can view at http://localhost:8079
gulp.task('server', ['build'], function () {
    gulp.src('./wwwroot')
      .pipe($.webserver({
          port: 8079,
          host: 'localhost',
          fallback: 'index.html',
          livereload: true,
          open: true
      }))
    ;
});

// Launch task: builds your app, starts a server, and recompiles assets when they change
gulp.task('launch', ['server'], function () {
    // Watch Sass
    gulp.watch(['./client/assets/scss/**/*', './scss/**/*'], ['sass', 'min']);

    // Watch JavaScript
    gulp.watch(['./client/assets/js/**/*', './js/**/*'], ['uglify:app']);

    // Watch static files
    gulp.watch(['./client/**/*.*', '!./client/templates/**/*.*', '!./client/assets/{scss,js}/**/*.*'], ['copy']);

    // Watch app templates
    gulp.watch(['./client/templates/**/*.html'], ['copy:templates']);
});
