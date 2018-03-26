include ../fsyn.fs

make_allpass_struct ap_struct
make_allpass_struct ap_struct2
make_allpass_struct ap_struct3
make_allpass_struct ap_struct4
make_allpass_struct ap_struct5
make_allpass_struct ap_struct6
ap_struct reinit_allpass
ap_struct2 reinit_allpass
ap_struct3 reinit_allpass
ap_struct4 reinit_allpass
ap_struct5 reinit_allpass
ap_struct6 reinit_allpass

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
  false ap_struct 180e 3.7e sinewave 200e f+ allpass1
  false ap_struct2 180e 3.72e sinewave 200e f+ allpass1
  false ap_struct3 180e 3.741e sinewave 200e f+ allpass1
  0.1e panmix
  \ RIGHT CHANNEL:
  saw_signal f@
  false ap_struct4 180e 3.6e sinewave 200e f+ allpass1
  false ap_struct5 180e 3.62e sinewave 200e f+ allpass1
  false ap_struct6 180e 3.641e sinewave 200e f+ allpass1
  0.9e panmix
  stereo_out 
  t+ \ advance counter
again ;

allpass_example
