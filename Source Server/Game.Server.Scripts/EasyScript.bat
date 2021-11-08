echo @off
cd C:\DDtServer\4.1\Source 4.1.5\Road.Service\bin\Debug\scripts
@RD /S /Q "C:\DDtServer\4.1\Source 4.1.5\Road.Service\bin\Debug\scripts\AI"
mkdir AI
xcopy /s /i "C:\DDtServer\4.1\Source 4.1.5\Game.Server.Scripts\AI"  "C:\DDtServer\4.1\Source 4.1.5\Road.Service\bin\Debug\scripts\AI"
Pause;