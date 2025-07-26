module MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers

open Avalonia.FuncUI.Types
open Avalonia.Controls
open Avalonia.FuncUI.Builder
open Avalonia
open MangoUI.Core.AbSyn
open Avalonia.Layout

let hasProp props tryExtract =
    props
    |> List.tryPick tryExtract
    |> Option.isSome

let getId props =
    props
    |> List.tryPick (function
        | Id (id, _) -> Some id
        | _ -> None)

// Thickness helper function
let createThickness (t: Thickness) = 
    let thickness =
        match t with 
        | Uniform x -> Thickness(float x)
        | Symmetric (x,y) -> Thickness(float x, float y, float x, float y)
        | Custom (l,t,r,b) -> Thickness(float l,float t, float r,float b)
    thickness

let applyProp props applied tryExtract =
    props
    |> List.tryPick tryExtract
    |> Option.map (fun attr -> applied @ [attr])
    |> Option.defaultValue applied

let applyMargin<'a when 'a :> Control> (props: CommonProp list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Margin (m, _) ->
            Some (AttrBuilder<'a>.CreateProperty(Control.MarginProperty, createThickness m, ValueNone))
        | _ -> None)

let applyWidth<'a when 'a :> Control> (props: CommonProp list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Width (size, _) -> 
            match size with
            | Pixels num -> Some (AttrBuilder<'a>.CreateProperty(Control.WidthProperty, float num, ValueNone))
            | Fill -> Some (AttrBuilder<'a>.CreateProperty(Control.HorizontalAlignmentProperty, HorizontalAlignment.Stretch, ValueNone))
            | Hug -> Some (AttrBuilder<'a>.CreateProperty(Control.HorizontalAlignmentProperty, HorizontalAlignment.Left, ValueNone))
        | _ -> None)

let applyHeight<'a when 'a :> Control> (props: CommonProp list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | Height (size, _) -> 
            match size with
            | Pixels num -> Some (AttrBuilder<'a>.CreateProperty(Control.HeightProperty, float num, ValueNone))
            | Fill -> Some (AttrBuilder<'a>.CreateProperty(Control.VerticalAlignmentProperty, VerticalAlignment.Stretch, ValueNone))
            | Hug -> Some (AttrBuilder<'a>.CreateProperty(Control.VerticalAlignmentProperty, VerticalAlignment.Top, ValueNone))
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