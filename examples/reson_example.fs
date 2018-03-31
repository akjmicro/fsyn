include ../fsyn.fs

make_reson_struct resstruct
resstruct reinit_reson

: reson_example
begin
  \ some noise
  0.9e noise
  resstruct false
  \ resonance (first arg) should be between 0 and 1:
  0.99999e 250e reson 4.9e f*
  0.5e panmix
  stereo_out 
  t+ \ advance counter
again ;

reson_example
