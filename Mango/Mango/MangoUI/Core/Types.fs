/// <summary>
/// Contains core type definitions for MangoUI, including environment types,
/// application state, and message variants used in the reactive update cycle.
/// </summary>
/// <remarks>
/// This module defines the fundamental data structures that connect the
/// Mango-fn runtime (AST evaluation, function environment, and UI element tree)
/// with the Avalonia-based rendering and event dispatch system.
/// </remarks>
module MangoUI.Core.Types

open MangoUI.SymTab
open AbSyn

/// <summary>
/// Represents the symbol table for UI elements, mapping identifiers
/// to their corresponding <see cref="UIElement" /> definitions.
/// </summary>
/// <remarks>
/// This environment holds the currently active Mango-fn UI definitions
/// in memory. It is typically updated when UI components are defined,
/// redefined, or deleted at runtime.
/// </remarks>
type TreeEnv = SymTab<UIElement>

/// <summary>
/// Represents the symbol table for Mango-fn functions, mapping function names
/// to their corresponding statement lists (<see cref="Stmt list" />).
/// </summary>
/// <remarks>
/// This environment is used by the Mango-fn interpreter to resolve and evaluate
/// function definitions dynamically at runtime.
/// </remarks>
type FuncEnv = SymTab<Stmt list>

/// <summary>
/// idk
/// </summary>
type VarEnv = SymTab<Value>

/// <summary>
/// Represents messages that can be dispatched in the MangoUI runtime.
/// </summary>
/// <remarks>
/// The <see cref="Msg" /> type defines the update actions used to modify
/// the Mango-fn application state, handle events, and evaluate user-defined
/// functions or expressions. These messages are typically sent from the UI layer
/// (e.g., Avalonia event handlers) and processed by the state update logic.
/// </remarks>
type Msg =
    /// <summary>
    /// Updates the function environment with a new or modified function definition.
    /// </summary>
    /// <param name="FuncEnv">The current function environment.</param>
    /// <param name="string">The name of the function being updated.</param>
    /// <param name="Stmt list">The statement list representing the function body.</param>
    | UpdateFuncEnv of FuncEnv * string * Stmt list

    /// <summary>
    /// Updates the tree environment with a new or modified UI element.
    /// </summary>
    /// <param name="TreeEnv">The current UI tree environment.</param>
    /// <param name="string">The identifier of the UI element being updated.</param>
    /// <param name="UIElement">The new or modified UI element definition.</param>
    | UpdateTreeEnv of TreeEnv * string * UIElement

    /// <summary>
    /// Replaces the current list of top-level UI elements.
    /// </summary>
    /// <param name="UIElement list">The new collection of root UI elements.</param>
    | UpdateUIElements of UIElement list

    /// <summary>
    /// Triggers the evaluation of a named Mango-fn function.
    /// </summary>
    /// <param name="string">The name of the function to evaluate.</param>
    | EvalFunc of string

    /// <summary>
    /// Triggers the evaluation of an anonymous function or lambda expression.
    /// </summary>
    /// <param name="Stmt list">The statement list representing the lambda body.</param>
    | EvalLambda of Stmt list

/// <summary>
/// Represents the full state of a MangoUI application, including
/// its function environment, UI tree, and currently rendered elements.
/// </summary>
/// <remarks>
/// This record acts as the model in MangoUI’s Elm-style architecture.
/// It holds all reactive state that can be mutated via dispatched messages.
/// </remarks>
type AppState =
    { /// <summary>
      /// The symbol table of all known UI element definitions.
      /// </summary>
      treeEnv: TreeEnv

      /// <summary>
      /// The symbol table of all known Mango-fn variable definitions.
      /// </summary>
      varEnv: VarEnv

      /// <summary>
      /// The symbol table of all known Mango-fn function definitions.
      /// </summary>
      funcEnv: FuncEnv

      /// <summary>
      /// The current set of active UI elements rendered on screen.
      /// </summary>
      uiElements: UIElement list }

/// <summary>
/// Exception raised when a syntax error occurs during Mango-fn parsing.
/// </summary>
/// <param name="obj">The raw parser error context or related data.</param>
/// <remarks>
/// This exception is raised by the Mango-fn parser when it encounters
/// invalid syntax or an unexpected token. The contained object may hold
/// parser context or diagnostic data, depending on the implementation.
/// </remarks>
exception SyntaxError of obj (* ParseErrorContext<_> *)