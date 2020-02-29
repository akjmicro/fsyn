include ../fsyn.fs

make-allpass-struct ap_struct
ap_struct reinit-allpass

: lowpass_example
begin
  \ some noise
  0.3e noise
  ap_struct false
  \ notice how make make a decending sawtooth by mirroring the phasor
  \ via "1 - phasor signal": 
  1e 0.5e phasor f- sq 10000e f* 20e f+ lowpass1
  0.5e panmix
  stereo_out 
  t+ \ advance counter
again ;

lowpass_example
