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
type Position = int * int                                                  // row column

type Value =
    | Int of int
    | Real of float
    | String of string
    | Bool of bool

type Exp =
    | Constant of Value * Position                                                  // int_val
    | Var of string * Position                                                // variable_name
    
type Stmt = 
    Let of string * Exp * Position

type FunctionT = Function of string * Stmt list * Position

type ButtonProp =
    | IsVisible of bool * Position                                                // is_visible?
    | Width of int * Position
    | Height of int * Position

type FontStyleT = 
    | Italic
    | StrikeThrough
    | Underline

type PredefinedColor = 
    | Blue 
    | Red
    | Yellow
    | Pink
    | Green

type HexCode = byte * byte * byte * byte

type ColorT =
    | ColorName of PredefinedColor * Position
    | Hex of HexCode * Position

type TextWrapT =
    | Overflow
    | Wrap
    | ForceWrap

type TextAlignT =
    | Center
    | Left 
    | Right

type TextTrimT =
    | Word
    | Character
    | NoTrim

type PaddingT =
  | Uniform of int * int * int * int
  | Symmetric of int * int

type TextBlockProp =
    | ForeGround of ColorT * Position
    | BackGround of ColorT * Position
    // Font Settings
    | FontFamily of string * Position
    | FontSize of  int * Position
    | FontWeight of int * Position
    | FontStyle of FontStyleT list * Position
    // Padding
    | Padding of PaddingT * Position
    // Formatting
    | LineHeight of int * Position
    | TextWrap of TextWrapT * Position
    | TextAlign of TextAlignT * Position
    | TextTrim of TextTrimT * Position

type UIElement = 
    | Button of string * ButtonProp list * Position
    | TextBlock of string * TextBlockProp list * Position
    | TextBox of string * Position 
    | CheckBox of string * Position
    | RadioButton of string * Position
    | ToggleSwitch of string * Position
    | Calendar of Position
    | ToggleButton of Position

type Window = Window of string * int option * int option * string option * UIElement list * Position  // name width height filepathToIcon