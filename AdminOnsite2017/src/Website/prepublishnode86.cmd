set PATH=C:\Program Files (x86)\nodejs;%PATH%

call npm install || goto :error
call bower install || goto :error
call gulp clean || goto :error
call gulp build --production || goto :error
goto :EOF

:error
echo Failed with error #%errorlevel%.
exit /b %errorlevel%