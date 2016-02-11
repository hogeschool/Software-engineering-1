module FunctionalWeek2

open CommonLatex
open SlideDefinition
open CodeDefinitionLambda
open Interpreter

let slides =
  [
    Section("Introduction")
    SubSection("Lecture topics")
    ItemsBlock
      [
        !"Make it pretty: delta rules"
        !"Booleans, boolean logic operators, if-then-else"
        !"Naturals, arithmetic operators, comparison operators"
        !"Let-binding and let rec"
        !"Tuples"
        !"Discriminated unions"
        !"Lists"
      ]

    LambdaStateTrace(TextSize.Small, ((Plus >>> !!"2") >>> !!"1"), None)

    LambdaStateTrace(TextSize.Small, ((And >>> True) >>> True), None)

//    VerticalStack
//      [
//        TextBlock "Addition:"
//        LambdaCodeBlock(TextSize.Tiny, ChurchNumerals.plus)
//      ]
//
//
//    VerticalStack
//      [
//        TextBlock "Multiplication:"
//        LambdaCodeBlock(TextSize.Tiny, (deltaRules Mult).Value)
//      ]
  ]

