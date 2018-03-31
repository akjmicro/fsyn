include ../fsyn.fs

\ a local constant for 'change-frequency'
0.7575001e fconstant CHANGE_FREQ

\ amp envelope stuff:
0.01e 0.4e create-port-struct port-struct
fvariable fmamp 0e fmamp f!

\ delay setup:
create-delay-line mydelay
mydelay init-delay-line

\ filter setup:
make-lowpass-struct lpstruct
lpstruct reinit-allpass

\ random pitch setup:
create pitch-choices 120e f, 140e f, 105e f, 160e f,
fvariable current-pitch current-pitch 120e f!
0.1e 0.1e create-port-struct port-struct2

: delay-example
BEGIN
  \ add some time-based interest, ala 'BYTEBEAT':
  t 149 * t 107 * - 219893 * 219889 % t ^ 219829 & s>f 219889e f/
  \ trigger for portamento envelope 
  0.5e f* CHANGE_FREQ metro 
  \ reference array for port and portamento call
  port-struct port
  \ route to 'fmamp' variable:
  fmamp f!

  \ change the pitch variable randomly, in sync with metronome freq:
  CHANGE_FREQ phasor 0.0e f- EPSILON 128e f* f<= if
    pitch-choices 4 randint floats + f@ current-pitch f! 
  endif
  \ FM oscillator:
  fmamp f@ 0.99e 1.5217e current-pitch f@ port-struct2 port fm
  \ cut the highs a bit:
  false lpstruct 1500e lowpass1
  \ boost a bit:
  1.1e f*
  \ pull in audio from previous rounds (feedback)
  \ 0.75e add-feedback 

  \ write fm signal and feedback variable to delay,
  \ original signal is copied and stays on stack:
  mydelay delay-write
  0.3e 0.6e sinewave 0.4e f+ panmix

  \ tap the delay to get a second signal; indicate feedback amount, too:
  mydelay 0.3e 0.75e delay-read
  0.15e 0.417e sinewave 0.4e f+ panmix

  stereo_out
  t+
AGAIN ;

delay-example
