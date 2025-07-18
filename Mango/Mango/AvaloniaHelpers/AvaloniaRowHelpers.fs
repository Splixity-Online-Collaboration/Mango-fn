module AvaloniaHelpers.AvaloniaRowHelpers

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open AbSyn
open Avalonia
open ViewHelpers
open ColorConverter

let applyMargin props applied =
    applyProp props applied (function
        | RowProp.Margin(m, _) ->
            let thickness =
                match m with
                | Uniform x -> Thickness(float x)
                | Symmetric(x, y) -> Thickness(float x, float y, float x, float y)
                | Custom(l, t, r, b) -> Thickness(float l, float t, float r, float b)

            Some(Panel.margin thickness)
        | _ -> None)

let applyWidth props applied =
    applyProp props applied (function
        | RowProp.Width(num, _) -> Some(Panel.width num)
        | _ -> None)

let applyHeight props applied =
    applyProp props applied (function
        | RowProp.Height(num, _) -> Some(Panel.height num)
        | _ -> None)

let applyBackgroundColor props applied =
    applyProp props applied (function
        | RowProp.BackgroundColor(c, _) -> Some(Panel.background (fromColor c))
        | _ -> None)

let applyRowProperties props =
    []
    |> applyMargin props
    |> applyWidth props
    |> applyHeight props
    |> applyBackgroundColor props
