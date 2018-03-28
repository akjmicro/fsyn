[IFUNDEF] 2PI
include constants.fs
[ENDIF]

\ just to be clear, we're using decimal
decimal

: seconds2samples ( seconds -- samples )
  SAMPLE_RATE f* f>s ;

: freq2sample_phase ( freq -- sample-phase )
  INV_SAMPLE_RATE f* ;

\ this scales between 0 and 1
: phasor ( freq -- phase )
  freq2sample_phase t s>f f* 
  1e fmod ;

\ a simple metronome trigger:
: metro ( amp freq -- sig )
  phasor 0.0005e f<= if noop else fdrop 0e then ;

\ a gate signal:
: gate ( amp width freq -- sig )
  phasor f>= if noop else fdrop 0e then ;

\ phase --> sine
: sine ( phase -- amp )
  2PI f* fsin ;

\ uses frequency
: sinewave ( amp freq -- sig )
  phasor sine f* ;

\ FM osc; basic carrier/modulator pair
: fm { F: amp F: index F: mod_ratio F: carrier_freq -- sig }
  index carrier_freq mod_ratio f* sinewave
  carrier_freq phasor f+ 1e fmod sine amp f* ;

\ normalizing factor
: find_saw_normalizing_factor ( num_harms -- ans )
0e 1 + 1
do
  1e i s>f f/ f+
loop
1e fswap f/ ;

\ bl_saw table:
create bl_saw_table 1024 floats allot
: make_bl_saw_table ( )
  1024 0
  do
    0e \ running sum
    17 1 do
      j i * 1024 mod s>f 1024e f/ sine   \ phase component of Nth harmonic
      1e i s>f f/ f*                     \ scale harmonic amplitude
      f+                                 \ add to running sum
    loop
    1 find_saw_normalizing_factor f*
    bl_saw_table i floats + f!  \ dump to table
  loop ;

make_bl_saw_table
\ band-limited sawtooth oscillator:
: bl_saw { F: amp F: freq -- sig }
  freq phasor 1024e f* f>s
  floats bl_saw_table + f@ 
  amp f* ;
