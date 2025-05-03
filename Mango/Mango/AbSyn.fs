module AbSyn

(*** Helper Functions ***)
let toCString (s : string) : string =
    let escape c =
        match c with
            | '\\' -> "\\\\"
            | '"'  -> "\\\""
            | '\n' -> "\\n"
            | '\t' -> "\\t"
            | _    -> System.String.Concat [c]
    String.collect escape s

// Doesn't actually support all escapes.  Too badefacilliteter.
let fromCString (s : string) : string =
    let rec unescape l: char list =
        match l with
            | []                -> []
            | '\\' :: 'n' :: l' -> '\n' :: unescape l'
            | '\\' :: 't' :: l' -> '\t' :: unescape l'
            | '\\' :: c   :: l' -> c    :: unescape l'
            | c           :: l' -> c    :: unescape l'
    Seq.toList s |> unescape |> System.String.Concat

(* position: (line, column) *)
type Position = int * int

let rec ppType = function
  | Int      -> "int"
  | Char     -> "char"
  | Bool     -> "bool"


type Exp =
    Constant of int * Position                                  // int_val
    | StringLit of string * Position                            // string
    | Var   of string * Position                                // variable_name
    
type UIElement = 
    Button of string * Position

type Window = 
    Window of string * UIElement list * Position                                 // name
    | WindowWithSize of string * int * int * UIElement list * Position           // name width height
    | WindowWithIcon of string * int * int * string * UIElement list * Position  // name width height filepathToIcon
    | Invalid

