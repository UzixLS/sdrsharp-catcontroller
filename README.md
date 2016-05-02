# sdrsharp-catcontroller

This plugin allow SDR# to be controlled via serial (COM) interface with set of TS-50 receiver commands.
E.g. it can be coupled with com0com virtual serial port and Fldigi for better convenience.

Supported commands:
* IF - get frequency and mode
* FA - set frequency
* MD - set mode (AM,FM,USB,LSB,CW)

Serial port parameters:
* Speed: 9600
* Data bits: 8
* Stop bits: 1
* Parity: none

This plugin based on code by pewusoft (http://users.vline.pl/~pewusoft/)
