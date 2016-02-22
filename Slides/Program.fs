open Compile

[<EntryPoint>]
let main argv = 
//  do batchProcess LatexDefinition.generatePresentation FunctionalWeek1.slides "INFSEN02-1-Lec1-Lambda-calculus" "The INFDEV@HR Team" "Introduction to functional programming and lambda calculus" true false
//  do batchProcess LatexDefinition.generatePresentation FunctionalWeek2.slides "INFSEN02-1-Lec2-Delta-rules" "The INFDEV@HR Team" "Delta rules" true false
//  do batchProcess LatexDefinition.generatePresentation FunctionalWeek3.slides "INFSEN02-1-Lec3-Data-structures" "The INFDEV@HR Team" "Data structures" true false
//  do batchProcess LatexDefinition.generatePresentation FunctionalWeek4.slides "INFSEN02-1-Lec4-Letrec-FSharp" "The INFDEV@HR Team" "Recursion and F\# translations" true false

  do batchProcess LatexDefinition.generatePresentation FunctionalWeek5.slides "INFSEN02-1-Lec5-Types" "The INFDEV@HR Team" "Types, inference, and F\# data types" true false
  // lambda calculus homework

  // Practicum 1
  // Lecture 7 Haskell translations and laziness
  // Practicum 2
  // Lecture 8 patterns and practices: auto-updateable entities, monads, coroutines, auto-updateable entities with coroutines
  0
