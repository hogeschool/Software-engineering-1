module SolvedExercises

open CommonLatex
open LatexDefinition
open CodeDefinitionLambda
open Interpreter

let exercise1 =
  (-"x" ==> (!!"x" >> !!"y")) >> (-"z" ==> !!"z")

let exercise2 =
  (-"x" ==> ((-"y" ==> (!!"x" >> !!"y")) >> !!"x")) >> (-"z" ==> !!"w")

let exercise3 =
  ((((-"f" ==> (-"g" ==> (-"x" ==> ((!!"f" >> !!"x") >> (!!"g" >> !!"x"))))) >> (-"m" ==> (-"n" ==> (!!"n" >> !!"m")))) >> (-"n" ==> !!"z")) >> !!"p")

let exercise4 =
  ((-"f" ==> ((-"g" ==> ((!!"f" >> !!"f") >> !!"g")) >> (-"h" ==> (!!"k" >> !!"h")))) >> (-"x" ==> (-"y" ==> !!"y")))

let exercise5 =
  (-"z" ==> !!"z") >> (-"y" ==> (!!"y" >> !!"y")) >> (-"x" ==> !!"x" >> !!"a")

let exercise6 =
  (-"z" ==> !!"z") >> (-"y" ==> !!"y" >> !!"y") >> (-"z" ==> !!"z" >> !!"y")

let exercise7 =
  ((-"x" ==> (-"y" ==> (!!"x" >> (!!"y" >> !!"y")))) >> (-"a" ==> !!"a")) >> !!"b"

let exercise8 =
  (-"x" ==> (-"y" ==> (!!"x" >> (!!"y" >> !!"y")))) >> (-"y" ==> !!"y") >> !!"y"

let exercise9 =
  (-"x" ==> !!"x" >> !!"x") >> (-"y" ==> !!"y" >> !!"x") >> !!"z"

let exercise10 =
  (-"x" ==> (-"y" ==> (!!"x" >> !!"y")) >> !!"y") >> !!"z"

let exercise11 =
  ((-"x" ==> (!!"x" >> !!"x")) >> (-"y" ==> !!"y")) >> (-"y" ==> !!"y")

let exercise12 =
  (((-"x" ==> (-"y" ==> (!!"x" >> !!"y"))) >> (-"y" ==> !!"y")) >> !!"w")

let exercises =
  [
    exercise1
    exercise2
    exercise3
    exercise4
    exercise5
    exercise6
    exercise7
    exercise8
    exercise9
    exercise10
    exercise11
    exercise12
  ]

let slides =
  exercises |> 
  List.fold
    (fun (i,s) e ->
      i + 1,
      s @ (
        Section(sprintf "Exercise %d" i) ::
        VerticalStack
          [
            TextBlock "Given the following lambda program, complete the empty beta reduction steps for this program."
            LambdaCodeBlock(TextSize.Small, e, false)
          ] ::
        SubSection(sprintf "Answer %d" i) ::
        [LambdaStateTrace(TextSize.Small, e, None, true, true, true, true, true, true)]
      )) (1,[]) |> snd
  

//let slides =
//  [
//    Section("Exercise 1")
//    VerticalStack
//      [
//        TextBlock "Given the following lambda program, complete the empty beta reduction steps for this program."
//        LambdaCodeBlock(TextSize.Small, exercise1, false)
//      ]
//    SubSection("Answer 1")
//    LambdaStateTrace(TextSize.Small, exercise1, None, true, true, true, true, true, true)
//
//    Section("Exercise 2")
//    VerticalStack
//      [
//        TextBlock "Given the following lambda program, complete the empty beta reduction steps for this program."
//        LambdaCodeBlock(TextSize.Small, exercise2, false)
//      ]
//    SubSection("Answer 2")
//    LambdaStateTrace(TextSize.Small, exercise2, None, true, true, true, true, true, true)
//
//    Section("Exercise 3")
//    VerticalStack
//      [
//        TextBlock "Given the following lambda program, complete the empty beta reduction steps for this program."
//        LambdaCodeBlock(TextSize.Small, exercise3, false)
//      ]
//    SubSection("Answer 3")
//    LambdaStateTrace(TextSize.Small, exercise3, None, true, true, true, true, true, true)
//
//    Section("Exercise 4")
//    VerticalStack
//      [
//        TextBlock "Given the following lambda program, complete the empty beta reduction steps for this program."
//        LambdaCodeBlock(TextSize.Small, exercise4, false)
//      ]
//    SubSection("Answer 4")
//    LambdaStateTrace(TextSize.Small, exercise4, None, true, true, true, true, true, true)
//
//    Section("Exercise 5")
//    VerticalStack
//      [
//        TextBlock "Given the following lambda program, complete the empty beta reduction steps for this program."
//        LambdaCodeBlock(TextSize.Small, exercise5, false)
//      ]
//    SubSection("Answer 5")
//    LambdaStateTrace(TextSize.Small, exercise5, None, true, true, true, true, true, true)
//
//    Section("Exercise 6")
//    VerticalStack
//      [
//        TextBlock "Given the following lambda program, complete the empty beta reduction steps for this program."
//        LambdaCodeBlock(TextSize.Small, exercise6, false)
//      ]
//    SubSection("Answer 6")
//    LambdaStateTrace(TextSize.Small, exercise6, None, true, true, true, true, true, true)
//
//  ]




