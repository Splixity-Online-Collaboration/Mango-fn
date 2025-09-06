module MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers

open Avalonia.FuncUI.Types
open Avalonia.Controls
open Avalonia.FuncUI.Builder
open Avalonia
open MangoUI.Core.AbSyn
open Avalonia.Layout
open MangoUI.AvaloniaHelpers.ColorConverter

let hasProp props tryExtract =
    props |> List.tryPick tryExtract |> Option.isSome

let getId props =
    props
    |> List.tryPick (function
        | Id(Some(id, _)) -> Some id
        | _ -> None)

// Thickness helper function
let createThickness (t: Thickness) =
    let thickness =
        match t with
        | Thickness.Uniform x -> Thickness(float x)
        | Thickness.Symmetric(x, y) -> Thickness(float x, float y, float x, float y)
        | Thickness.Custom(l, t, r, b) -> Thickness(float l, float t, float r, float b)

    thickness

let applyProp props applied tryExtract =
    props
    |> List.tryPick tryExtract
    |> Option.map (fun attr -> applied @ [ attr ])
    |> Option.defaultValue applied

let applyMargin<'a when 'a :> Control> (props: Property list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Margin(Some(m, _)) ->
            Some(AttrBuilder<'a>.CreateProperty(Control.MarginProperty, createThickness m, ValueNone))
        | _ -> None)

let applyWidth<'a when 'a :> Control> (props: Property list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Width(Some(size, _)) ->
            match size with
            | Size.Pixels num -> Some(AttrBuilder<'a>.CreateProperty(Control.WidthProperty, float num, ValueNone))
            | Size.Fill ->
                Some(
                    AttrBuilder<'a>
                        .CreateProperty(Control.HorizontalAlignmentProperty, HorizontalAlignment.Stretch, ValueNone)
                )
            | Size.Hug ->
                Some(
                    AttrBuilder<'a>
                        .CreateProperty(Control.HorizontalAlignmentProperty, HorizontalAlignment.Left, ValueNone)
                )
        | _ -> None)

let applyHeight<'a when 'a :> Control> (props: Property list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Height(Some(size, _)) ->
            match size with
            | Size.Pixels num -> Some(AttrBuilder<'a>.CreateProperty(Control.HeightProperty, float num, ValueNone))
            | Size.Fill ->
                Some(
                    AttrBuilder<'a>
                        .CreateProperty(Control.VerticalAlignmentProperty, VerticalAlignment.Stretch, ValueNone)
                )
            | Size.Hug ->
                Some(
                    AttrBuilder<'a>.CreateProperty(Control.VerticalAlignmentProperty, VerticalAlignment.Top, ValueNone)
                )
        | _ -> None)

let applyHidden<'a when 'a :> Control> (props: Property list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Hidden(Some(b, _)) -> Some(AttrBuilder<'a>.CreateProperty(Control.IsVisibleProperty, not b, ValueNone))
        | _ -> None)

let applyBackgroundColor props applied =
    applyProp props applied (function
        | BackgroundColor(Some(c, _)) ->
            Some(AttrBuilder<'a>.CreateProperty(Panel.BackgroundProperty, fromColor c, ValueNone))
        | _ -> None)

let applyCommonProps props =
    []
    |> applyHidden props
    |> applyWidth props
    |> applyHeight props
    |> applyMargin props
    |> applyBackgroundColor props
