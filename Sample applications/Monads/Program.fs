// type Option<'a> = None | Some of 'a

(* 
Monad =
  type constructor M (M<'a>): think of List, Option, etc.
  bind operator       M<'a> -> ('a -> M<'b>) -> M<'b>
  return operator     'a -> M<'a>
*)

let fail() = None

let return_ x = Some x

let (>>=) (p:Option<'a>) (k:'a->Option<'b>) : Option<'b> =
  match p with
  | None -> None
  | Some(v_p:'a) ->
    k v_p

type OptionBuilder() =
  member this.ReturnFrom p = p
  member this.Return x = return_ x
  member this.Bind(p,k) = p >>= k

let opt = OptionBuilder()

let safeDivide (x:Option<int>) (y:Option<int>) : Option<int> =
  opt{
    let! v_x = x
    let! v_y = y
    if v_y = 0 then
      return! fail()
    else
      return (v_x / v_y)
  }

[<EntryPoint>]
let main argv = 
    printfn "%A" (safeDivide (Some 100) (Some 5))
    0 // return an integer exit code
