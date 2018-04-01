include ../fsyn.fs
include ../primes.fs

\ a local constant for 'change-frequency'
0.7575e 1.333333e f* fconstant CHANGE_FREQ

\ amp envelope stuff:
0.001e 0.5e create-port-struct port-struct
fvariable fmamp 0e fmamp f!

\ delay setup:
create-delay-line mydelay
mydelay init-delay-line

\ filter setup:
make-lowpass-struct lpstruct
lpstruct reinit-allpass

\ random pitch setup:
create pitch-choices 120e f, 140e f, 105e f, 160e f,
fvariable current-pitch 120e current-pitch f!
0.02e 0.03e create-port-struct port-struct2
fvariable last_phase 0.0e last_phase f!

: delay-example
begin
  \ add some time-based interest, ala 'BYTEBEAT':
  t 1149 * t 107 * - 219893 * 219889 % t ^ 219829 & s>f 219889e f/
  \ trigger for portamento envelope 
  t 20 <<  1 + is-prime s>f -1.0e f* f* 5.0e f* 1.0e fmod
  0.001e CHANGE_FREQ gate 
  \ reference array for port and portamento call
  port-struct port
  \ route to 'fmamp' variable:
  fmamp f!

  \ change the pitch variable randomly, in sync with metronome freq:
  CHANGE_FREQ 1.618e f* phasor { F: cur_phase }
  cur_phase last_phase f@ f< if
    pitch-choices 4 randint floats + f@ current-pitch f!
  else 4 randint drop 
  endif cur_phase last_phase f!
  \ FM oscillator:
  fmamp f@ 0.99e 1.5217e current-pitch f@ port-struct2 port fm
  \ cut the highs a bit:
  lpstruct false 2500e lowpass1

  \ write fm signal and feedback variable to delay,
  \ original signal is copied and stays on stack:
  mydelay delay-write 0.4e f*
  0.3e CHANGE_FREQ 0.5e f* sinewave 0.5e f+ panmix

  \ tap the delay to get a second signal; indicate feedback amount, too:
  mydelay 0.3e 0.6e delay-read
  0.15e CHANGE_FREQ 1.618e f/ sinewave 0.3e f+ panmix
  stereo_out
  t+
again ;

delay-example
