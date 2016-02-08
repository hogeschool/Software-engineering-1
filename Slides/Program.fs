open Compile

[<EntryPoint>]
let main argv = 
  do batchProcess FunctionalWeek1.slides "fp_week1" "Giuseppe Maggiore, Francesco Di Giacomo" "Introduction to functional programming and lambda calculus" true false
  0
