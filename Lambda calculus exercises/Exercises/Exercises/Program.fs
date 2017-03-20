open Compile


[<EntryPoint>]
let main argv = 
  do batchProcess LatexDefinition.generateDocument SolvedExercises.slides "BetaReductionExercises" "Lambda Calculus exercises \\\\ Beta reduction" "Software Engineering 1" true false 
  0
