# Deprecation notice

I leave the notes below for those interested, but note that I plan on continuing any DSP-related development started here in my own RPN, stack-based language, `dclang`: http://github.com/akjmicro/dclang

# Fsyn

Fsyn is a Forth-based synthesis-package, developed using gforth and Linux. 
Originally inspired by the excellent package
[Sporth](http://paulbatchelor.github.io/proj/sporth.html) by Paul Batchelor,
I wanted to see how far I could go writing something similar, but have the
audio units written as much in possible in Forth and/or, have Forth itself
available for making colon word definitions (the interpreter in Sporth
doesn't have this feature).

Fsyn is still in very early development, so not usuable for much, but early
results are showing that there is promise. Maybe if I get some Forth-savvy
people to join this effort, it will go faster!

## Features

- Most audio units are written in Forth from the ground-up
- Stack-based/Postfix paradigm
- Small and fast
- Extensible with C (e.g. the PortMIDI module)
- Tentative plans may include ability to host LV2 plugins and script them

## Installation

You will need to install a Forth interpreter. I highly recommend Gforth, as
this is what I use to develop Fsyn with. Gforth has a nice C-interface, and
this package will surely lean on that from time to time.


## Examples

Several examples demonstrating features can be found in 
the [examples](examples) folder of the repository. As this project is in
early development phase, there will be more (and fuller examples)
forthcoming (pun intended!).

Currently, the way to get audio out of Fsyn is to run an example and pipe
through 'aplay'. Since the default setting is 32-bit audio at a sample rate
of 48000, you'd do this, for example:

```
   gforth-fast bells_of_doom.fs | aplay -f S32_LE -r 48000 -c 2
```
