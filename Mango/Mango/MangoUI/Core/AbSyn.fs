module MangoUI.Core.AbSyn

// Doesn't actually support all escapes. Too bad.
let fromCString (s: string) : string =
    let rec unescape l : char list =
        match l with
        | []                -> []
        | '\\' :: 'n' :: l' -> '\n' :: unescape l'
        | '\\' :: 't' :: l' -> '\t' :: unescape l'
        | '\\' :: c   :: l' -> c    :: unescape l'
        | c    :: l'        -> c    :: unescape l'

    Seq.toList s |> unescape |> System.String.Concat

type Position = int * int // (line, column)

type Thickness =
    | Uniform of int
    | Symmetric of int * int
    | Custom of int * int * int * int

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

type Size =
    | Pixels of int
    | Fill
    | Hug

type Value =
    | Int of int
    | Real of float
    | String of string
    | Bool of bool

type Exp =
    | Constant of Value * Position
    | Var of string * Position
    | Call of string * Position

type FontStyleT = 
    | Italic
    | StrikeThrough
    | Underline

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

type Property =
    | Hidden of (bool * Position) option
    | Margin of (Thickness * Position) option
    | Width of (Size * Position) option
    | Height of (Size * Position) option
    | Id of (string * Position) option
    | Color of (ColorT * Position) option
    | BackgroundColor of (ColorT * Position) option
    | FontFamily of (string * Position) option
    | FontSize of (int * Position) option
    | FontWeight of (int * Position) option
    | FontStyle of (FontStyleT list * Position) option
    | LineHeight of (int * Position) option
    | TextAlign of (TextAlignT * Position) option
    | TextTrim of (TextTrimT * Position) option
    | TextWrap of (TextWrapT * Position) option
    | Corner of (Thickness * Position) option
    | Density of (Thickness * Position) option
    | Wrap of (bool * Position) option
    | Label of (string * Position) option
    | Onclick of (string * Position) option

type UIElement = 
    | Button of string * Property list option * Position
    | TextBlock of string * Property list option * Position
    | TextBox of string * Position 
    | CheckBox of string * Position
    | RadioButton of string * Position
    | ToggleSwitch of string * Position
    | Calendar of Position
    | ToggleButton of Position
    | Row of Property list option * UIElement list * Position
    | Column of Property list option * UIElement list * Position
    | Border of Property list option * UIElement * Position
    | Identifier of string * Position

type Stmt =
    | Let of string * Exp * Position
    | Set of Property * string * Exp * Position // (propertyName, elementId, elementValue, position)
    | Update of string * Property list * Position // (elementId, updatedProperties, position)
    | ExprStmt of Exp * Position

type FunctionT = 
    | Function of string * Stmt list * Position

type Window = Window of string * int option * int option * string option * UIElement list * FunctionT list * Position
