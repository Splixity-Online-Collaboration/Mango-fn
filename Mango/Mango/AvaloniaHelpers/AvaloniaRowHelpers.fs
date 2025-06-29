module AvaloniaHelpers.AvaloniaRowHelpers

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open AbSyn
open Avalonia
open ViewHelpers

let applyMargin props applied =
    applyProp props applied (function
        | RowProp.Margin(m, _) ->
            let thickness =
                match m with
                | Uniform x -> Thickness(float x)
                | Symmetric(x, y) -> Thickness(float x, float y, float x, float y)
                | Custom(l, t, r, b) -> Thickness(float l, float t, float r, float b)

            Some(StackPanel.margin thickness))

let applyRowProperties props = [] |> applyMargin props
