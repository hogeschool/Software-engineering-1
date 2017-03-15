module Program

open SyntaxTree
open Interpreter

let factorial =
   Program.Create(
        [
          Declaration("n",!+ 10)
          Declaration("i",!+ 1)
          Declaration("fact",!+ 1)
          While(Leq(!!!"i",!!!"n"),
            Block(
              [
                Printf(Add(!/ "INDEX: ",StringConversion !!!"i"))
                Declaration("fact",Mul(!!!"fact",!!!"i"))
                Declaration("i",Add(!!!"i",!+ 1))
                Printf(Add(!/ "FACTORIAL: ",StringConversion !!!"fact"))
              ]))
        ])

let fizzbuzz =
  Program.Create(
    [
      Declaration("d",!+ 3)
      Declaration("max",!+ 100)
      Declaration("i",!+ 0)
      While(Lt(!!!"i",!!!"max"),
        Block(
          [
            IfElse(Eq(Mod(!!!"i",!!!"d"),!+ 0),
              Block(
                [
                  Printf(Add(StringConversion !!!"i", !/ "  ---> Fizz"))
                ]),
              Block(
                [
                  Printf(Add(StringConversion !!!"i", !/ "  ---> Buzz"))
                ]))
            Declaration("i",Add(!!!"i",!+ 1))
          ]))
    ]
  )

[<EntryPoint>]
let main argv =
  try
    do printfn "%s" (string (eval factorial))
    0
  with
  | RuntimeError msg -> 
      do printfn "%s" msg
      1
