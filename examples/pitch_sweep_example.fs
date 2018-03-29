include ../fsyn.fs

\ this is included as a simple example, but was spurned by the need to
\ prove that the new phasor code was behaving as expected, and allowing
\ for smooth frequency transitions in oscillators, etc.

: sweep
begin
  0.2e 0.2e phasor 250e f* 60e f+ sinewave
  0.5e panmix
  stereo_out
  t+
again ;

sweep

