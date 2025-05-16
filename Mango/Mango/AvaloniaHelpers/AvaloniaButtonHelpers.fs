module AvaloniaHelpers.AvaloniaButtonHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open AbSyn
open ViewHelpers
let applyIsVisible props applied =
    applyProp props applied (function
        | IsVisible (b, _) -> Some (Button.isVisible b)
        | _ -> None)

let applyWidth props applied =
    applyProp props applied (function
        | Width (num, _) -> Some (Button.width num)
        | _ -> None)

let applyHeight props applied =
    applyProp props applied (function
        | Height (num, _) -> Some (Button.height num)
        | _ -> None)

let applyButtonProperties props =
    []
    |> applyIsVisible props
    |> applyWidth props
    |> applyHeight props

let createButton (label: string) (props: ButtonProp list) : IView =
    Button.create ([ Button.content label ] @ applyButtonProperties props)