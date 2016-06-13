%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe FashionService.exe
Net Start MyService
sc config MyService start= auto