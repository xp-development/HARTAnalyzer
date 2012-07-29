!include MUI2.nsh

!define APPNAME "HART Analyzer"
!define APPEXE "Hart.Analyzer.exe"

  ;Name and file
Name "${APPNAME}"
OutFile "Setup.exe"

  ;Default installation folder
InstallDir "$PROGRAMFILES\${APPNAME}"
  
  ;Get installation folder from registry if available
InstallDirRegKey HKLM "SOFTWARE\${APPNAME}" "installdir"

  ;Request application privileges for Windows Vista
RequestExecutionLevel admin

!define MUI_ABORTWARNING
!define MUI_ICON "content\Icon.ico"
!define HELPURL "http://hartanalyzer.codeplex.com/"
!define UPDATEURL "http://hartanalyzer.codeplex.com/"
!define ABOUTURL "http://hartanalyzer.codeplex.com/"


Var SMDir ;Start menu folder
!insertmacro MUI_PAGE_LICENSE "Microsoft_Permissive_License.rtf"
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_STARTMENU 0 $SMDir
!insertmacro MUI_PAGE_INSTFILES

# These indented statements modify settings for MUI_PAGE_FINISH
!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_CHECKED
!define MUI_FINISHPAGE_RUN_TEXT "Start ${APPNAME}"
!define MUI_FINISHPAGE_RUN_FUNCTION "StartApplication"
!insertmacro MUI_PAGE_FINISH


!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English"

Section ""

  SetOutPath "$INSTDIR"
  
  ;ADD YOUR OWN FILES HERE...
  File content\*.*

  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayName" "${APPNAME}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "QuietUninstallString" "$\"$INSTDIR\Uninstall.exe$\" /S"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "InstallLocation" "$\"$INSTDIR$\""
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayIcon" "$\"$INSTDIR\Icon.ico$\""
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "Publisher" "${APPNAME}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "HelpLink" "$\"${HELPURL}$\""
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "URLUpdateInfo" "$\"${UPDATEURL}$\""
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "URLInfoAbout" "$\"${ABOUTURL}$\""
  # There is no option for modifying or repairing the install
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "NoRepair" 1

  
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

Section -StartMenu
  !insertmacro MUI_STARTMENU_WRITE_BEGIN 0
  CreateDirectory "$SMPROGRAMS\$SMDir"
  CreateShortCut "$SMPROGRAMS\$SMDir\${APPNAME}.lnk" "$INSTDIR\${APPEXE}" "" "$INSTDIR\Icon.ico"

  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "startmenu" "$SMDir"

  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section "Uninstall"

  ReadRegStr $SMDir HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "startmenu"

  Delete "$SMPROGRAMS\$SMDir\${APPNAME}.lnk"
  RMDir "$SMPROGRAMS\$SMDir"

  Delete "$INSTDIR\Uninstall.exe"

  RMDir /r "$INSTDIR"

  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}"

SectionEnd

Function StartApplication
  ExecShell "" "$INSTDIR\${APPEXE}"
FunctionEnd

















