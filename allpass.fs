[IFUNDEF] 2PI
include constants.fs
[ENDIF]

\ implementation of 1st-order allpass, and derived low- and highpass
\ filters as side-effects. These are filters w/o Q (resonant peaks).

: xt1_allpass 0 floats + ;
: yt1_allpass 1 floats + ;
: xt2_allpass 2 floats + ;
: yt2_allpass 3 floats + ;
: prvcf_allpass 4 floats + ;
: c_param_allpass 5 floats + ;

: make_allpass_struct
  create 6 floats allot ;

: make_lowpass_struct
  make_allpass_struct ;

: make_highpass_struct
  make_allpass_struct ;

: reinit_allpass { struct -- }
  0e struct xt1_allpass f!
  0e struct yt1_allpass f!
  0e struct xt2_allpass f!
  0e struct yt2_allpass f!
  0e struct prvcf_allpass f!
  0e struct c_param_allpass f! ;

: calc_c { F: freq -- F: coeff }
  pi freq INV_SAMPLE_RATE f* f* ftan { F: factor } 
  factor 1e f- factor 1e f+ f/ ;

: allpass1 { F: sig reinit struct F: cf -- F: output-sig }
  reinit if
    struct reinit_allpass
  endif
  \ if cutoff frequency has changed:
  struct prvcf_allpass f@ cf f<> if
    cf calc_c struct c_param_allpass f!
  endif
  struct c_param_allpass f@ { F: c-factor }
  c-factor sig f*
  struct xt1_allpass f@ f+
  c-factor struct yt1_allpass f@ f* f- { F: y }
  \ UPDATE:
  cf struct prvcf_allpass f!
  y struct yt1_allpass f!
  sig struct xt1_allpass f!
  y ;

: lowpass1 { F: sig reinit struct F: cf -- F: output-sig }
  reinit if
    struct reinit_allpass
  endif
  sig sig reinit struct cf allpass1 f+ 0.5e f* ;

: highpass1 { F: sig reinit struct F: cf -- F: output-sig }
  reinit if
    struct reinit_allpass
  endif
  sig sig reinit struct cf allpass1 f- 0.5e f* ;
