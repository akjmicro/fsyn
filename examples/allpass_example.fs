include ../fsyn.fs

make-allpass-struct ap_struct
make-allpass-struct ap_struct2
make-allpass-struct ap_struct3
make-allpass-struct ap_struct4
make-allpass-struct ap_struct5
make-allpass-struct ap_struct6
ap_struct reinit-allpass
ap_struct2 reinit-allpass
ap_struct3 reinit-allpass
ap_struct4 reinit-allpass
ap_struct5 reinit-allpass
ap_struct6 reinit-allpass

variable saw_signal
variable unity_scale
variable scale_factor
variable freq_factor
0.07e unity_scale f!
unity_scale f@ 17e f/ scale_factor f!
90e 17e f* freq_factor f! 

: allpass_example
begin
  \ saw wave, sent to a variable that we can tap as a 'bus'
  unity_scale f@ 90e bl_saw
  scale_factor f@ freq_factor f@ bl_saw f+
  saw_signal f!
  \ LEFT CHANNEL:
  saw_signal f@
  ap_struct false 10e 3.7e sinewave 20e f+ allpass1
  ap_struct2 false 10e 3.72e sinewave 20e f+ allpass1
  ap_struct3 false 10e 3.741e sinewave 20e f+ allpass1
  0.1e panmix
  \ RIGHT CHANNEL:
  saw_signal f@
  ap_struct4 false 10e 3.6e sinewave 20e f+ allpass1
  ap_struct5 false 10e 3.62e sinewave 20e f+ allpass1
  ap_struct6 false 10e 3.641e sinewave 20e f+ allpass1
  0.9e panmix
  stereo_out 
  t+ \ advance counter
again ;

allpass_example
