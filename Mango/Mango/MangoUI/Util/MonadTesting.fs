/// <summary>
/// Provides computation expression builders that make it easier to compose
/// Avalonia FuncUI attributes and UI elements in a declarative, functional way.
///
/// The builders in this module allow you to write expressive DSL-like code
/// for building UI attribute lists and nested view hierarchies without
/// explicitly concatenating lists or manually managing composition.
/// </summary>
module MangoUI.Util.MonadTesting

open Avalonia.FuncUI.Types
open Avalonia.Controls

/// <summary>
/// A computation expression builder for declaratively constructing lists
/// of Avalonia FuncUI attributes (<see cref="IAttr{T}" />).
///
/// The <c>PropBuilder</c> enables you to write syntax like:
///
/// <code>
/// let buttonAttrs = attrsFor<Button> {
///     yield Button.content "Click me"
///     yield Button.fontSize 18.0
///     if isPrimary then
///         yield Button.background Brushes.Blue
/// }
/// </code>
///
/// Instead of manually building and concatenating lists of attributes.
/// </summary>
/// <typeparam name="'t">
/// The Avalonia control type for which the attributes are being built.
/// </typeparam>
type PropBuilder<'t when 't :> Control>() =
    /// <summary>
    /// Yields a single attribute into the computation.
    /// </summary>
    member _.Yield(attr: IAttr<'t>) : IAttr<'t> list = [attr]

    /// <summary>
    /// Yields multiple attributes into the computation.
    /// </summary>
    member _.YieldFrom(attrs: IAttr<'t> list) : IAttr<'t> list = attrs

    /// <summary>
    /// Represents an empty attribute list.
    /// </summary>
    member _.Zero() : IAttr<'t> list = []

    /// <summary>
    /// Combines two lists of attributes into one.
    /// </summary>
    member _.Combine(a: IAttr<'t> list, b: IAttr<'t> list) = a @ b

    /// <summary>
    /// Delays evaluation of a computation until needed.
    /// </summary>
    member _.Delay(f: unit -> IAttr<'t> list) = f()

    /// <summary>
    /// Iterates over a sequence, collecting all yielded attributes into a single list.
    /// </summary>
    member _.For(seq: seq<'a>, f: 'a -> IAttr<'t> list) = seq |> Seq.collect f |> Seq.toList

/// <summary>
/// A convenience instance of the <see cref="PropBuilder{T}" /> type for building attributes.
///
/// Example usage:
/// <code>
/// let props = attrsFor<Button> {
///     yield Button.content "Submit"
///     yield Button.onClick (fun _ -> printfn "Clicked!")
/// }
/// </code>
/// </summary>
let attrsFor<'t when 't :> Control> = PropBuilder<'t>()

/// <summary>
/// A computation expression builder for declaratively constructing lists of Avalonia FuncUI views (<see cref="IView" />).
///
/// The <c>UIBuilder</c> allows nested UI elements to be expressed cleanly:
///
/// <code>
/// let myStack =
///     ui {
///         yield TextBlock.create [ TextBlock.text "Hello" ]
///         yield Button.create [ Button.content "Click" ]
///     }
/// </code>
/// </summary>
type UIBuilder() =
    /// <summary>
    /// Yields a single view element into the computation.
    /// </summary>
    member _.Yield(x: IView) : IView list = [x]

    /// <summary>
    /// Yields multiple view elements into the computation.
    /// </summary>
    member _.YieldFrom(xs: IView list) : IView list = xs

    /// <summary>
    /// Represents an empty view list.
    /// </summary>
    member _.Zero() = []

    /// <summary>
    /// Combines two lists of views into one.
    /// </summary>
    member _.Combine(a: IView list, b: IView list) = a @ b

    /// <summary>
    /// Delays evaluation of a computation until needed.
    /// </summary>
    member _.Delay(f: unit -> IView list) = f()

    /// <summary>
    /// Iterates over a sequence, collecting all yielded views into a single list.
    /// </summary>
    member _.For(seq: seq<'a>, f: 'a -> IView list) = seq |> Seq.collect f |> Seq.toList

/// <summary>
/// A convenience instance of the <see cref="UIBuilder" /> type for constructing UI trees.
///
/// Example usage:
/// <code>
/// let myColumn =
///     ui {
///         yield TextBlock.create [ TextBlock.text "Welcome" ]
///         yield Button.create [ Button.content "Start" ]
///     }
/// </code>
/// </summary>
let ui = UIBuilder()