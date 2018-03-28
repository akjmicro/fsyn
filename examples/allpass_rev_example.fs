include ../fsyn.fs

make_allpass_struct ap_struct
make_allpass_struct ap_struct2
ap_struct reinit_allpass
ap_struct2 reinit_allpass

0.01e 0.1e create-port-struct port-struct

variable port-signal
variable saw_signal
variable unity_scale
variable scale_factor
variable freq_factor
0.07e unity_scale f!
unity_scale f@ 17e f/ scale_factor f!
90e 17e f* freq_factor f! 

: allpass_rev_example
begin
  \ metronome signal, altered to create a decay envelope:
  0.2e 0.5e metro port-struct port port-signal f!
  \ saw wave, sent to a variable that we can tap as a 'bus'
  unity_scale f@ port-signal f@ f* 90e bl_saw
  scale_factor f@ port-signal f@ f* freq_factor f@ bl_saw f+
  saw_signal f!
  \ LEFT CHANNEL:
  saw_signal f@
  false ap_struct 25e allpass1
  0.1e panmix
  \ RIGHT CHANNEL:
  saw_signal f@
  false ap_struct2 25e allpass1
  0.9e panmix
  stereo_out 
  t+ \ advance counter
again ;

allpass_rev_example
