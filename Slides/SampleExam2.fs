module SampleExams.Exam2

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
        LambdaCodeBlock(TextSize.Small, ((Or >> True) >> False), false)
      ]
    SubSection("Relevant delta rules")
    VerticalStack
      [
        TextBlock "Boolean or:"
        LambdaCodeBlock(TextSize.Small, (deltaRules Or).Value, false)

        TextBlock "True"
        LambdaCodeBlock(TextSize.Small, (deltaRules True).Value, false)

        TextBlock "False"
        LambdaCodeBlock(TextSize.Small, (deltaRules False).Value, false)
      ]

    SubSection("Answer 1 (note: you do not need to write all this detail yourself, it is only included for completeness)")
    LambdaStateTrace(TextSize.Small, ((Or >> True) >> False), None, true, true, true, true, true, true)


    Section("Question 2")
    VerticalStack
      [
      TextBlock "Given the following lambda calculus program, and a series of relevant delta rules, give the full typing derivation for the program."
      LambdaCodeBlock(TextSize.Small, (deltaRules Or).Value, true)
    ]
    SubSection("Relevant delta rules")
    VerticalStack
      [
        TextBlock "Boolean type:"
        LambdaCodeBlock(TextSize.Small, Type(defaultTypes.[Boolean]), true)
      ]

    SubSection("Answer 2 (note: you do not need to write all this detail yourself, it is only included for completeness)")
    LambdaTypeTrace(TextSize.Small, (deltaRules Or).Value)

  ]
