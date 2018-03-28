[IFUNDEF] 2PI
include constants.fs
[ENDIF]

\ WARNING: STILL UNDER DEVELOPMENT. Pardon our dust!

variable 4SEC_STORAGE_SIZE
SAMPLE_RATE f>s 4 * 4SEC_STORAGE_SIZE !

: create-delay-line ( name -- )
  create 4SEC_STORAGE_SIZE @ floats 2 floats + allot ;

: init-delay-line { delay-struct -- }
  4SEC_STORAGE_SIZE @ delay-struct !  \ loop size
  0 delay-struct 1 cells + !          \ write-position
  4SEC_STORAGE_SIZE @ 0 DO
    0.0e delay-struct i floats + 2 floats + f!
  LOOP ;

: get-write-position ( delay-struct -- pos )
  1 cells + @ ;

: set-write-position ( delay-struct -- pos )
  1 cells + ! ;

: delay-write { F: sig delay-struct -- sig }
  delay-struct @ { loop-size }
  delay-struct get-write-position { write-position }
  \ write current signal:
  sig
  delay-struct write-position loop-size mod floats + 2 floats +
  f!
  \ increment write-position
  \ write-position 1 + loop-size mod
  t loop-size mod
  delay-struct set-write-position
  sig ;

: delay-tap { delay-struct F: time -- delsig }
  time SAMPLE_RATE f* floor f>s { floor-samp-offset }
  time SAMPLE_RATE f* fceil f>s { ceil-samp-offset }
  delay-struct get-write-position { wp }
  delay-struct @ { loop-size }
  wp floor-samp-offset - loop-size mod { floor-samp-addr }
  wp ceil-samp-offset - loop-size mod { ceil-samp-addr }
  delay-struct floor-samp-addr floats + 2 floats + f@
  delay-struct ceil-samp-addr floats + 2 floats + f@
  \ average the two values:
  f+ 2.0e f/ ;

: delay-fb { F: sig delay-struct F: time F: feedback -- sigout }
  sig
  delay-struct time delay-tap feedback f*
  f+ f2/
  delay-struct delay-write ;
