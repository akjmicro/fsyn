variable div_by
-1 1 rshift div_by !

variable last_num 0 last_num !
: xorshift-rand
  last_num @ 0= if
    utime drop last_num !
  then
  last_num @
  dup 12 rshift xor
  dup 25 lshift xor
  dup 27 rshift xor
  dup last_num !
  0x2545F4914F6CDD1D um* drop
  1 rshift ;

: noise ( amp -- sig )
  xorshift-rand s>f div_by s>f f/ 1.0e fmod 2.0e f*
  1.0e f- f* ;

: randint ( range -- int )
  s>f f2/ fdup noise f+ floor f>s ;
