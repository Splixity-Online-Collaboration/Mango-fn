module AvaloniaCommonHelpers

open Avalonia.FuncUI.Types
open Avalonia.Controls
open Avalonia.FuncUI.Builder
open Avalonia
open AbSyn

let applyProp props applied tryExtract =
    props
    |> List.tryPick tryExtract
    |> Option.map (fun attr -> applied @ [attr])
    |> Option.defaultValue applied

let applyMargin<'a when 'a :> Control> (props: CommonProp list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Margin (m, _) ->
            let thickness =
                match m with
                | Uniform x -> Thickness(float x)
                | Symmetric (x, y) -> Thickness(float x, float y, float x, float y)
                | Custom (l, t, r, b) -> Thickness(float l, float t, float r, float b)
            Some (AttrBuilder<'a>.CreateProperty(Control.MarginProperty, thickness, ValueNone))
        | _ -> None)

let applyWidth<'a when 'a :> Control> (props: CommonProp list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Width (num, _) -> Some (AttrBuilder<'a>.CreateProperty(Control.WidthProperty, float num, ValueNone))
        | _ -> None)

let applyHeight<'a when 'a :> Control> (props: CommonProp list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Height (num, _) -> Some (AttrBuilder<'a>.CreateProperty(Control.HeightProperty, float num, ValueNone))
        | _ -> None)

let applyHidden<'a when 'a :> Control> (props: CommonProp list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Hidden (b, _) -> Some (AttrBuilder<'a>.CreateProperty(Control.IsVisibleProperty,not b, ValueNone))
        | _ -> None)

let applyCommonProps props = 
    []
    |> applyHidden props
    |> applyWidth props
    |> applyHeight props
    |> applyMargin props