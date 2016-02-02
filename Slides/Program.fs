open Compile

[<EntryPoint>]
let main argv = 
  do batchProcess FunctionalWeek1.slides "fp_week1" "The INFDEV team" "Test" true false
  0
