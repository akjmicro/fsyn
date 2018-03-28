include ../fsyn.fs

create-delay-line mydelay
mydelay init-delay-line

0.04e 0.4e create-port-struct port-struct
fvariable fmamp 0e fmamp f!

fvariable delay-hold 0e delay-hold f!

: add-feedback
delay-hold f@ f* f+ ;

: delay-example
BEGIN
  \ add some time-based interest, ala 'BYTEBEAT':
  t 149 * t 107 * - 219893 * 219889 % t ^ 219829 & s>f 219889e f/
  \ trigger for portamento envelope 
  0.5e f* 0.7575001e metro 
  \ reference array for port and portamento call
  port-struct port
  \ route to 'fmamp' variable:
  fmamp f!

  \ FM oscillator:
  fmamp f@ 0.99e 1.5023e 120e fm
  \ from previous rounds (feedback)
  0.66e add-feedback 

  \ write fm signal and feedback variable to delay,
  \ original signal is copied and stays on stack:
  mydelay delay-write
  0.3e 0.3e sinewave 0.5e f+ panmix

  \ tap the delay to get a second signal:
  mydelay 0.3e delay-tap
  \ send it to the feedback variable for next time:
  fdup delay-hold f!
  0.15e 0.417e sinewave 0.5e f+ panmix

  stereo_out
  t+
AGAIN ;

delay-example
