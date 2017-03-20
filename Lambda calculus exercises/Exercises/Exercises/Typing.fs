module Typing

open CommonLatex
open LatexDefinition
open CodeDefinitionLambda
open Interpreter

let _string = Id("string")
let _int = Id("int")
let _tuple = Id("Tuple")
let _intlist = Id("List<int>")


let exercise1 =
  let f = ("f",Arrow(Arrow(_int,_string),Arrow(_int,_string)))
  let g = ("g",Arrow(_int,_string))
  let x = ("x",_int)
  f ==> (g ==> (x ==> ((!!"f" >> !!"g") >> !!"x")))

let exercise2 =
  let g = ("g",Arrow(_int,_int))
  let l = ("l",_intlist)
  let f = ("f",Arrow(Arrow(_int,_int),Arrow(_intlist,_intlist)))
  f ==> (g ==> (l ==> ((!!"f" >> !!"g") >> !!"l")))

let exercise3 =
  let g = ("g",Arrow(_int,Arrow(_int,_int)))
  let l = ("l",_intlist)
  let s = ("s",_int)
  let f = ("f",Arrow(Arrow(_int,Arrow(_int,_int)),Arrow(_int,Arrow(_intlist,_intlist))))
  f ==> (g ==> (s ==> (l ==> (((!!"f" >> !!"g") >> !!"s") >> !!"l"))))

let exercise4 =
  let f = ("f",Arrow(_int,Arrow(_int,_string)))
  let x = ("x",_int)
  let g = ("g",Arrow(_int,_int))
  let y = ("y",_int)
  f ==> (g ==> (x ==> (y ==> ((!!"f" >> !!"x") >> (!!"g" >> !!"y")))))

let exercise5 =
  let f = ("f",Arrow(Arrow(_string,Arrow(_int,_string)),Arrow(_int,_string)))
  let x = ("x",Arrow(_int,_string))
  let a = ("a",_int)
  let y = ("y",Arrow(_string,Arrow(_int,_string)))
  f ==> (x ==> (a ==> (y ==> (!!"y" >> (!!"x" >> !!"a")))))


let exercises =
  [
    exercise1
    exercise2
    exercise3
    exercise4
    exercise5
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
            TextBlock "Given the following lambda program, complete the typing derivation for this program."
            LambdaCodeBlock(TextSize.Small, e, true)
          ] ::
        SubSection(sprintf "Answer %d" i) ::
        [LambdaTypeTrace(TextSize.Small, e)]
      )) (1,[]) |> snd

