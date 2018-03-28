[IFUNDEF] 2PI
include constants.fs
[ENDIF]

: xt1_bq 0 floats + ;
: yt1_bq 1 floats + ;
: xt2_bq 2 floats + ;
: yt2_bq 3 floats + ;
: fltcon_bq 4 floats + ;
: resonance_bq 5 floats + ;
: alpha_bq 6 floats + ;
: beta_bq 7 floats + ;
: gamma_bq 8 floats + ;
: denom_bq 9 floats + ;
: b0_bq 10 floats + ;
: b1_bq 11 floats + ;
: b2_bq 12 floats + ;
: a0_bq 13 floats + ;
: a1_bq 14 floats + ;
: a2_bq 15 floats + ;

: make_biquad_struct
  create 16 floats allot ;

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

: biquad_compute { F: sig reinit struct -- sig-out }
  \ the actual transformative equation:
  struct b0_bq f@ sig f*
  struct b1_bq f@ struct xt1_bq f@ f* f+
  struct b2_bq f@ struct xt2_bq f@ f* f+
  struct a1_bq f@ struct yt1_bq f@ f* f-
  struct a2_bq f@ struct yt2_bq f@ f* f-
  struct a0_bq f@ f/
  ( sig-out_now_on_stack )
  fdup ( sig-out sig-out )
  \ update state:
  struct xt1_bq f@ struct xt2_bq f!
  sig struct xt1_bq f!      \ xn (sig) --> xt1_bq
  struct yt1_bq f@ struct yt2_bq f!
  struct yt1_bq f! ;  ( sig-out )

: reinit_biquad { struct -- }
  0.0e struct xt1_bq f!
  0.0e struct xt2_bq f!
  0.0e struct yt1_bq f!
  0.0e struct yt2_bq f! ;

: biquad_helper { F: sig reinit struct F: res F: flc -- sig-out }
  res struct resonance_bq f!
  flc struct fltcon_bq f!
  res flc calc_alpha struct alpha_bq f!
  res flc calc_beta struct beta_bq f!
  flc calc_gamma struct gamma_bq f!
  struct fltcon_bq f@ struct beta_bq f@ struct gamma_bq f@ struct alpha_bq f@
  calc_denominator struct denom_bq f!
  \ put b0 into struct:
  struct alpha_bq f@ sq struct beta_bq f@ sq f+ 1.5e f*
  struct denom_bq f@ f/ struct b0_bq f! 
  \ put b1 into struct:
  struct b0_bq f@ struct b1_bq f!
  \ put b2 into struct:
  0.0e struct b2_bq f!
  \ put a0 into struct:
  1.0e struct a0_bq f!
  \ put a1 into struct:
  -2.0e struct resonance_bq f@ f* struct fltcon_bq f@ fcos f* struct a1_bq f!
  \ put a2 into struct:
  struct resonance_bq f@ sq struct a2_bq f!  

  \ reinit?
  reinit if struct reinit_biquad endif
  
  \ activate actual transformational equations:  
  sig reinit struct biquad_compute ;

: biquad { F: sig reinit struct F: res F: cutoff -- sig-out }
  sig reinit struct res cutoff calc_fltcon biquad_helper ;
