module AvaloniaHelpers.AvaloniaContainerHelpers

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open AbSyn
open Avalonia.Layout
open Avalonia
open ViewHelpers
let applyOrientation props applied =
    applyProp props applied (function
        | Layout (layout, _) ->
            match layout with
            | Horizontal -> Some (StackPanel.orientation Orientation.Horizontal)
            | Vertical -> Some (StackPanel.orientation Orientation.Vertical)
        | _ -> None)

let applyMargin props applied =
    applyProp props applied (function
        | Margin (m, _) ->
            let thickness =
                match m with
                | Uniform x -> Thickness(float x)
                | Symmetric (x, y) -> Thickness(float x, float y, float x, float y)
                | Custom (l, t, r, b) -> Thickness(float l, float t, float r, float b)
            Some (StackPanel.margin thickness)
        | _ -> None)

let applyContainerProperties props =
    []
    |> applyOrientation props
    |> applyMargin props