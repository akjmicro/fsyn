s" portmidi" add-lib
\c #include <portmidi.h>
\c #include <porttime.h>
\c #include "stdlib.h"
\c #include "stdio.h"
\c #include "string.h"
\c #include "assert.h"
\c #define DRIVER_INFO NULL
\c #define TIME_PROC ((int32_t (*)(void *)) Pt_Time)
\c #define TIME_INFO NULL
\c #define TIME_START Pt_Start(1, 0, 0)
\c PmStream * midi;
\c PmError status, length;
\c PmEvent buffer[1];
\c #define Pm_DeviceInfoWrap(i) \
\c   const PmDeviceInfo *info = Pm_GetDeviceInfo(i); \
\c   printf("\n%d: %s, %s, (%s)  ", (int) i, info->interf, info->name, \
\c          info->input == 1 ? "input" : "output");
\c #define Pm_OpenInputWrap(i) \
\c         Pm_OpenInput(&midi,i,DRIVER_INFO,100,TIME_PROC,TIME_INFO)
\c #define Pm_PollWrap() Pm_Poll(midi)
\c #define Pm_ReadWrap() Pm_Read(midi, buffer, 1)
\c #define Pm_Timestamp() buffer[0].timestamp
\c #define Pm_StatusWrap() Pm_MessageStatus(buffer[0].message)
\c #define Pm_Data1Wrap() Pm_MessageData1(buffer[0].message)
\c #define Pm_Data2Wrap() Pm_MessageData2(buffer[0].message)
\c #define Pm_CloseWrap() Pm_Close(midi)

c-function pm-count-devices Pm_CountDevices -- n
c-function pm-get-device-info Pm_DeviceInfoWrap n -- void
c-function pm-get-default-in Pm_GetDefaultInputDeviceID -- d
c-function pm-open-input Pm_OpenInputWrap n -- n
c-function pm-poll Pm_PollWrap -- n
c-function pm-read Pm_ReadWrap -- n
c-function pm-get-timestamp Pm_Timestamp -- d
c-function pm-get-status Pm_StatusWrap -- n
c-function pm-get-data1 Pm_Data1Wrap -- n
c-function pm-get-data2 Pm_Data2Wrap -- n
c-function pm-close Pm_CloseWrap -- n

: pm-list-devices ( -- )
  pm-count-devices 0
  do
    i pm-get-device-info
  loop ;

: pm-get-message ( -- n n n )
  pm-get-data2 pm-get-data1 pm-get-status ;

: pm-echo
  pm-poll 
  if
    pm-read
    if
      pm-get-message . . . CR
    endif
  endif ;

: pm-echo-loop
  begin pm-echo again ;
