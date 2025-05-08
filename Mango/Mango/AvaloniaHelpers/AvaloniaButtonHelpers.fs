module AvaloniaHelpers.AvaloniaButtonHelpers
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open AbSyn

let applyIsVisible (props: ButtonProp list) (applied: IAttr<Button> list) : IAttr<Button> list =
    props
    |> List.tryPick (function
        | IsVisible (b, _) -> Some (List.append applied [ Button.isVisible b ])
        | _ -> None)
    |> Option.defaultValue applied

let applyWidth (props: ButtonProp list) (applied: IAttr<Button> list) : IAttr<Button> list =
    props
    |> List.tryPick (function
        | Width (num, _) -> Some (List.append applied [ Button.width num ])
        | _ -> None)
    |> Option.defaultValue applied

let applyHeight (props: ButtonProp list) (applied: IAttr<Button> list) : IAttr<Button> list =
    props
    |> List.tryPick (function
        | Height (num, _) -> Some (List.append applied [ Button.height num ])
        | _ -> None)
    |> Option.defaultValue applied

let applyButtonProperties props = 
    []
    |> applyIsVisible props
    |> applyWidth props
    |> applyHeight props

let createButton (label: string) (props: ButtonProp list) : IView = 
    Button.create (List.append [ Button.content label] (applyButtonProperties props))