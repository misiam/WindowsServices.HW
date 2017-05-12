for %%A in (..\WindowsServices.HW.ScanService.exe) DO set P=%%~fA
sc create WindowsServices.HW.ScanService binPath="%P% -inputFolders:C:\winserv\inputs\1;C:\winserv\inputs\2 -outputFolder:C:\winserv\outputs -scanInterval:5000 -logPath:C:\winserv\ImgScanner.log"
