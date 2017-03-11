# MedIOExNET
.NET wrapper for MedIOEx Raspberry IO Expander Card ( https://github.com/pe2a/MedIOEx )

This library uses https://github.com/frankhommers/LibBcm2835.Net to communicate with Broadcom BCM 2835 chip and the necessary libbcm2835.so lib is compiled on demand on first run.

You can run the sample program with the following command:

```
sudo mono MedIOEx.NET.Sample.exe
```