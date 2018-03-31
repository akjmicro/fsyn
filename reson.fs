[IFUNDEF] 2PI
include constants.fs
[ENDIF]

: xt1_reson 0 floats + ;
: yt1_reson 1 floats + ;
: xt2_reson 2 floats + ;
: yt2_reson 3 floats + ;

: make_reson_struct
  create 4 floats allot ;

: calc_a1 { F: freq F: res -- coeff }
\ a1 = -2r cos(2piFcTs)
  2PIDSR freq f* fcos res -2e f* f* ;

: calc_b0 { F: res -- coeff }
  1e res sq f- 2e f/ ;

: reinit_reson { struct -- }
  0e struct xt1_reson f!
  0e struct yt1_reson f!
  0e struct xt2_reson f!
  0e struct yt2_reson f! ;

: reson { F: sig struct reinit F: res F: freq -- F: output-sig }
  \ formula described as:
  \ y[n] = b0 x[n] + b1 x[n-1] + b2 x[n-2] - a1 y[n-1] - a2 y[n-2]
  \
  reinit if
    struct reinit_reson
  endif
  res calc_b0 { F: myb0 } myb0 sig f*
  0e struct xt1_reson f@ f* f+
  myb0 -1e f* struct xt2_reson f@ f* f+
  freq res calc_a1 struct yt1_reson f@ f* f-
  res sq struct yt2_reson f@ f* f- { F: sig-out }  
  \ UPDATE:
  struct yt1_reson f@ struct yt2_reson f!
  sig-out struct yt1_reson f!
  struct xt1_reson f@ struct xt2_reson f!
  sig struct xt1_reson f!
  sig-out ;
