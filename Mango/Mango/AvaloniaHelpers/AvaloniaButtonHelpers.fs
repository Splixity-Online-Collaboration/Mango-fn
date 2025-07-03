module AvaloniaHelpers.AvaloniaButtonHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open AbSyn
open ViewHelpers

let applyHidden props applied =
    applyProp props applied (function
        | ButtonProp.Hidden(b, _) -> Some(Button.isVisible (not b))
        | _ -> None)

let applyWidth props applied =
    applyProp props applied (function
        | ButtonProp.Width(num, _) -> Some(Button.width num)
        | _ -> None)

let applyHeight props applied =
    applyProp props applied (function
        | ButtonProp.Height(num, _) -> Some(Button.height num)
        | _ -> None)

let applyButtonProperties props =
    [] |> applyHidden props |> applyWidth props |> applyHeight props

let createButton (label: string) (props: ButtonProp list) : IView =
    Button.create ([ Button.content label ] @ applyButtonProperties props)
