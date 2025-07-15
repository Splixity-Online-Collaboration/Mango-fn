module AvaloniaHelpers.AvaloniaButtonHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open AbSyn
open ViewHelpers
open AvaloniaCommonHelpers

let applyIsVisible props applied =
    applyProp props applied (function
        | CommonProp (IsVisible (b, _)) -> Some (Button.isVisible b)
        | _ -> None)

let applyWidth props applied =
    applyProp props applied (function
        | ButtonProp (Width (num, _)) -> Some (Button.width num)
        | _ -> None)

let applyHeight props applied =
    applyProp props applied (function
        | ButtonProp (Height (num, _)) -> Some (Button.height num)
        | _ -> None)

let applyButtonProperties props =
    []
    |> applyIsVisible props
    |> applyWidth props
    |> applyHeight props

let createButton (label: string) (props: ButtonProperty list) : IView =
    Button.create ([ Button.content label ] @ [AvaloniaCommonHelpers.applyIsVisible<Button> true []] @ applyButtonProperties props)