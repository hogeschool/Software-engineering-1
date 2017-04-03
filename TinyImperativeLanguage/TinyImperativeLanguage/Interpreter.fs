module Interpreter

exception RuntimeError of string

open SyntaxTree
  

let rec evalExpr (vars : Map<string,Literal>) (expr : Expression) =
  let runBinaryOp (left : Expression) (right : Expression) (behaviour : Literal -> Literal -> Literal) =
    let evalLeft = evalExpr vars left
    let evalRight = evalExpr vars right
    behaviour evalLeft evalRight

  let runLogicalOp (left : Expression) (right : Expression) (op : bool -> bool -> bool) (errorSymbol : string) =
    let evalLeft = evalExpr vars left
    let evalRight = evalExpr vars right
    match evalLeft,evalRight with
    | Boolean x,Boolean y -> Boolean (op x y)
    | _ -> raise(RuntimeError ("Incompatible types for " + errorSymbol + " operator"))

  match expr with
  | Value v -> v
  | Var id ->
      match vars |> Map.tryFind id with
      | Some value -> value
      | None -> raise(RuntimeError (sprintf "Undefined variable %s" id))
  | Add(left,right) ->
      runBinaryOp left right
        (fun left right ->
            match left,right with
            | Integer x,Integer y -> Integer (x + y)
            | Double x,Double y -> Double (x + y)
            | Float x,Float y -> Float(x + y)
            | String x,String y -> String(x + y)
            | Char x,Char y -> Char((byte x) + (byte y) |> char)
            | _ -> raise(RuntimeError "Incompatible types for + operator"))
  | Sub(left,right) -> 
      runBinaryOp left right
        (fun left right ->
            match left,right with
            | Integer x,Integer y -> Integer (x - y)
            | Double x,Double y -> Double (x - y)
            | Float x,Float y -> Float(x - y)
            | Char x,Char y -> Char((byte x) - (byte y) |> char)
            | _ -> raise(RuntimeError "Incompatible types for - operator"))
   | Mul(left,right) -> 
        runBinaryOp left right
          (fun left right ->
              match left,right with
              | Integer x,Integer y -> Integer (x * y)
              | Double x,Double y -> Double (x * y)
              | Float x,Float y -> Float(x * y)
              | _ -> raise(RuntimeError "Incompatible types for * operator"))
   
    | Div(left,right) ->
        runBinaryOp left right
          (fun left right ->
              match left,right with
              | Integer x,Integer y -> Integer (x / y)
              | Double x,Double y -> Double (x / y)
              | Float x,Float y -> Float(x / y)
              | _ -> raise(RuntimeError "Incompatible types for / operator"))
    | Mod(left,right) ->
        runBinaryOp left right
          (fun left right ->
              match left,right with
              | Integer x,Integer y -> Integer (x % y)
              | _ -> raise(RuntimeError "Incompatible types for / operator"))
    | Not expr ->
        let evalExpr = evalExpr vars expr
        match evalExpr with
        | Boolean b -> Boolean (not b)
        | _ -> raise(RuntimeError "Incompatible types for ! operator")
    | And(left,right) -> runLogicalOp left right (&&) "&&"
    | Or(left,right) -> runLogicalOp left right (||) "||"
    | Gt(left,right) ->
        runBinaryOp left right
          (fun left right ->
              match left,right with
              | Integer x,Integer y -> Boolean (x > y)
              | Double x,Double y -> Boolean (x > y)
              | Float x,Float y -> Boolean (x > y)
              | _ -> raise(RuntimeError "Incompatible types for > operator"))
    | Geq(left,right) ->
        runBinaryOp left right
          (fun left right ->
              match left,right with
              | Integer x,Integer y -> Boolean (x >= y)
              | Double x,Double y -> Boolean (x >= y)
              | Float x,Float y -> Boolean (x >= y)
              | _ -> raise(RuntimeError "Incompatible types for >= operator"))
    | Lt(left,right) ->
        runBinaryOp left right
          (fun left right ->
              match left,right with
              | Integer x,Integer y -> Boolean (x < y)
              | Double x,Double y -> Boolean (x < y)
              | Float x,Float y -> Boolean (x < y)
              | _ -> raise(RuntimeError "Incompatible types for < operator"))
    | Leq(left,right) ->
        runBinaryOp left right
          (fun left right ->
              match left,right with
              | Integer x,Integer y -> Boolean (x <= y)
              | Double x,Double y -> Boolean (x <= y)
              | Float x,Float y -> Boolean (x <= y)
              | _ -> raise(RuntimeError "Incompatible types for <= operator"))
    | Eq(left,right) ->
        runBinaryOp left right
          (fun left right ->
              match left,right with
              | Integer x,Integer y -> Boolean (x = y)
              | Double x,Double y -> Boolean (x = y)
              | Float x,Float y -> Boolean (x = y)
              | _ -> raise(RuntimeError "Incompatible types for = operator"))
    | StringConversion expr ->
        let e = evalExpr vars expr
        String (string e)

        


let rec evalStmt (vars: Map<string,Literal>) (stmt : Statement) =
  match stmt with
  | Declaration(var,expr) -> vars |> Map.add var (evalExpr vars expr)
  | Printf expr ->
      let evalExpr = evalExpr vars expr
      do printfn "%s" (string evalExpr)
      vars
  | Block block ->
      block |>
      List.fold (fun updatedVars stmt -> evalStmt updatedVars stmt) vars 
  | IfElse(condition,_then,_else) ->
      let evalCondition = evalExpr vars condition
      match evalCondition with
      | Boolean _ ->
        if (evalCondition = Boolean true) then
          evalStmt vars _then
        else
          evalStmt vars _else
      | _ -> raise(RuntimeError "The condition of an if-statement must be a boolean")
  | If(condition,_then) ->
      let evalCondition = evalExpr vars condition
      match evalCondition with
      | Boolean _ ->
        if (evalCondition = Boolean true) then
          evalStmt vars _then
        else
          vars
      | _ -> raise(RuntimeError "The condition of an if-statement must be a boolean")
  | While(condition,block) ->
      let evalCondition = evalExpr vars condition
      match evalCondition with
      | Boolean _ ->
        if (evalCondition = Boolean true) then
          let newVars = evalStmt vars block
          evalStmt newVars stmt
        else
          vars
      | _ -> raise(RuntimeError "The condition of an while-statement must be a boolean")

  
let eval (program : Program) =
  program.Statements |>
  List.fold(fun updatedProgram stmt -> 
//              do printfn "%s" (string updatedProgram)
              let res = { updatedProgram with Variables = evalStmt updatedProgram.Variables stmt } 
              res) program
  

