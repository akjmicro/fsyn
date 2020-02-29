require ../fsyn.fs

make-allpass-struct ap_struct
ap_struct reinit-allpass

: highpass_example
begin
  \ some noise
  0.3e noise
  ap_struct false
  0.5e phasor sq 10000e f* 20e f+ highpass1
  0.5e panmix
  stereo_out 
  t+ \ advance counter
again ;

highpass_example
