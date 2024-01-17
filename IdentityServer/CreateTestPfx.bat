rem epasaule as pwd
"C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\MakeCert" -n "CN=localhost" -a sha256 -sv epasaule.pvk -r epasaule.cer
"C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\pvk2pfx" -pvk epasaule.pvk -spc epasaule.cer -pfx epasaule.pfx -pi epasaule