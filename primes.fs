( to return if a number is a prime )

: is-2-3-5
dup ( x x )
2 ( x x 2 )
= ( x -1 )
swap dup ( -1 x x )
3 = ( -1 x 0 )
swap dup ( -1 0 x x )
5 = ( -1 0 x 0 )
swap drop or or ; 

: is-div-2-3-5
dup ( x x )
2 ( x x 2 )
mod 0= ( x tf )
if drop true exit endif
dup 3 mod 0=
if drop true exit endif
dup 5 mod 0=
if drop true exit endif
drop false ;

( for wheel rotation setup )
create wheel
0 , 4 , 2 , 4 , 2 , 4 , 6 , 2 , 6 ,

: wheel-add
wheel @ 1 + cells wheel +
@ + 
wheel @ 1 + 8 mod
wheel ! ;

: is-prime
dup 2 <
if drop false exit endif
dup is-2-3-5
if drop true exit endif
dup is-div-2-3-5
if drop false exit endif
7
BEGIN
dup ( n 7 7 )
( this is a 64-bit limit )
3037000499 > ( n 7 bool )
if
  drop drop true exit
endif
2dup ( n 7 n 7 )
dup ( n 7 n 7 7 )
* ( n 7 n 49 )
>= ( n 7 bool )
WHILE 
  2dup mod 0= 
  if
    drop drop false exit
  endif
  wheel-add
REPEAT
drop drop true ;
