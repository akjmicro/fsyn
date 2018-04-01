[IFUNDEF] 2PI
include constants.fs
[ENDIF]

\ just to be clear, we're using decimal
decimal

: seconds2samples ( seconds -- samples )
  SAMPLE_RATE f* f>s ;

: freq2sample_phase ( freq -- sample-phase )
  INV_SAMPLE_RATE f* ;

: phasor-pointer
  phasor-acc-array phasor_count @ floats + ;

: phasor ( freq -- phase )
  \ get current phase; wrap around 0-1:
  freq2sample_phase 
  \ grab the accumulator value:
  phasor-pointer f@
  \ add, wrap-around from 0.0-1.0
  f+ 1.0e fmod
  \ write back current value:
  fdup phasor-pointer f!
  \ update count reference (this will be reset by the clock advance) :
  1 phasor_count +! ;

\ a simple metronome trigger:
: metro { F: amp F: freq -- sig }
  freq phasor 0.0005e f<= if amp else 0.0e endif ;

\ a gate signal:
: gate { F: amp F: width F: freq -- sig }
  freq phasor width f<= if amp else 0.0e endif ;

\ phase --> sine
: sine ( phase -- amp )
  2PI f* fsin ;

\ uses frequency
: sinewave ( amp freq -- sig )
  phasor sine f* ;

\ FM osc; basic carrier/modulator pair
: fm { F: amp F: index F: mod_ratio F: carrier_freq -- sig }
  index carrier_freq mod_ratio f* sinewave
  carrier_freq phasor f+ 1.0e fmod sine amp f* ;

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
