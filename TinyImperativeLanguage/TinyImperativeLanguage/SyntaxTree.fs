module SyntaxTree

type Literal =
| Integer of int
| Double of float
| Float of float32
| String of string
| Boolean of bool
| Char of char
with
  override this.ToString() =
    match this with
    | Integer x -> string x
    | Double x -> string x
    | Float x -> string x
    | String x -> x
    | Boolean x -> string x
    | Char x -> string x

type Expression =
| Value of Literal
| Var of string
| Add of Expression * Expression
| Sub of Expression * Expression
| Mul of Expression * Expression
| Div of Expression * Expression
| Mod of Expression * Expression
| Not of Expression
| And of Expression * Expression
| Or of Expression * Expression
| Gt of Expression * Expression
| Geq of Expression * Expression
| Lt of Expression * Expression
| Leq of Expression * Expression
| Eq of Expression * Expression
| StringConversion of Expression
with
  override this.ToString() =
    let combineExpr left right symbol =
      "(" + left.ToString() + symbol + right.ToString() + ")"
    match this with
    | Value v -> string v
    | Var id -> id
    | Add(left,right) -> combineExpr left right " + "
    | Sub(left,right) -> combineExpr left right " - "
    | Mul(left,right) -> combineExpr left right " * "
    | Div(left,right) -> combineExpr left right " / "
    | Mod(left,right) -> combineExpr left right " % "
    | Not expr -> "(" + "!" + (expr.ToString()) + ")"
    | And(left,right) -> combineExpr left right " && "
    | Or(left,right) -> combineExpr left right " || "
    | Gt(left,right) -> combineExpr left right " > "
    | Geq(left,right) -> combineExpr left right " >= "
    | Lt(left,right) -> combineExpr left right " < "
    | Leq(left,right) -> combineExpr left right " <= "
    | Eq(left,right) -> combineExpr left right " == "
    | StringConversion expr -> sprintf "%s.ToString();" (string expr)
   


type Statement =
| Declaration of string * Expression
| Printf of Expression
| IfElse of Expression * Statement * Statement
| If of Expression * Statement
| While of Expression * Statement
| Block of Statement list
with
  override this.ToString() = this.GenerateString 0
  member private this.GenerateString indent =      
    let rec indentString indent = 
      match indent with
      | 0 -> ""
      | n -> "  " + (indentString (n - 1))
    let whiteSpaces = indentString indent
    match this with
    | Declaration(var,expr) -> sprintf "%s%s = %s;" whiteSpaces var (string expr)
    | Printf expr -> sprintf "%sprintf %s;" whiteSpaces (string expr)
    | Block block ->
        block |>
        List.fold(fun s stmt ->
                  s + (stmt.GenerateString indent) + "\n") ""
    | IfElse(condition,_then,_else) ->
        let thenString = _then.GenerateString (indent + 1) 
        let elseString = _then.GenerateString (indent + 1)
        sprintf "%sif %s then\n%selse\n%s" whiteSpaces (string condition) thenString elseString
    | If(condition,_then) ->
        let thenString = _then.GenerateString (indent + 1)
        sprintf "%sif %s then\n%s" whiteSpaces (string condition) thenString
    | While(condition,block) ->
        let blockString = block.GenerateString (indent + 1)
        sprintf "%swhile %s do\n%s" whiteSpaces (string condition) blockString

type Program = 
  {
    Statements          : Statement list
    Variables           : Map<string,Literal>
  }
  with
    override this.ToString() =
      "===== PROGRAM =====\n\n" +
      (this.Statements |> 
       List.fold(fun s stmt -> s + (string stmt) + "\n") "") + "\n\n==== VARIABLES ====\n\n" + "{\n" +
      (this.Variables |>
       Map.fold(fun s id value -> s + (sprintf "  %s --> %s\n" id (string value))) "") + "}"
    static member Create(statements : Statement list) =
      {
        Statements = statements
        Variables = Map.empty
      }

let (!->) x = Value x
let (!!!) id = Var id
let (!+) x = Value(Integer x)
let (!-) x = Value(Double x)
let (!*) x = Value(Float x)
let (!/) x = Value(String x)
let (!&) x = Value(Boolean x)
let (!+!) x = Value(Char x)
