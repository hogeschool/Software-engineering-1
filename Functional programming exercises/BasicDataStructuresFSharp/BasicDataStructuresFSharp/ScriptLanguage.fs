module ScriptLanguage

type Script<'input, 'a> =
| Sequence of Script<'input,'a> * Script<'input,'a>
| Result of 'input
| Execute of ('a -> unit) * 'a
| Call of ('input -> 'input)
| Wait of float32
| When of ('input -> bool)
| Return
| If of ('input -> bool) * Script<'input,'a> * Script<'input,'a>
| While of ('input -> bool) * Script<'input,'a>
with
  member this.Run(input,dt) =
    match this with
    | Wait t ->
        if t <= 0.0f then
          Result input
        else
          Wait(t - dt)
    | When p ->
        if p input then
          Result input
        else
          When p
    | Result x -> Result x
    | Return -> Result input
    | Execute (f,arg) ->
        do f arg
        Result input
    | Call(f) ->
        let res = f input
        Result res
    | Sequence(current,next) ->
        let res = current.Run(input,dt)
        Sequence(res,next)
    | If(c,_then,_else) ->
        if c input then
          _then
        else
          _else
    | While(c,block) ->
        if c input then
          Sequence(block,While(c,block))
        else
          Result input
        

let (>>) current next = Sequence(current,next)


let rec scheduler (scripts : Script<'input,'a> list) (updatedScripts : Script<'input,'a> list) (input : 'input) (dt : float32) : (Script<'input,'a> list) * 'input =
  match scripts with
  | [] -> updatedScripts |> List.rev,input
  | script :: scripts ->
      let executionResult = script.Run(input,dt)
      match executionResult with
      | Sequence(current,next) ->
          match current with
          | Result x ->
              scheduler (next :: scripts) updatedScripts x dt
          | If _
          | While _ ->
              scheduler (executionResult :: scripts) updatedScripts input dt
          | _ ->
              scheduler scripts (executionResult :: updatedScripts) input dt
      | Result x -> scheduler scripts updatedScripts x dt
      | _ -> failwith "Invalid return type in script"

