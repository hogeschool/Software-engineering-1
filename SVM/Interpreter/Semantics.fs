module SVM

open SVMAST

exception RuntimeError of string * Position

type Value =
| Float of float
| Int of int
| String of string
with
  override this.ToString() =
    match this with
    | Float f -> string f
    | Int i -> string i
    | String s -> s
  static member (===) (v1,v2) = 
    match v1,v2 with
    | Float _,Float _
    | Int _,Int _
    | String _,String _ -> true
    | _ -> false

type SVM =
  {
    Memory              : List<Value>
    Reg1                : Value
    Reg2                : Value
    Reg3                : Value
    Reg4                : Value
    ProgramCounter      : int
    Labels              : Map<string,int>
    Stack               : Map<string,List<Value>>
  }
  with
    static member Create(memorySize : int) =    
      {
        Memory = [for x = 1 to memorySize do yield Int(0)]
        Reg1 = Int(0)
        Reg2 = Int(0)
        Reg3 = Int(0)
        Reg4 = Int(0)
        ProgramCounter = 0
        Labels = Map.empty
        Stack = Map.empty
      }
    member private this.PrintMemory =
        this.Memory |> 
        List.mapi (fun i x -> (i,string x)) |>
        List.fold (fun s (i,x) -> if i = 0 then x elif i % 10 = 0 then s + "\n" + x else s + "\t" + x) ""
    member this.Dump =
      let separator = "============"
      sprintf "MEMORY:\n%s\n\n%s\nREGISTERS:\n%s\t%s\t%s\t%s\n\n%s\n\nPROGRAM COUNTER:\n%d" 
        this.PrintMemory
        separator 
        (string this.Reg1) 
        (string this.Reg2) 
        (string this.Reg3)
        (string this.Reg4)
        separator
        this.ProgramCounter
    member this.SetMemory (address : int) (v : Value) (position : Position) =
      if address < this.Memory.Length then
        { this with Memory = this.Memory |> List.mapi (fun i x -> if i = address then v else x) }
      else
        raise(RuntimeError("The address is outside the memory address range",position))
    member this.SetRegister (register : Register) (v : Value) =
      match register with
      | Reg1 -> { this with Reg1 = v }
      | Reg2 -> { this with Reg2 = v }
      | Reg3 -> { this with Reg3 = v }
      | Reg4 -> { this with Reg4 = v }
    member this.GetRegister (register : Register) =
      match register with
      | Reg1 -> this.Reg1
      | Reg2 -> this.Reg2
      | Reg3 -> this.Reg3
      | Reg4 -> this.Reg4
    member this.GetAddressFromRegister (register : Register) (position : Position) =
      match this.GetRegister register with
      | Int i -> i
      | _ -> raise(RuntimeError("The content of the register is not an integer",position))

let setMemWithAnyArg (i : int) (arg : Literal) (position : Position) (svm : SVM) =
  match arg with
  | Literal.Float(x,_) -> svm.SetMemory i (Float x) position
  | Literal.Integer(x,_) -> svm.SetMemory i (Int x) position
  | Literal.String(x,_) -> svm.SetMemory i (String x) position
  | Literal.Address(Literal.Integer(x,_)) -> svm.SetMemory i (svm.Memory.[x]) position
  | Literal.Address(Register(x,p)) -> svm.SetMemory i (svm.Memory.[svm.GetAddressFromRegister x p]) position
  | Literal.Register(x,_) -> svm.SetMemory i (svm.GetRegister x) position
  | _ ->  failwith "Invalid right argument syntactical structure"

let setRegWithAnyArg (register : Register) (arg : Literal) (svm : SVM) =
  match arg with
  | Literal.Float(x,_) -> svm.SetRegister register (Float x)
  | Literal.Integer(x,_) -> svm.SetRegister register (Int x)
  | Literal.String(x,_) -> svm.SetRegister register (String x)
  | Literal.Address(Literal.Integer(x,_)) -> svm.SetRegister register (svm.Memory.[x])
  | Literal.Address(Register(x,p)) -> svm.SetRegister register (svm.Memory.[svm.GetAddressFromRegister x p])
  | Literal.Register(x,_) -> svm.SetRegister register (svm.GetRegister x)
  | _ -> failwith "Invalid right argument syntactical structure"

let trySetMemFromReg (register : Register) (position : Position) (l : Literal) (svm : SVM) =
  match svm.GetRegister register with
  | Int i -> setMemWithAnyArg i l position svm 
  | _ -> raise(RuntimeError("The register content should be an integer",position))
 

let conditionalUpdateOnValues (v1 : Value) (v2 : Value) (svm : SVM) (predicate : Value -> Value -> bool) (_then : SVM -> SVM) (_else : SVM -> SVM) =
  if predicate v1 v2 then
    _then svm
  else
    _else svm

let executeJmpOp
  (id : string)
  (register : Register)
  (position : Position)
  (predicate : int -> bool)
  (svm : SVM) =
  let registerValue = svm.GetRegister register
  match registerValue with
  | Int x ->
      if predicate x then 
        { svm with ProgramCounter = svm.Labels.[id] }
      else
        { svm with ProgramCounter = svm.ProgramCounter + 1 }
  | _ -> raise(RuntimeError("A jump instruction is trying to use a non-integer values",position))

