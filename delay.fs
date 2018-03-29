[IFUNDEF] 2PI
include constants.fs
[ENDIF]

variable 10SEC_STORAGE_SIZE
SAMPLE_RATE f>s 10 * 10SEC_STORAGE_SIZE !

: create-delay-line ( name -- )
  create 10SEC_STORAGE_SIZE @ floats 2 floats + allot ;

: init-delay-line { delay-struct -- }
  10SEC_STORAGE_SIZE @ delay-struct !  \ loop size
  0 delay-struct 1 cells + !          \ write-position
  10SEC_STORAGE_SIZE @ 0 DO
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
  t loop-size mod
  delay-struct set-write-position
  sig ;

: interpolate { F: fl-val F: ceil-val F: fp-offset -- F: out-val }
  ceil-val fdup fl-val f- fp-offset 1.0e fmod f* f- ;

: delay-read { delay-struct F: time -- delsig }
  \ get write-position and loop-size (circular buffer) reference variables:
  delay-struct @ { loop-size }
  delay-struct get-write-position { write-position }
  
  \ calculate real time offset, and integer time offset in samples:
  time SAMPLE_RATE f* { F: real-offset }
  real-offset floor f>s { floor-samp-offset }
  real-offset fceil f>s { ceil-samp-offset }

  \ where are we on the circular buffer 'clock'?
  write-position floor-samp-offset - loop-size mod { floor-samp-addr }
  write-position ceil-samp-offset - loop-size mod { ceil-samp-addr }

  \ get the value, but account for the data cell size and offset
  \ by two, due to the loop-size and write-position slots:
  delay-struct floor-samp-addr floats + 2 floats + f@
  delay-struct ceil-samp-addr floats + 2 floats + f@

  \ average the two values by linear 'distance' interpolation:
  real-offset interpolate ;
