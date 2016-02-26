module SampleExams.Exam5

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
        LambdaCodeBlock(TextSize.Small, Let(-"apply", -"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))), false)
      ]
    SubSection("Relevant delta rules")
    VerticalStack
      [
        TextBlock "Integer addition"
        LambdaCodeBlock(TextSize.Small, (deltaRules Plus).Value, false)

        TextBlock "Integer three (3)"
        LambdaCodeBlock(TextSize.Small, (deltaRules !!"3").Value, false)

        TextBlock "Integer size (6)"
        LambdaCodeBlock(TextSize.Small, (deltaRules !!"6").Value, false)
      ]

    SubSection("Answer 1 (note: you do not need to write all this detail yourself, it is only included for completeness)")
    LambdaStateTrace(TextSize.Small, Let(-"apply", -"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))), None, true, true, true, true, true, true)


    Section("Question 2")
    VerticalStack
      [
      TextBlock "Given the following lambda calculus program, and a series of relevant delta rules, give the full typing derivation for the program."
      LambdaCodeBlock(TextSize.Small, (deltaRules Plus).Value, true)
    ]
    SubSection("Relevant delta rules")
    VerticalStack
      [
        TextBlock "Boolean type:"
        LambdaCodeBlock(TextSize.Small, Type(defaultTypes.[Nat]), true)
      ]

    SubSection("Answer 2 (note: you do not need to write all this detail yourself, it is only included for completeness)")
    LambdaTypeTrace(TextSize.Small, (deltaRules Plus).Value)

  ]
