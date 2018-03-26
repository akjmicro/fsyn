include constants.fs

\ A portamento can be roughly done by grabbing and saving a previous sample,
\ considering that a starting point for the next sample 'target', and making
\ the decision: is this a rising slope or a falling slope? We have half-way
\ point parameters for both. The algorithm is basically:
\   * fetch the previous sample value (call it 'old-sig')
\   * fetch the current signal (called 'new-sig') 
\   * if the previous sample is lower than the current signal, add:
\       new-sig old-sig - 2 / rise-time-in-samples /
\       and save it to old-sig
\   * if the previous sample is higher than the current signal, add:
\     new_sig old_sig - 2 / fall-time-in-samples /
\ Neat bonus: this 'port' ugen can be used to make envelopes!
\ See the example file 'bell_of_doom.fs' for an example.

: port-factor ( time-in-ms -- factor )
  1e fswap 0.5e f* SAMPLE_RATE f* f/ ;  

: create-port-struct ( rise-time-ms fall_time-ms struct-name -- struct )
  fswap create
  0.0e f, port-factor f, port-factor f, ;

: get-rising-factor ( accum-struct -- float )
  1 cells + f@ ;

: get-falling-factor ( accum-struct -- float )
  2 cells + f@ ;

: get-accumulated-signal ( accum-struct -- sig )
  0 cells + f@ ;

: port ( sig accum_struct -- out-sig )
  dup dup get-accumulated-signal ( ac ac ) ( sig ac@ )
  ftuck                      ( ac ac ) ( ac@ sig ac@ )
  fover fover                ( ac ac ) ( ac@ sig ac@ sig ac@ ) 
  f>                         ( ac ac ) ( ac@ sig ac@ bool )
  if ( rising )              ( ac ac ) ( ac@ sig ac@ )
    f- get-rising-factor f*   ( ac ) ( ac@ res )
  else ( falling )
    f- get-falling-factor f*  ( ac ) ( ac@ res )
  endif     
  f+ fdup f! ;     ( new )
