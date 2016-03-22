type Then = Then
type Else = Else

type Expr = 
  | Int of int
  | Bool of bool
  | Var of string

let rec eval (m:Map<string,Expr>) (e:Expr) : Expr =
  match e with
  | Int i -> Int i
  | Bool b -> Bool b
  | Var x -> m.[x]

type Statement =
  | Assign of string * Expr
  | If of Expr * Statement * Statement
  | Print of Expr
  | Sequence of Statement * Statement
  | EOB

let rec run (s:Statement) (m:Map<string,Expr>) =
  match s with
  | EOB -> m
  | Assign(x,v) ->
    m |> Map.add x (eval m v)
  | Print(v) ->
    printfn "%A" (eval m v)
    m
  | Sequence(s1,s2) ->
    let m1 = run s1 m
    run s2 m1
  | If(c,t,e) ->
    match eval m c with
    | Bool true ->
      run t m
    | _ ->
      run e m

let (:=) x e = Assign(x,e)
let (!) v = Var v
let i x = Int x
let (>>) s1 s2 = Sequence(s1,s2)
let _if c (_:Then) t (_:Else) e = If(c,t,e)
let _then = Then
let _else = Else

[<EntryPoint>]
let main argv = 
  let p = 
      ("x" := (i 10)) >>
      Print(!"x") >>
      (_if (Bool true) _then
             (Print(i 20)) 
           _else
             (Print(i -5)))
      
  let _ = run p Map.empty
  0 // return an integer exit code
