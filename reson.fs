include constants.fs

: yt1 0 floats + ;
: yt2 1 floats + ;
: xt1 2 floats + ;
: xt2 3 floats + ;

: make_reson_struct
  create 4 floats allot ;

: calc_a1 { F: freq F: res -- coeff }
\ a1 = -2r cos(2piFcTs)
  2PIDSR freq f* fcos res -2e f* f* ;

: calc_b0 { F: res -- coeff }
  1e res sq f- 2e f/ ;

: reinit_reson { struct -- }
  0e struct xt1 f!
  0e struct xt2 f!
  0e struct yt1 f!
  0e struct yt2 f! ;

: reson { F: sig reinit struct F: res F: freq -- F: output-sig }
  \ formula described as:
  \ y[n] = b0 x[n] + b1 x[n-1] + b2 x[n-2] - a1 y[n-1] - a2 y[n-2]
  \
  reinit if
    struct reinit_reson
  endif
  res calc_b0 { F: myb0 } myb0 sig f*
  0e struct xt1 f@ f* f+
  myb0 -1e f* struct xt2 f@ f* f+
  freq res calc_a1 struct yt1 f@ f* f-
  res sq struct yt2 f@ f* f- { F: sig-out }  
  \ UPDATE:
  struct yt1 f@ struct yt2 f!
  sig-out struct yt1 f!
  struct xt1 f@ struct xt2 f!
  sig struct xt1 f!
  sig-out ;
