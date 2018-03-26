include constants.fs

: calc_fltcon ( cutoff -- fcon )
  2PI INV_SAMPLE_RATE f* f* ;

: calc_alpha { F: res F: fltcon -- alpha }
  1e res 2e f* fltcon fcos sq f*
  res sq fltcon 2e f* fcos f* f+
  f- ;

: calc_beta { F: res F: fltcon -- beta }
  res fdup f* fltcon 2e f* fsin f*
  res 2e f* fltcon fcos fltcon fsin f* f* 
  f- ;

: calc_gamma ( fltcon -- gamma )
  fcos 1e f+ ;

: calc_denominator ( fltcon beta gamma alpha -- denom )
  f* fswap frot fsin f*
  fover fover f+ frot frot f-
  sq fswap sq f+ fsqrt ;

: fltcon 0 floats + ;
: resonance 1 floats + ;
: alpha 2 floats + ;
: beta 3 floats + ;
: gamma 4 floats + ;
: denom 5 floats + ;
: b0 6 floats + ;
: b1 7 floats + ;
: b2 8 floats + ;
: a0 9 floats + ;
: a1 10 floats + ;
: a2 11 floats + ;
: xnm1 12 floats + ;
: xnm2 13 floats + ;
: ynm1 16 floats + ;
: ynm2 14 floats + ;

: make_biquad_struct
  create 15 floats allot ;

: biquad_compute { F: sig reinit struct -- sig-out }
  \ the actual transformative equation:
  struct b0 f@ sig f*
  struct b1 f@ struct xnm1 f@ f* f+
  struct b2 f@ struct xnm2 f@ f* f+
  struct a1 f@ struct ynm1 f@ f* f-
  struct a2 f@ struct ynm2 f@ f* f-
  struct a0 f@ f/
  ( sig-out_now_on_stack )
  fdup ( sig-out sig-out )
  \ update state:
  struct xnm1 f@ struct xnm2 f!
  sig struct xnm1 f!      \ xn (sig) --> xnm1
  struct ynm1 f@ struct ynm2 f!
  struct ynm1 f! ;  ( sig-out )

: reinit_biquad { struct -- }
  0.0e struct xnm1 f!
  0.0e struct xnm2 f!
  0.0e struct ynm1 f!
  0.0e struct ynm1 f! ;

: biquad_helper { F: sig reinit struct F: res F: flc -- sig-out }
  res struct resonance f!
  flc struct fltcon f!
  res flc calc_alpha struct alpha f!
  res flc calc_beta struct beta f!
  flc calc_gamma struct gamma f!
  struct fltcon f@ struct beta f@ struct gamma f@ struct alpha f@
  calc_denominator struct denom f!
  \ put b0 into struct:
  struct alpha f@ sq struct beta f@ sq f+ 1.5e f*
  struct denom f@ f/ struct b0 f! 
  \ put b1 into struct:
  struct b0 f@ struct b1 f!
  \ put b2 into struct:
  0.0e struct b2 f!
  \ put a0 into struct:
  1.0e struct a0 f!
  \ put a1 into struct:
  -2.0e struct resonance f@ f* struct fltcon f@ fcos f* struct a1 f!
  \ put a2 into struct:
  struct resonance f@ sq struct a2 f!  

  \ reinit?
  reinit if struct reinit_biquad endif
  
  \ activate actual transformational equations:  
  sig reinit struct biquad_compute ;

: biquad { F: sig reinit struct F: res F: cutoff -- sig-out }
  sig reinit struct res cutoff calc_fltcon biquad_helper ;
