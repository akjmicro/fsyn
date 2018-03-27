include ../fsyn.fs

\ portamento variables
0.04e 5e create-port-struct port-struct
0.04e 2e create port-struct port-struct2

\ 'patch bay' variables
variable fmamp 0e fmamp f!
variable fmamp2 0e fmamp2 f!

: bells_of_doom
begin
  \ trigger for portamento envelope 
  0.4e 0.2e metro 
  \ reference array for port and portamento call
  port-struct port
  \ route to 'fmamp' variable:
  fmamp f!

  \ sig for 2nd portamento envelope
  \ sig for portamento envelope 
  0.8e 0.5e metro 
  \ reference array for port and portamento call
  port-struct2 port
  \ route to 'fmamp2' variable:
  fmamp2 f!

  \ FM oscillator:
  fmamp f@ 0.9e 0.5723e 230e fm
  \ moving pan signal
  0.4e 0.13e sinewave 0.41e f+ panmix

  \ another FM oscillator:
  fmamp2 f@ 0.9e 0.7e 164e fm
  \ as above (metronome)
  \ another moving pan signal:
  0.4e 0.1e sinewave 0.41e f+ fabs panmix
  
  \ send outputs:
  stereo_out

  t+ \ advance sample clock, clear channels
again ;

bells_of_doom
