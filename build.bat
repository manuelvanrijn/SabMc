@echo off
echo.
echo --------------------------------------------------
echo Build SabMc
echo --------------------------------------------------
echo.

set hour=%time:~0,2%
if "%hour:~0,1%"==" " set hour=0%time:~1,1%
set BuildTimeStamp=%date:~-4,4%-%date:~-7,2%-%date:~-10,2%_%hour%-%time:~3,2%

echo. %BuildTimeStamp%

msbuild SabMc.sln

mkdir release\sabmc_%BuildTimeStamp%

echo SabMc.TvShow
cd SabMc.TvShow\bin\Debug
cp -R -v -f * ../../../release/sabmc_%BuildTimeStamp%
cd ../../../

echo SabMc.Movie
cd SabMc.Movie\bin\Debug
cp -R -v -f * ../../../release/sabmc_%BuildTimeStamp%
cd ../../../

echo SabMc.Notifo
cd SabMc.Notifo\bin\Debug
cp -R -v -f * ../../../release/sabmc_%BuildTimeStamp%
cd ../../../

echo Cleanup
cd release\sabmc_%BuildTimeStamp%
del /Q *.pdb
del /Q *.vshost.exe
del /Q config.xml

cd ../../

:End
pause
