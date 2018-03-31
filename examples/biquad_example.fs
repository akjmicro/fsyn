include ../fsyn.fs

make_biquad_struct bqstruct
bqstruct reinit_biquad

: biquad_example
begin
  \ 1st voice
  0.3e 1.5e 1e 69.951e fm
  0.3e 1.5e 1e 70e     fm
  0.3e 2.5e 1e 69.971e fm
  f+ f+
  bqstruct false 
  \ resonance:
  0.5e 
  \ cutoff freq:
  700e 0.3e sinewave 1000e f+ 
  biquad
  0.2e f*
  0.05e panmix
  
  \ 2nd voice
  0.3e 1.5e 1e 70.127e fm
  0.3e 1.5e 1e 70.103e fm
  0.3e 2.5e 1e 70.451e fm
  f+ f+
  bqstruct false
  \ resonance:
  0.5e 
  \ cutoff freq:
  700e 0.3e sinewave 1000e f+ 
  biquad
  0.2e f*
  0.95e panmix
  
  stereo_out
  ( ADVANCE SAMPLE COUNTER )
  t+ \ advance counter
again ;

biquad_example
