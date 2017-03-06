module ParserUtils

open Microsoft.FSharp.Text.Parsing

exception ParseError of string * int * int

let rhs (parseState: IParseState) i =
  let pos = parseState.InputEndPosition i
  (pos.Line + 1,pos.Column)

let errAtPos (msg : string) (pos : int * int) =
  raise(ParseError(msg,fst pos,snd pos))

let err msg =
  errAtPos msg (0,0)