let executeBinaryOp 
  (left : Register) 
  (right : Literal)
  (position : Position)
  (svm : SVM) 
  (valueFunction : Value -> Value -> Value) =
    let registerValue = svm.GetRegister left
    match right with
    | Literal.Integer(x,_) ->
        svm.SetRegister left (valueFunction registerValue (Int x))
    | Literal.Float(x,_) ->
        svm.SetRegister left (valueFunction registerValue (Float x))
    | Literal.String(s,_) ->
        svm.SetRegister left (valueFunction registerValue (String s))
    | Address(Integer(x,_)) ->
        svm.SetRegister left (valueFunction registerValue svm.Memory.[x])
    | Address(Register(r,p)) ->
        svm.SetRegister left (valueFunction registerValue svm.Memory.[svm.GetAddressFromRegister r p])
    | Register(r,_) ->
        let rightValue = svm.GetRegister r
        svm.SetRegister left (valueFunction registerValue rightValue)
    | _ -> raise(RuntimeError("Invalid binary operator argument format",position))
let execute (instruction : Instruction) (program : Program) (svm : SVM) =
  match instruction with
  | Nop _ -> { svm with ProgramCounter = svm.ProgramCounter + 1 }
  | Mov (left,right,p) ->
      match left with
      | Address(Integer(i,_)) -> setMemWithAnyArg i right p svm
      | Address(Register(r,_)) -> trySetMemFromReg r p right svm
      | Register(r,_) -> setRegWithAnyArg r right svm
      | _ -> failwith "Invalid MOV syntactical structure"
  | Not(register,p) ->
      let registerValue = svm.GetRegister register
      match registerValue with
      | Int x ->
          let notValue = if x >= 0 then Int -1 else Int 0
          svm.SetRegister register notValue
      | _ -> raise(RuntimeError("NOT can be applied only to integer values",p))
  | And (left,right,p) ->
      executeBinaryOp left right p svm
        (fun v1 v2 ->
            match v1,v2 with
            | Int x,Int y -> if x > 0 && y > 0 then Int 1 else Int 0
            | _ -> raise(RuntimeError("AND can be applied only to integer values",p)))
  | Or (left,right,p) ->
      executeBinaryOp left right p svm
        (fun v1 v2->
            match v1,v2 with
            | Int x,Int y -> if x <= 0 && y <= 0 then Int 0 else Int 1
            | _ -> raise(RuntimeError("OR can be applied only to integer values",p)))
  | Add (left,right,p) ->
      executeBinaryOp left right p svm
        (fun v1 v2 ->
            match v1,v2 with
            | Int x,Int y -> Int(x + y)
            | Float x,Float y -> Float(x + y)
            | String x,String y -> String(x + y)
            | _ -> raise(RuntimeError("ADD can be applied only to integer,float, or string arguments",p)))
  | Sub (left,right,p) ->
      executeBinaryOp left right p svm
        (fun v1 v2 ->
            match v1,v2 with
            | Int x,Int y -> Int(x - y)
            | Float x,Float y -> Float(x - y)
            | _ -> raise(RuntimeError("SUB can be applied only to integer or float arguments",p)))
  | Mul (left,right,p) ->
      executeBinaryOp left right p svm
        (fun v1 v2 ->
            match v1,v2 with
            | Int x,Int y -> Int(x * y)
            | Float x,Float y -> Float(x * y)
            | _ -> raise(RuntimeError("MUL can be applied only to integer or float arguments",p)))
  | Div (left,right,p) ->
      executeBinaryOp left right p svm
        (fun v1 v2 ->
            match v1,v2 with
            | Int x,Int y -> Int(x / y)
            | Float x,Float y -> Float(x / y)
            | _ -> raise(RuntimeError("DIV can be applied only to integer or float arguments",p)))
  | Mod (left,right,p) ->
      executeBinaryOp left right p svm
        (fun v1 v2 ->
            match v1,v2 with
            | Int x,Int y -> Int(x % y)
            |_ -> raise(RuntimeError("MOD can be applied only to integer arguments",p)))
  | Cmp (left,right,p) ->
      executeBinaryOp left right p svm
        (fun v1 v2 ->
            let compareFunc x y = if x < y then Int -1 elif x = y then Int 0 else Int 1
            match v1,v2 with
            | Int x,Int y -> compareFunc x y
            | Float x,Float y -> compareFunc x y
            | _ -> raise(RuntimeError("CMP can be applied only to integer or float arguments",p)))
  | Label(id,p) -> svm
  | Jmp(id,_) -> { svm with ProgramCounter = svm.Labels.[id] }
  | Jc(id,register,p) -> executeJmpOp id register p (fun x -> x > 0) svm
  | Jeq(id,register,p) -> executeJmpOp id register p (fun x -> x = 0) svm


let run (program : Program) (svm : SVM) =
  let svmWithLabels =
    program |> List.fold(fun (svm,i) currentStmt ->
                            match currentStmt with
                            | Label(id,p1) ->
                                if svm.Labels.ContainsKey id then
                                  raise(RuntimeError(sprintf "Duplicate label definition %s" id,p1))
                                else
                                  { svm with Labels = svm.Labels |> Map.add id i },i + 1 
                            | _ -> svm,i + 1) (svm,0) |> fst
  let rec execution (program : Program) (svm : SVM) =
    if svm.ProgramCounter < program.Length then
      let svmAfterExec = execute program.[svm.ProgramCounter] program svm
      match program.[svm.ProgramCounter] with
      | Jmp _
      | Jc _
      | Jeq _ -> execution program svmAfterExec
      | _ -> execution program { svmAfterExec with ProgramCounter = svmAfterExec.ProgramCounter + 1 }
    else
      svm
  execution program svmWithLabels
 

