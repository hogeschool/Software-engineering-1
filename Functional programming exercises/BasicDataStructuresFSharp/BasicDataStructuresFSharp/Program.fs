//A fraction is made of a pair of integer numbers called numerator and denominator.
//The sum of two fractions can be done only if the denominator is the same. The sum is a fraction having the sum of the numerators and the same denominator.
//The multiplication of two fractions is a fraction containing the result of the multiplication of numerators and denominators.
//The division is the multiplication of the first fraction with the inverse of the second.
//Write a function which implements the operations between two fractions. Use a record to model a fraction.

module Program

open ScriptLanguage

let curry f a b = f (a,b)
let uncurry f (a,b) =  f a b

type Fraction =
  {
    Numerator     : int
    Denominator   : int
  }
  with 
    static member Create(a,b) =
      {
        Numerator = a
        Denominator = b
      }
    static member (+) (f1 : Fraction,f2 : Fraction) =
      if f1.Denominator = f2.Denominator then
        Fraction.Create(f1.Numerator + f2.Numerator, f1.Denominator)
      else
        failwith "The two fractions must have the same denominator"

    static member (-) (f1,f2) = f1 + (Fraction.Create(-f2.Numerator,f2.Denominator))

    static member (*) (f1,f2) =
      Fraction.Create(f1.Numerator * f2.Numerator, f1.Denominator * f2.Denominator)

    static member (!) f =
      Fraction.Create(f.Denominator,f.Numerator)

    static member (/) (f1,f2) = f1 * (!f2)

    override this.ToString() = (this.Numerator |> string) + "\n-----------\n" + (this.Denominator |> string)

let (.|) a b = Fraction.Create(a,b)



type GameState =
  {
    X     : int
    Y     : float32
    Quit  : bool
  }

type ScriptManager = 
  {
    StateScripts       : Script<GameState> list
  }
with
   member this.Update(dt,state) =
    let updatedScripts,result = scheduler this.StateScripts [] state dt
    result,{this with StateScripts = updatedScripts}

let script1 =
  Wait 0.5f >> (
    Call ((fun state -> {state with X = state.X + 1})) >> (
      Wait 2.0f >>
        Call ((fun state -> {state with X = state.X + 3}))))

let script2 = 
  When (fun state -> state.X >= 4) >> (
    Wait 0.5f >> (
      Call (fun state -> {state with X = 0})))

let script3 =
  While((fun state -> state.Y >= 0.0f),
                      (If ((fun state -> state.X < 4),Sequence(Call((fun state -> {state with Y = state.Y + 0.1f})),Wait 0.05f),
                          Sequence(Call(fun state -> {state with Y = state.Y - 0.1f}),Wait 0.05f))))

let quit =
  When(fun state -> state.Y < 0.0f) >> (
    Call(fun state -> {state with Quit = true}))
    





let rec gameLoop (watch : System.Diagnostics.Stopwatch) (before : float32) (state : GameState) (manager : ScriptManager) (frameRate : float32) (quitCondition : GameState -> bool)=
  let now = (watch.ElapsedMilliseconds |> float32) / 1000.0f
  let dt = (now - before |> float32)
  if (dt >= 1.0f / frameRate && quitCondition state |> not) then
    let updatedState,updatedManager = manager.Update(dt,state)
    do System.Console.Clear()
    do printfn "%A" updatedState
    do gameLoop watch now updatedState updatedManager frameRate quitCondition
  elif (quitCondition state |> not) then
    do gameLoop watch before state manager frameRate quitCondition

[<EntryPoint>]
let main argv =
  let f1 = 5 .| 3
  let f2 = 2 .| 4
  let r = f1 * f2
  let state = { X = 0; Y = 0.0f; Quit = false }
  let scripts = { StateScripts = [script1; script3; quit] }
  let watch = System.Diagnostics.Stopwatch()
  do watch.Start()
  do gameLoop watch (watch.ElapsedMilliseconds |> float32) state scripts 60.0f (fun state -> state.Quit)
  0 // return an integer exit code
