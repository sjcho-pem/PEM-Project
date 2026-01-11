@echo off
:: 날짜 변수 초기화
setlocal enabledelayedexpansion

:: 사용자로부터 며칠치 로그를 처리할지 입력받기
:input_days
set /p days=Enter the number of days to process (1-100): 

:: 입력값 검증
set /a test=days 2>nul
if "%test%" neq "%days%" (
    echo Invalid input. Please enter a numeric value.
    goto input_days
)

if %days% LSS 1 (
    echo Invalid input. Please enter a number greater than 0.
    goto input_days
)

if %days% GTR 100 (
    echo Invalid input. Please enter a number less than or equal to 100.
    goto input_days
)

echo Valid input: %days%
set script_dir=%~dp0

:: 출력 폴더 생성
set output_dir=!script_dir!LogExport_%date:~0,4%%date:~5,2%%date:~8,2%_%days%Days
if not exist "%output_dir%" (
    mkdir "%output_dir%"
)
echo folderName: !output_dir!

:: 오늘 날짜 가져오기
for /f "tokens=2 delims==" %%I in ('wmic os get localdatetime /value') do set dt=%%I
set today_year=%dt:~0,4%
set today_month=%dt:~4,2%
set today_day=%dt:~6,2%
set today=%today_year%-%today_month%-%today_day%
set today_nohyphen=%today_year%%today_month%%today_day%

:: 날짜별 로그 복사
:: 1일 입력 시 오늘 날짜만 처리되도록 수정
set /a days_to_process=%days%-1

if %days_to_process% LSS 0 (
    set days_to_process=0
)

for /l %%I in (0,1,%days_to_process%) do (
    :: 각 날짜 계산
    call :CalculateDate %%I processed_date processed_date_nohyphen

    echo Processing date: !processed_date! and !processed_date_nohyphen!

    :: SECS-II 로그 복사
    if exist "D:\LOG\UbiGem\GEM_EQP\SECS-II\!processed_date!" (
        for %%L in ("D:\LOG\UbiGem\GEM_EQP\SECS-II\!processed_date!\*.log") do (
            echo Copying: %%L
            copy "%%L" "!output_dir!"
        )
    ) else (
        echo No SECS-II logs found for: !processed_date!
    )

    :: SeqLog 복사
    if exist "D:\LOG\Sequence" (
        if exist "D:\LOG\Sequence\SeqLog_!processed_date_nohyphen!.csv" (
            echo Copying: D:\LOG\Sequence\SeqLog_!processed_date_nohyphen!.csv
            copy "D:\LOG\Sequence\SeqLog_!processed_date_nohyphen!.csv" "!output_dir!"
        ) else (
            echo No SeqLog found for: !processed_date_nohyphen!
        )
    ) else (
        echo Sequence log directory does not exist.
    )

    :: MMI 로그 복사
    if exist "D:\LOG\MMI" (
        if exist "D:\LOG\MMI\MachineName_!processed_date_nohyphen!.log" (
            echo Copying: D:\LOG\MMI\MachineName_!processed_date_nohyphen!.log
            copy "D:\LOG\MMI\MachineName_!processed_date_nohyphen!.log" "!output_dir!"
        ) else (
            echo No MMI logs found for: !processed_date_nohyphen!
        )
    ) else (
        echo MMI log directory does not exist.
    )
)

:: 완료 메시지
echo Log processing completed. Check the output directory: %output_dir%
pause
goto :eof

:: 날짜 계산 함수
:CalculateDate
:: 인수: %1 (일 수), %2 (출력 변수1), %3 (출력 변수2)
set /a days_offset=%1
for /f "tokens=2 delims==" %%I in ('wmic os get localdatetime /value') do set dt=%%I
set /a current_year=%dt:~0,4%
set /a current_month=%dt:~4,2%
set /a current_day=%dt:~6,2%

:: 계산된 날짜를 YYYY-MM-DD 및 YYYYMMDD로 출력
for /f %%I in ('powershell -Command "[DateTime]::Today.AddDays(-%days_offset%).ToString('yyyy-MM-dd')"') do set %2=%%I
for /f %%I in ('powershell -Command "[DateTime]::Today.AddDays(-%days_offset%).ToString('yyyyMMdd')"') do set %3=%%I
goto :eof
