module SampleExams.Exam3

open CommonLatex
open LatexDefinition
open CodeDefinitionLambda
open Interpreter

let slides =
  [
    Section("Question 1")
    VerticalStack
      [
        TextBlock "Given the following lambda program, and a series of relevant delta rules, show the beta reductions for this program."
        LambdaCodeBlock(TextSize.Small, ((Plus >> !!"1") >> !!"2"), false)
      ]
    SubSection("Relevant delta rules")
    VerticalStack
      [
        TextBlock "Integer addition:"
        LambdaCodeBlock(TextSize.Small, (deltaRules Plus).Value, false)

        TextBlock "Integer one (1)"
        LambdaCodeBlock(TextSize.Small, (deltaRules !!"1").Value, false)

        TextBlock "Integer two (2)"
        LambdaCodeBlock(TextSize.Small, (deltaRules !!"1").Value, false)
      ]

    SubSection("Answer 1 (note: you do not need to write all this detail yourself, it is only included for completeness)")
    LambdaStateTrace(TextSize.Small, ((Plus >> !!"1") >> !!"2"), None, true, true, true, true, true, true)


    Section("Question 2")
    VerticalStack
      [
      TextBlock "Given the following lambda calculus program, and a series of relevant delta rules, give the full typing derivation for the program."
      LambdaCodeBlock(TextSize.Small, (deltaRules Plus).Value, true)
    ]
    SubSection("Relevant delta rules")
    VerticalStack
      [
        TextBlock "Integer type:"
        LambdaCodeBlock(TextSize.Small, Type(defaultTypes.[Nat]), true)
      ]

    SubSection("Answer 2 (note: you do not need to write all this detail yourself, it is only included for completeness)")
    LambdaTypeTrace(TextSize.Small, (deltaRules Plus).Value)

  ]
