\ just so we're clear, we're using decimal:
decimal

\ definition of 2pi
pi 2e f* fconstant 2PI

\ sample rate and bit depth here
48000e fconstant SAMPLE_RATE  \ can be 8000e, 44100e, etc. etc

\ more efficient, for multiplication:
SAMPLE_RATE 1/f fconstant INV_SAMPLE_RATE

1 31 lshift 1 - s>f fconstant BIT_DEPTH  \ 32-bit audio sample depth

\ useful in normalized frequency calculations:
2PI SAMPLE_RATE f/ fconstant 2PIDSR

\ epsilon, a very small value representing a unit of sample height
\ In other words, our minimal amplitude displacement:
BIT_DEPTH 1/f fconstant EPSILON

\ some shortcuts:
: >> rshift ;
: << lshift ;
: % mod ;
: & and ;
: | or ;
: ^ xor ;
: sq fdup f* ;
: fceil 0.5e f+ fround ;

\ we need to create a common, general space for phasor accumulators.
\ that happens here:

128 constant PHASOR_ARRAY_SIZE

create phasor-acc-array PHASOR_ARRAY_SIZE floats allot
: init-phasor-array 
  PHASOR_ARRAY_SIZE 0 do
  phasor-acc-array i floats + 0.0e f!
  loop ;

init-phasor-array

variable phasor_count 0 phasor_count !

: reset-phasor-count
  0 phasor_count ! ;
