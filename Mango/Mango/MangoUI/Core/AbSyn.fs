/// <summary>
/// Defines the abstract syntax tree (AST) for the Mango-fn language,
/// including expressions, properties, UI elements, and function definitions.
/// </summary>
/// <remarks>
/// The types in this module represent the syntactic and semantic structure
/// of Mango-fn source code. They form the basis for parsing, semantic analysis,
/// and later translation into Avalonia UI elements and event handlers.
/// </remarks>
module MangoUI.Core.AbSyn

// ------------------------------------------------------------
// Utility functions
// ------------------------------------------------------------

/// <summary>
/// Converts a C-style escaped string (e.g., containing '\n' or '\t')
/// into its unescaped form.
/// </summary>
/// <param name="s">The escaped string to process.</param>
/// <returns>A new string with recognized escape sequences replaced by their character equivalents.</returns>
/// <remarks>
/// Only supports a limited subset of standard C escapes: `\n`, `\t`, and
/// single-character escapes such as `\\` or `\'`.
/// </remarks>
let fromCString (s: string) : string =
    let rec unescape l : char list =
        match l with
        | [] -> []
        | '\\' :: 'n' :: l' -> '\n' :: unescape l'
        | '\\' :: 't' :: l' -> '\t' :: unescape l'
        | '\\' :: c :: l' -> c :: unescape l'
        | c :: l' -> c :: unescape l'

    Seq.toList s |> unescape |> System.String.Concat

// ------------------------------------------------------------
// Fundamental language constructs
// ------------------------------------------------------------

/// <summary>
/// Represents a position in source code, defined by a line and column.
/// </summary>
type Position = int * int // (line, column)

/// <summary>
/// Describes the thickness or spacing around a UI element, similar to CSS margin/padding.
/// </summary>
type Thickness =
    | Uniform of int
    | Symmetric of int * int
    | Custom of int * int * int * int

/// <summary>
/// A predefined set of named colors available in Mango-fn.
/// </summary>
type PredefinedColor =
    | Blue
    | Red
    | Yellow
    | Pink
    | Green
    | Black
    | White

/// <summary>
/// Represents a color defined by a hexadecimal RGBA value.
/// </summary>
/// <remarks>
/// Each component is a byte, where the fourth byte represents alpha transparency.
/// </remarks>
type HexCode = byte * byte * byte * byte

/// <summary>
/// Represents a color in the Mango-fn AST, either by name or hexadecimal code.
/// </summary>
type ColorT =
    | ColorName of PredefinedColor * Position
    | Hex of HexCode * Position

/// <summary>
/// Describes how a UI element should size itself within a layout.
/// </summary>
type Size =
    | Pixels of int
    | Fill
    | Hug

/// <summary>
/// Represents constant literal values in Mango-fn expressions.
/// </summary>
type Value =
    | Int of int
    | Real of float
    | String of string
    | Bool of bool

/// <summary>
/// idk
/// </summary>
type Variable = string * Value

/// <summary>
/// Represents expressions within Mango-fn, such as constants,
/// variable references, or function calls.
/// </summary>
type Exp =
    | Constant of Value * Position
    | Var of string * Position
    | Call of string * Position

// ------------------------------------------------------------
// Text and font-related enums
// ------------------------------------------------------------

/// <summary>
/// Describes text styling options such as italic or underline.
/// </summary>
type FontStyleT =
    | Italic
    | StrikeThrough
    | Underline

/// <summary>
/// Determines how text should wrap within its container.
/// </summary>
type TextWrapT =
    | Overflow
    | Wrap
    | ForceWrap

/// <summary>
/// Controls horizontal alignment of text.
/// </summary>
type TextAlignT =
    | Center
    | Left
    | Right

/// <summary>
/// Controls how overflowing text should be trimmed.
/// </summary>
type TextTrimT =
    | Word
    | Character
    | NoTrim

// ------------------------------------------------------------
// UI Properties and Components
// ------------------------------------------------------------

/// <summary>
/// Represents a single property that can be applied to a UI element.
/// </summary>
/// <remarks>
/// Many properties carry both a value and a <see cref="Position"/> for
/// precise error reporting. These map closely to Avalonia attributes
/// and Mango-fn layout semantics.
/// </remarks>
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
    | OnclickLambda of (Stmt list * Position) option

/// <summary>
/// Represents all Mango-fn UI components that can be declared in source code.
/// </summary>
/// <remarks>
/// Each variant represents a specific UI element type and its associated
/// properties, children, and position in the source code. These structures
/// are later translated into Avalonia UI elements.
/// </remarks>
and UIElement =
    | Button of Property list option * Position
    | TextBlock of Property list option * Position
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

// ------------------------------------------------------------
// Statements and Functions
// ------------------------------------------------------------

/// <summary>
/// Represents executable statements in Mango-fn, forming the
/// body of functions or event handlers.
/// </summary>
/// <remarks>
/// Statements can define variables, update UI properties, or trigger expressions.
/// </remarks>
and Stmt =
    /// <summary>Declares a variable with a given expression value.</summary>
    | Let of string * Exp * Position
    /// <summary>Sets a specific property on a UI element by ID.</summary>
    | SetProperty of Property * string * Exp * Position  // (propertyName, elementId, elementValue, position)
    | SetVariable of string * Exp * Position // (variableName, variableValue, position)
    /// <summary>Updates an existing UI element with a list of modified properties.</summary>
    | Update of string * Property list * Position // (elementId, updatedProperties, position)
    /// <summary>Evaluates a standalone expression.</summary>
    | ExprStmt of Exp * Position
    | StateDecl of Variable list * Position

/// <summary>
/// Represents Mango-fn functions, including named and anonymous lambdas.
/// </summary>
/// <remarks>
/// Functions contain lists of statements to execute when invoked,
/// and are the primary reusable unit in Mango-fn.
/// </remarks>
and FunctionT =
    | Function of string * Stmt list * Position
    | Lambda of Stmt list * Position

/// <summary>
/// Represents a complete Mango-fn window definition, including its title,
/// size, root UI elements, and associated functions.
/// </summary>
/// <remarks>
/// This acts as the top-level program structure parsed from Mango-fn source.
/// It serves as the entry point for UI rendering and runtime execution.
/// </remarks>
type Window =
    | Window of
        string *               // Name
        int option *           // Width
        int option *           // Height
        string option *        // Background color (optional)
        Variable list *        // Variables
        UIElement list *       // UI hierarchy
        FunctionT list *       // Functions
        Position               // Source position