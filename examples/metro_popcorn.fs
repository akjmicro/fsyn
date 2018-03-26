include ../fsyn.fs

: mymetro
begin
  0.3e 1.0e metro 0.25e panmix
  0.3e 4.7e metro 0.35e panmix 
  0.3e 3.5e metro 0.50e panmix
  0.3e 2.8e metro 0.50e panmix
  0.3e 5.5e metro 0.75e panmix
  stereo_out
  t+ \ advance counter
again ;

mymetro
