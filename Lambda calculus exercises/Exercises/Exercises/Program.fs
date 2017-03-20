open Compile


[<EntryPoint>]
let main argv = 
//  do batchProcess LatexDefinition.generateDocument Reduction.slides "BetaReductionExercises" "Lambda Calculus exercises \\\\ Beta reduction" "Software Engineering 1" true false 
  do batchProcess LatexDefinition.generateDocument Typing.slides "TypingExercises" "Lambda Calculus exercises \\\\ Typing" "Software Engineering 1" true false 
  0
