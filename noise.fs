variable div-by
1 31 lshift 1 - div-by !

variable last-num
0 last-num !
: xorshift-rand
  last-num @ 0= if
    utime drop last-num !
  then
  last-num @
  dup 12 rshift xor
  dup 25 lshift xor
  dup 27 rshift xor
  dup last-num !
  0x2545F4914F6CDD1D um* drop 
  1 32 lshift 1 - and ;

: noise ( amp -- sig )
  xorshift-rand s>f div-by @ s>f f/
  1.0e f- f* ;

: randint ( range -- int )
  s>f f2/ fdup noise f+ floor f>s ;
