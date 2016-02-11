open Compile

[<EntryPoint>]
let main argv = 
  do batchProcess FunctionalWeek1.slides "INFSEN02-1-Lec1-Lambda-calculus" "Giuseppe Maggiore, Francesco Di Giacomo" "Introduction to functional programming and lambda calculus" true false
  do batchProcess FunctionalWeek2.slides "INFSEN02-1-Lec2-Delta-rules" "Giuseppe Maggiore, Francesco Di Giacomo" "Delta rules" true false
//  do batchProcess FunctionalWeek3.slides "INFSEN02-1-Lec3-Typing" "Giuseppe Maggiore, Francesco Di Giacomo" "Type system and type inference" true false
  0
