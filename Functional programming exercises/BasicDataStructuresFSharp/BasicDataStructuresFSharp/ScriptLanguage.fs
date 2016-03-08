module ScriptLanguage

type Script<'input> =
| Sequence of Script<'input> * Script<'input>
| Result of 'input
| Execute of ('input -> unit) * 'input
| Call of ('input -> 'input)
| Wait of float32
| When of ('input -> bool)
| Done
| If of ('input -> bool) * Script<'input> * Script<'input>
| While of ('input -> bool) * Script<'input>
with
  static member joinScripts (s1 : Script<'input>) (s2: Script<'input>) : Script<'input> =
    let rec join s1 s2 =
      match s1 with
      | Sequence(c1,n1) -> Sequence(c1, join n1 s2)
      | _ -> Sequence(s1,s2)
    join s1 s2     
      
  member this.Run(input,dt) =
    match this with
    | Wait t ->
        if t <= 0.0f then
          input, Done
        else
          input, Wait(t - dt)
    | When p ->
        if p input then
          input, Done
        else
          input, When p
    | Execute (f,arg) ->
        do f arg
        input, Done
    | Call(f) ->
        let res = f input
        res, Done
    | Sequence(current,next) ->
        let res,script = current.Run(input,dt)
        match script with
        | Wait _
        | When _ ->
            res, Sequence(script,next)
        | Done ->
            next.Run(res,dt)
        | _ -> failwith "Invalid script result while processing sequence"
    | If(c,_then,_else) ->
        if c input then
          _then.Run(input,dt)
        else
          _else.Run(input,dt)
    | While(c,block) ->
        if c input then
          let newBlock = Script.joinScripts block (While(c,block))
          newBlock.Run(input,dt)
        else
          input, Done
    | Done -> input, Done
        

let (>>) current next = Sequence(current,next)


let rec scheduler (scripts : Script<'input> list) (updatedScripts : Script<'input> list) (input : 'input) (dt : float32) : (Script<'input> list) * 'input =
  match scripts with
  | [] -> updatedScripts |> List.rev,input
  | script :: scripts ->
      let result, script = script.Run(input,dt)
      match script with
      | Result x -> scheduler scripts updatedScripts x dt
      | Done -> scheduler scripts updatedScripts result dt
      | _ -> scheduler scripts (script :: updatedScripts) result dt

