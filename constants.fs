\ just so we're clear, we're using decimal:
decimal

\ some shortcuts:
: >> rshift ;
: << lshift ;
: % mod ;
: & and ;
: | or ;
: ^ xor ;
: sq fdup f* ;

\ definition of 2pi
pi 2e f* fconstant 2PI

\ sample rate and bit depth here
48000e fconstant SAMPLE_RATE  \ can be 8000e, 44100e, etc. etc
\ more efficient, for multiplication:
1e SAMPLE_RATE f/ fconstant INV_SAMPLE_RATE
1 23 lshift 1 - s>f fconstant BIT_DEPTH \ 24-bit audio,
                                        \ can be 255e or 32767e 
                                        \ (8-bit or 16-bit audio)
2PI SAMPLE_RATE f/ fconstant 2PIDSR
