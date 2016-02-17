open Compile

[<EntryPoint>]
let main argv = 
//  do batchProcess FunctionalWeek1.slides "INFSEN02-1-Lec1-Lambda-calculus" "The INFDEV@HR Team" "Introduction to functional programming and lambda calculus" false false
//  do batchProcess FunctionalWeek2.slides "INFSEN02-1-Lec2-Delta-rules" "The INFDEV@HR Team" "Delta rules" false false
//  do batchProcess FunctionalWeek3.slides "INFSEN02-1-Lec3-Data-structures" "The INFDEV@HR Team" "Data structures" false false
//  do batchProcess FunctionalWeek4.slides "INFSEN02-1-Lec4-Let-Letrec-FSharp" "The INFDEV@HR Team" "Let, let-rec, and F\# translations" false false
  do batchProcess FunctionalWeek5.slides "INFSEN02-1-Lec5-Types" "The INFDEV@HR Team" "Types, inference, and F\# data types" true false
  // add notice about homework
  // some terms in F# rendering are too verbose, and should be on a single line

  // Practicum 1
  // Lecture 7 Haskell translations and laziness
  // Practicum 2
  // Lecture 8 patterns and practices: auto-updateable entities, monads, coroutines, auto-updateable entities with coroutines
  0
