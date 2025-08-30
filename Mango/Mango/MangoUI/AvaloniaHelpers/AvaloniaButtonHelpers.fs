module MangoUI.AvaloniaHelpers.AvaloniaButtonHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers
open MangoUI.Core.AbSyn
open MangoUI.Core.Types

let applyOnClick props dispatch applied =
    applyProp props applied (function
        | Onclick(Some (funcName, _)) -> Some (Button.onClick (fun _ -> dispatch (EvalFunc funcName)))
        | _ -> None)

let applyLabel props applied =
    applyProp props applied (function
        | Label(Some (text, _)) -> Some (Button.content text)
        | _ -> None)

let applyButtonProperties props dispatch =
    []
    |> applyOnClick props dispatch
    |> applyLabel props

let createButton (props: Property list) treeEnv funcEnv dispatch : IView =
    Button.create (applyCommonProps props @ applyButtonProperties props dispatch)