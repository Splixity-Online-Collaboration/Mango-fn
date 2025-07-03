module AbSyn

// Doesn't actually support all escapes. Too bad.
let fromCString (s: string) : string =
    let rec unescape l : char list =
        match l with
        | [] -> []
        | '\\' :: 'n' :: l' -> '\n' :: unescape l'
        | '\\' :: 't' :: l' -> '\t' :: unescape l'
        | '\\' :: c :: l' -> c :: unescape l'
        | c :: l' -> c :: unescape l'

    Seq.toList s |> unescape |> System.String.Concat

(* position: (line, column) *)
type Position = int * int // row, column

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

type FunctionT = Function of string * Stmt list * Position

// Experimental, have to implement first
type WidthT =
    | Pixels of int
    | Hug
    | Fill
    | Percentage of int

type HeightT =
    | Pixels of int
    | Hug
    | Fill
    | Percentage of int

type ButtonProp =
    | Hidden of bool * Position
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
    | Black
    | White

type HexCode = byte * byte * byte * byte

type ColorT =
    | ColorName of PredefinedColor * Position
    | Hex of HexCode * Position

type TextAlignT =
    | Center
    | Left
    | Right

type TextTrimT =
    | Word
    | Character
    | NoTrim

type Thickness =
    | Uniform of int
    | Symmetric of int * int
    | Custom of int * int * int * int

type TextBlockProp =
    | Hidden of bool * Position
    | Color of ColorT * Position
    | BackgroundColor of ColorT * Position
    // Font Settings
    | FontFamily of string * Position
    | FontSize of int * Position
    | FontWeight of int * Position
    | FontStyle of FontStyleT list * Position
    // Margin
    | Margin of Thickness * Position
    // Formatting
    | LineHeight of int * Position
    | TextAlign of TextAlignT * Position
    | TextTrim of TextTrimT * Position

type RowProp =
    | Margin of Thickness * Position
    | Wrap of bool * Position
    | Width of int * Position
    | Height of int * Position
    | BackgroundColor of ColorT * Position

type ColumnProp =
    | Margin of Thickness * Position
    | Wrap of bool * Position
    | Width of int * Position
    | Height of int * Position
    | BackgroundColor of ColorT * Position

// TODO implement
type UIElementProp =
    | Margin of Thickness * Position
    | Width of int * Position
    | Height of int * Position

type UIElement =
    | Button of string * ButtonProp list option * Position
    | TextBlock of string * TextBlockProp list option * Position
    | TextBox of string * Position
    | CheckBox of string * Position
    | RadioButton of string * Position
    | ToggleSwitch of string * Position
    | Calendar of Position
    | ToggleButton of Position
    | Row of RowProp list option * UIElement list option * Position
    | Column of ColumnProp list option * UIElement list option * Position

type Window = Window of string * int option * int option * string option * UIElement list * FunctionT list * Position
