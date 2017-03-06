module SVMAST

// Discriminated union for the 4 registers of the SVM
type Register = Reg1 | Reg2 | Reg3 | Reg4


// Tuple used to store the line and column index of an instruction. It is used to write the position of a parse or runtime error
// when the relative exception is thrown
type Position = int * int

// Data structures representing the constant values of the language. Address may contain the Integer representing the memory address or
// the register from which the address is read.
type Literal =
| Integer of int * Position
| Float of float * Position
| String of string * Position
| Address of Literal
| Register of Register * Position

// Instructions supported by the SVM. See the documentation for further details.
type Instruction =
| Nop of Position
| Mov of Literal * Literal * Position
| And of Register * Literal * Position
| Or of Register * Literal * Position
| Not of Register * Position
| Mod of Register * Literal * Position
| Add of Register * Literal * Position
| Sub of Register * Literal * Position
| Mul of Register * Literal * Position
| Div of Register * Literal * Position
| Cmp of Register * Literal * Position
| Jmp of string * Position
| Jc of string * Register * Position
| Jeq of string * Register * Position
| Label of string * Position


// The Parser generates a Program data structure which is simply a list of instructions.
and Program = Instruction list
