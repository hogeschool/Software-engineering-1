module ScriptLanguage where

data Script input = Sequence ((Script input),(Script input)) 
  | Result input
  | Call (input -> input)
  | Wait Double
  | When (input -> Bool)
  | Done
  | If ((input -> Bool),(Script input),(Script input))
  | While ((input -> Bool),(Script input))
  
joinScripts :: (Script input) -> (Script input) -> (Script input)
joinScripts s1 s2 =
  case s1 of
  Sequence(c1,n1) -> Sequence(c1, joinScripts n1 s2)
  _ -> Sequence(s1,s2)

run :: (Script input) -> input -> Double -> (input,(Script input))
run script input dt =
  case script of
  Wait t ->
    if t <= 0.0 then
      (input,Done)
    else
      (input,Wait(t - dt))
  When p ->
    if p input then
      (input,Done)
    else
      (input,Done)
  Call f ->
    let
      res = f input
    in
      (res,Done)
  Sequence(current,next) ->
    let 
      (res,script) = run current input dt
    in
      case script of
      Wait _ -> (res,Sequence(script,next))
      When _ -> (res,Sequence(script,next))
      Done ->
        run next res dt 
  If(c,_then,_else) ->
    if c input then
      run _then input dt
    else
      run _else input dt
  While(c,block) ->
    if c input then
      let 
        newBlock = joinScripts block (While(c,block))
      in
        run newBlock input dt
    else
      (input,Done)
  Done -> (input,Done)
  
(>>>) current next = Sequence(current,next)

scheduler :: [Script input] -> [Script input] -> input -> Double -> ([Script input],input)
scheduler [] updatedScripts input _ = ((reverse updatedScripts),input)
scheduler (script:scripts) updatedScripts input dt =
  let
    (result,script) = run script input dt
  in
    case script of
    Result x -> scheduler scripts updatedScripts x dt
    Done -> scheduler scripts updatedScripts result dt
    _ -> scheduler scripts (script:updatedScripts) result dt