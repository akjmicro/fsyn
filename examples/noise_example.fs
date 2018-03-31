include ../fsyn.fs

make_biquad_struct bqstruct
bqstruct reinit_biquad

0.04e 2.5e create-port-struct port-struct
variable global-env 0e global-env f!

: mynoise
begin
  \ metronome trigger
  1e 0.25e metro port-struct port global-env f!
  global-env f@ 0.5e f* noise
    \ biquad data structure
    bqstruct false
    \ resonance
    0.94e
    \ cutoff is controlled by grabbing global envelope
    \ and scaling/adding a constant
    global-env f@ 400e f* 300e f+
  biquad 0.1e f*
  0.5e panmix
  stereo_out
  t+
again ;

mynoise
