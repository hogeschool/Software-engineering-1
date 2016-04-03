//A fraction is made of a pair of integer numbers called numerator and denominator.
//The sum of two fractions can be done only if the denominator is the same. The sum is a fraction having the sum of the numerators and the same denominator.
//The multiplication of two fractions is a fraction containing the result of the multiplication of numerators and denominators.
//The division is the multiplication of the first fraction with the inverse of the second.
//Write a function which implements the operations between two fractions. Use a record to model a fraction.

//===========================================================================================================

//In several situations having a language that simulates thread behaviours is required because of performance reasons.
//In this exercise we build our own scripting language having interruptible and synchronizable scripts.
//Each script statement is defined by a union where each choice is a statement. The union is parametric with respect to the input, because we want every script
//to be able to process any kind of input. The execution of each statement returns a pair made by the result of processing the input at the previous step
//and a new statement. The scripts are run by a scheduler that loops forever and makes them advance of one step. The script data structure should have
//a function or a member Update that takes as input the input of the script and dt, which is the time difference between the current update and the previous
//one.

//The following are the possible statements of a script:
//Done: Done is used to mark when a script terminated its execution.
//Wait: it takes as argument a float32 that is the amount of time to wait. The timer is decremented by dt if it is positive. If it is less or equal than 0
//then the script returns Done. The input is never modified by the Wait so we just return input.
//When: it takes as argument a function ('input -> bool). At each update it runs the function and if it is true, it returns Done, otherwise it returns the script
//itself because the same predicate must be checked again during the next update. The input is never modified by this script, so we just return it as it is.
//Call: it takes as input a function ('input -> 'input) that modifies the input of the script and returns a modified copy. The script runs the function and 
//returns Done. We return also the result of the function.
//Sequence: it takes as input two statements, called "current" and "next". We run "current" and read the result (res,script). If "script" is Wait or When we return
//the result and the new script and pause the execution because we need to pause the exectuion.
//If "res" is anything else we immediately run"next" by passing as input the result "res" of the previous execution.
//If: this is required to support pausable if-statements. If takes as input a function ('input -> bool) and two statements, one for "then", and one for "else". We run
//the condition function, and if it returns true we run "then", otherwise we run "else". "then" and "else" are usually Sequences.
//While: this is required to support pausable while-statements. It takes as input a function ('input -> bool) and a statement "body" that is usually a sequence. We run
//the condition function, and if it returns true we do the following: we add at the end of "body" the while-loop again making a new sequence of statements, because once
//the body has been executed we must re-evaluate the while condition. Then we run this new sequence. If the condition returns false, we just return Done and the input of
//this script, which is untouched.

//SCHEDULER:
//The scheduler runs a list of scripts. Every time we run a script we get as result (res,script) where "res" is the possibly changed input of the script, and "script" is
//the updated state of the script. "res" is passed as input to the next script to execute. If "script" is Done than the script has terminated its execution and will be removed
//from the scripts for the next update. Otherwise the scheduler puts the updated script into a new list of scripts that will be returned after running all the scripts.

//RUNNING THE SCHEDULER":
//Use the function schedulerLoop you find below. The function takes as input a state to modify, which can be any record data structure, a scriptManager which is a record containing
//the scripts and calling the scheduler to update them, a frameRate, which is the maximum frequency of update (60 updates per second is fine), a quitCondition, which is a function
//(State -> bool) that, when returns true, halts the execution of the function.


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



type State =
  {
    X     : int
    Y     : float32
    Quit  : bool
  }

type ScriptManager = 
  {
    StateScripts       : Script<State> list
  }
with
   member this.Update(dt,state) =
    let updatedScripts,result = scheduler this.StateScripts [] state dt
    result,{this with StateScripts = updatedScripts}

let script1 =
  Wait 0.5f >> (
    Call ((fun state -> {state with X = state.X + 1})) >> (
      Wait 2.0f >> (
        Call ((fun state -> {state with X = state.X + 3})))))

let script2 = 
  When (fun state -> state.X >= 4) >> (
    Wait 0.5f >> (
      Call (fun state -> {state with X = 0})))

let script3 =
  While((fun _ -> true),
                      (If ((fun state -> state.X < 4),Sequence(Call((fun state -> {state with Y = state.Y + 0.1f})),Wait 0.05f),
                          Sequence(Call(fun state -> {state with Y = state.Y - 0.1f}),Wait 0.05f))))

let script4 = 
  While ((fun _ -> true),When(fun state -> state.X >= 4) >> (Call((fun state -> {state with Y = state.Y - 0.1f})) >> (Wait 0.05f)))

let quit =
  When(fun state -> state.Y < 0.0f) >> (
    Call(fun state -> {state with Quit = true}))
    





let rec schedulerLoop (watch : System.Diagnostics.Stopwatch) (before : float32) (state : State) (manager : ScriptManager) (frameRate : float32) (quitCondition : State -> bool)=
  let now = (watch.ElapsedMilliseconds |> float32) / 1000.0f
  let dt = (now - before |> float32)
  if (dt >= 1.0f / frameRate && quitCondition state |> not) then
    let updatedState,updatedManager = manager.Update(dt,state)
    do System.Console.Clear()
    do printfn "%A" updatedState
    do schedulerLoop watch now updatedState updatedManager frameRate quitCondition
  elif (quitCondition state |> not) then
    do schedulerLoop watch before state manager frameRate quitCondition

[<EntryPoint>]
let main argv =
  let f1 = 5 .| 3
  let f2 = 2 .| 4
  let r = f1 * f2
  let state = { X = 0; Y = 4.0f; Quit = false }
  let scripts = { StateScripts = [script1; script4;quit] }
  let watch = System.Diagnostics.Stopwatch()
  do watch.Start()
  do schedulerLoop watch (watch.ElapsedMilliseconds |> float32) state scripts 60.0f (fun state -> state.Quit)
  0 // return an integer exit code
