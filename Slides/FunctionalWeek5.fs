module FunctionalWeek5

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
        ! @"Let"
        ! @"Let-rec"
        ! @"F\# translations of tuples, union types, lists, list comprehensions, records"
      ]

  // Lecture 5 Typing and inference, type definitions of records, union types, lists, list higher order functions, list comprehensions

//    Section( @"\texttt{Let-in}")
//    SubSection("Idea")
//    VerticalStack
//      [
//        TextBlock "We can now try our hand at a factorial computation"
//
//        LambdaCodeBlock(TextSize.Small, (Fix >>> ("f" ==> ("n" ==> ((If >>> (IsZero >>> !!"n") >>> !!"1") >>> ((Mult >>> (!!"f" >>> ((Minus >>> !!"n") >>> !!"1"))) >>> !!"n"))))) >>> !!"2")
//      ]
//    LambdaStateTrace(TextSize.Small, (Fix >>> ("f" ==> ("n" ==> ((If >>> (IsZero >>> !!"n") >>> !!"1") >>> ((Mult >>> (!!"f" >>> ((Minus >>> !!"n") >>> !!"1"))) >>> !!"n"))))) >>> !!"2", 
//                      Option.None, false, false, false, true, true)

    Section "Conclusion"
    SubSection(@"Recap")
    ItemsBlock[
        ! @"The lambda calculus can be translated, term to term, into F\#"
        ! @"F\# is therefore just a practical lambda calculus with a series of handy extensions and slightly more readable"
      ]
  ]

