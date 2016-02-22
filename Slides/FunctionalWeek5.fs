module FunctionalWeek5

open CommonLatex
open LatexDefinition
open CodeDefinitionLambda
open Interpreter

let slides =
  [
    Section("Introduction")
    SubSection("Lecture topics")
    LambdaCodeBlock(TextSize.Small, (deltaRules Plus).Value, true)
    LambdaTypeBlock(TextSize.Small, defaultTypes.[Nat])
    LambdaTypeBlock(TextSize.Small, defaultTypes.[Boolean])
    LambdaTypeBlock(TextSize.Small, defaultTypes.[Product])
    LambdaTypeBlock(TextSize.Small, defaultTypes.[Sum])

//    ItemsBlock
//      [
//        ! @"The typed lambda calculus"
//        ! @"Type inference"
//        ! @"F\# basic types"
//        ! @"F\# custom advanced types"
//      ]
//
//    Section( @"The typed lambda calculus")
//    SubSection("Idea")
//    TextBlock @"We can add types to the lambda calculus!"
//
//    TextBlock @"This makes the lambda calculus safer to compose, as we run no more the risk of passing a parameter of the wrong shape (like an integer where a function is expected)"
//
//    SubSection("Shapes of types")
//    VerticalStack [
//        TextBlock @"Types in the lambda calculus follow the same basic syntactic shape of the lambda calculus itself."
//        
//        TextBlock @"Consider any type $\tau$; it will be made up, recursively, out of any of the following shapes:"
//
//        ItemsBlock
//          [
//            ! @"a type variable (like generic type parameters, such as \texttt{T}): $\alpha$"
//            ! @"a generic type definition (like the definition of \texttt{List<T>}: $\forall \alpha \Rightarrow \tau$"
//            ! @"a function type: $\tau \rightarrow \tau$"
//            ! @"a type application (like \texttt{List<Customer>}): $\tau \tau$"
//          ]
//      ]
//
//
//    VerticalStack
//      [
//        TextBlock "True now becomes:"
//
//        LambdaCodeBlock(TextSize.Small, (deltaRules True).Value)
//      ]
//    
//
//    SubSection("Shapes of programs")
//    VerticalStack [
//        TextBlock @"Lambda calculus programs are now extended to include type declarations:"
//        
//        TextBlock @"Consider any term \texttt{t}; it will be made up, recursively, out of any of the following shapes:"
//
//        ItemsBlock
//          [
//            ! @"a free variable: \texttt{x}"
//            ! @"a lambda definition: \texttt{$\lambda$ x $\rightarrow$ t}"
//            ! @"a generic parameter introduction: \texttt{$\Lambda \alpha \Rightarrow$ t}"
//            ! @"a value application: \texttt{t t}"
//            ! @"a type application: \texttt{t $\tau$}"
//          ]
//      ]
//
////    VerticalStack
////      [
////        TextBlock "We can now try our hand at a factorial computation"
////
////        LambdaCodeBlock(TextSize.Small, (Fix >>> ("f" ==> ("n" ==> ((If >>> (IsZero >>> !!"n") >>> !!"1") >>> ((Mult >>> (!!"f" >>> ((Minus >>> !!"n") >>> !!"1"))) >>> !!"n"))))) >>> !!"2")
////      ]
//
////  @"The (simply) typed lambda calculus"
////  @"Type inference"
////  @"F\# basic types"
////  @"F\# custom advanced types"
////  records, union types, lists, list higher order functions, list comprehensions in F\#
//
////    LambdaStateTrace(TextSize.Small, (Fix >>> ("f" ==> ("n" ==> ((If >>> (IsZero >>> !!"n") >>> !!"1") >>> ((Mult >>> (!!"f" >>> ((Minus >>> !!"n") >>> !!"1"))) >>> !!"n"))))) >>> !!"2", 
////                      Option.None, false, false, false, true, true)
//
////    Section "Conclusion"
////    SubSection(@"Recap")
////    ItemsBlock[
////        ! @"The lambda calculus can be translated, term to term, into F\#"
////        ! @"F\# is therefore just a practical lambda calculus with a series of handy extensions and slightly more readable"
////      ]
  ]
