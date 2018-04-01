[IFUNDEF] 2PI
include constants.fs
[ENDIF]

SAMPLE_RATE f>s 10 * constant 10SEC_STORAGE_SIZE
3 constant DELAY_HEADER_SIZE
DELAY_HEADER_SIZE 10SEC_STORAGE_SIZE + constant DELAY_STRUCT_SIZE

: create-delay-line ( name -- )
  create DELAY_STRUCT_SIZE floats allot ;

: init-delay-line { delay-struct -- }
  10SEC_STORAGE_SIZE delay-struct !    \ max loop size
  0 delay-struct 1 floats + !           \ write-position
  0.0e delay-struct 2 floats + f!       \ fb holding pen
  10SEC_STORAGE_SIZE 0 DO
    0.0e delay-struct i floats + DELAY_HEADER_SIZE floats + f!
  LOOP ;

: get-write-position ( delay-struct -- pos )
  1 floats + @ ;

: set-write-position ( delay-struct -- pos )
  1 floats + ! ;

: fbcell-read
  2 floats + f@ ;

: fbcell-write
  2 floats + f! ;

: delay-write { F: sig delay-struct -- sig }
  delay-struct @ { loop-size }
  delay-struct get-write-position { write-position }
  \ write current signal, adding feedback:
  sig delay-struct fbcell-read f+ fdup
  delay-struct write-position loop-size mod floats +
  DELAY_HEADER_SIZE floats + f!
  \ increment write-position
  t loop-size mod
  delay-struct set-write-position ;

: interpolate { F: fl-val F: ceil-val F: fp-offset -- F: out-val }
  ceil-val fdup fl-val f- fp-offset 1.0e fmod f* f- ;

: delay-read { delay-struct F: time F: fb -- delsig }
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
  delay-struct floor-samp-addr floats + DELAY_HEADER_SIZE floats + f@
  delay-struct ceil-samp-addr floats + DELAY_HEADER_SIZE floats + f@

  \ average the two values by linear 'distance' interpolation:
  real-offset interpolate

  \ write back the result to feedback, which will be added to the next
  \ round of live signal in 'delay-write':
  fdup fb f* delay-struct fbcell-write ;
