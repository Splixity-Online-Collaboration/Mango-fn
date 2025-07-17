module AbSyn

// Doesn't actually support all escapes. Too bad.
let fromCString (s : string) : string =
    let rec unescape l: char list =
        match l with
        | []                -> []
        | '\\' :: 'n' :: l' -> '\n' :: unescape l'
        | '\\' :: 't' :: l' -> '\t' :: unescape l'
        | '\\' :: c   :: l' -> c    :: unescape l'
        | c           :: l' -> c    :: unescape l'
    Seq.toList s |> unescape |> System.String.Concat

type Position = int * int // (line, column)

type Thickness =
    | Uniform of int
    | Symmetric of int * int
    | Custom of int * int * int * int

type CommonProp = 
    | IsVisible of bool * Position
    | Margin of Thickness * Position
    | Width of int * Position
    | Height of int * Position

type Value =
    | Int of int
    | Real of float
    | String of string
    | Bool of bool

type Exp =
    | Constant of Value * Position
    | Var of string * Position
    | Call of string * Position

type Stmt = 
    | Let of string * Exp * Position
    | ExprStmt of Exp * Position

type FunctionT = 
    | Function of string * Stmt list * Position

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
    | Black
    | White

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

type LayoutT =
    | Horizontal
    | Vertical

type TextBlockProp =
    | Foreground of ColorT * Position
    | Background of ColorT * Position
    // Font Settings
    | FontFamily of string * Position
    | FontSize of int * Position
    | FontWeight of int * Position
    | FontStyle of FontStyleT list * Position
    // Formatting
    | LineHeight of int * Position
    | TextWrap of TextWrapT * Position
    | TextAlign of TextAlignT * Position
    | TextTrim of TextTrimT * Position  

type ContainerProp =
    | Layout of LayoutT * Position

type InterleavedProp<'specific> =
  | Common of CommonProp
  | Specific of 'specific

type UIElement = 
    | Button of string * CommonProp list option * Position
    | TextBlock of string * CommonProp list option * TextBlockProp list option * Position
    | TextBox of string * Position 
    | CheckBox of string * Position
    | RadioButton of string * Position
    | ToggleSwitch of string * Position
    | Calendar of Position
    | ToggleButton of Position
    | Container of CommonProp list option * ContainerProp list option * UIElement list * Position

type Window = 
    | Window of string * int option * int option * string option * UIElement list * FunctionT list * Position
