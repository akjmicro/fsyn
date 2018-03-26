\ A sequencer/scheduler should be structured as an array, needing the
\ following fields:
\   * variable to be updated by the sequencer
\   * Start time of the entire sequence
\   * end time of the entire sequence
\   * a series of time/value pairs, i.e. at X ticks, update the variable
\    (given above) to the value Y.
\   * We can have a setup word that will do the appropriate conversion
\     between seconds and sample-clock ticks.

: make-seq
  noop ; \ TODO

: seq
  noop ; \ TODO
