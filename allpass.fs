[IFUNDEF] 2PI
include constants.fs
[ENDIF]

\ implementation of 1st-order allpass, and derived low- and highpass
\ filters as side-effects. These are filters w/o Q (resonant peaks).

: xt1-allpass 0 floats + ;
: yt1-allpass 1 floats + ;
: xt2-allpass 2 floats + ;
: yt2-allpass 3 floats + ;
: prvcf-allpass 4 floats + ;
: c-param-allpass 5 floats + ;

: make-allpass-struct
  create 6 floats allot ;

: make-lowpass-struct
  make-allpass-struct ;

: make-highpass-struct
  make-allpass-struct ;

: reinit-allpass { struct -- }
  0e struct xt1-allpass f!
  0e struct yt1-allpass f!
  0e struct xt2-allpass f!
  0e struct yt2-allpass f!
  0e struct prvcf-allpass f!
  0e struct c-param-allpass f! ;

: calc-c { F: freq -- F: coeff }
  pi freq INV_SAMPLE_RATE f* f* ftan { F: factor } 
  factor 1e f- factor 1e f+ f/ ;

: allpass1 { F: sig reinit struct F: cf -- F: output-sig }
  reinit if
    struct reinit-allpass
  endif
  \ if cutoff frequency has changed:
  struct prvcf-allpass f@ cf f<> if
    cf calc-c struct c-param-allpass f!
  endif
  struct c-param-allpass f@ { F: c-factor }
  c-factor sig f*
  struct xt1-allpass f@ f+
  c-factor struct yt1-allpass f@ f* f- { F: y }
  \ UPDATE:
  cf struct prvcf-allpass f!
  y struct yt1-allpass f!
  sig struct xt1-allpass f!
  y ;

: lowpass1 { F: sig reinit struct F: cf -- F: output-sig }
  reinit if
    struct reinit-allpass
  endif
  sig sig reinit struct cf allpass1 f+ 0.5e f* ;

: highpass1 { F: sig reinit struct F: cf -- F: output-sig }
  reinit if
    struct reinit-allpass
  endif
  sig sig reinit struct cf allpass1 f- 0.5e f* ;
