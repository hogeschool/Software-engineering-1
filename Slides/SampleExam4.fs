module SampleExams.Exam4

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
        LambdaCodeBlock(TextSize.Small, ((If >> False) >> !!"1")>> !!"0", false)
      ]
    SubSection("Relevant delta rules")
    VerticalStack
      [
        TextBlock "If-then-else"
        LambdaCodeBlock(TextSize.Small, (deltaRules If).Value, false)

        TextBlock "Boolean false"
        LambdaCodeBlock(TextSize.Small, (deltaRules False).Value, false)

        TextBlock "Integer one (0)"
        LambdaCodeBlock(TextSize.Small, (deltaRules !!"0").Value, false)

        TextBlock "Integer one (1)"
        LambdaCodeBlock(TextSize.Small, (deltaRules !!"1").Value, false)
      ]

    SubSection("Answer 1 (note: you do not need to write all this detail yourself, it is only included for completeness)")
    LambdaStateTrace(TextSize.Small, ((If >> False) >> !!"1")>> !!"0", None, true, true, true, true, true, true)


    Section("Question 2")
    VerticalStack
      [
      TextBlock "Given the following lambda calculus program, and a series of relevant delta rules, give the full typing derivation for the program."
      LambdaCodeBlock(TextSize.Small, (deltaRules If).Value, true)
    ]
    SubSection("Relevant delta rules")
    VerticalStack
      [
        TextBlock "Boolean type:"
        LambdaCodeBlock(TextSize.Small, Type(defaultTypes.[Nat]), true)
      ]

    SubSection("Answer 2 (note: you do not need to write all this detail yourself, it is only included for completeness)")
    LambdaTypeTrace(TextSize.Small, (deltaRules If).Value)

  ]
