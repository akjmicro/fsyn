include ../fsyn.fs

make_biquad_struct bqstruct
bqstruct reinit_biquad

: bl_saw_example
begin
  \ 1st voice
  0.5e 51e bl_saw
  0 bqstruct 
  \ resonance:
  0.46e 
  \ cutoff freq:
  2700e 0.3e sinewave 2800e f+ 
  biquad
  0.2e f*
  0.083e panmix
  
  \ 2nd voice
  0.5e 51.107e bl_saw
  0 bqstruct 
  \ resonance:
  0.45e 
  \ cutoff freq:
  2700e 0.317e sinewave 2800e f+ 
  biquad
  0.2e f*
  0.25e panmix
  
  \ 3rd voice
  0.5e 51.213e bl_saw
  0 bqstruct 
  \ resonance:
  0.417e 
  \ cutoff freq:
  2700e 0.298e sinewave 2800e f+ 
  biquad
  0.2e f*
  0.417e panmix

  \ 4th voice
  0.5e 51.011e bl_saw
  0 bqstruct 
  \ resonance:
  0.5e 
  \ cutoff freq:
  2700e 0.136e sinewave 2800e f+ 
  biquad
  0.2e f*
  0.583e panmix

  \ 5th voice
  0.5e 51.141e bl_saw
  0 bqstruct 
  \ resonance:
  0.5e 
  \ cutoff freq:
  2700e 0.311e sinewave 2800e f+ 
  biquad
  0.2e f*
  0.75e panmix

  \ 6th voice
  0.5e 50.718e bl_saw
  0 bqstruct 
  \ resonance:
  0.5e 
  \ cutoff freq:
  2700e 0.372e sinewave 2800e f+ 
  biquad
  0.2e f*
  0.916e panmix

  stereo_out
  ( ADVANCE SAMPLE COUNTER )
  t+ \ advance counter
again ;

bl_saw_example
