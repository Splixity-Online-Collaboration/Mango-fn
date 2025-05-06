module AbSyn

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
type Position = int * int                                                       // row column

type Exp =
    Constant of int * Position                                                  // int_val
    | StringLit of string * Position                                            // string
    | Var   of string * Position                                                // variable_name
    
type ButtonProp =
    IsVisible of bool * Position                                                // is_visible?

type UIElement = 
    Button of string * ButtonProp list * Position                               // label properties
    | TextBlock of string * Position                                            // text
    | TextBox of string * Position                                              // text
    | CheckBox of string * Position                                             // label
    | RadioButton of string * Position                                          // label
    | Calendar of Position                                                      // 

type Window = Window of string * int option * int option * string option * UIElement list * Position  // name width height filepathToIcon

