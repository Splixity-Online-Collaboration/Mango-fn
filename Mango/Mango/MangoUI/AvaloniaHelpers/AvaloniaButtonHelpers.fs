module MangoUI.AvaloniaHelpers.AvaloniaButtonHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers
open MangoUI.Core.AbSyn
open MangoUI.Core.Types
open MangoUI.Util.MonadTesting

let buttonAttrs (props: Property list) dispatch =
    attrsFor {
        for prop in props do
            match prop with
            | Onclick(Some(funcName, _)) ->
                yield Button.onClick (fun _ -> dispatch (EvalFunc funcName))

            | OnclickLambda(Some(stmts, _)) ->
                yield Button.onClick (fun _ -> dispatch (EvalLambda stmts))

            | Label(Some(text, _)) ->
                yield Button.content text

            | _ -> ()
    }

let createButton (props: Property list) dispatch : IView =
    Button.create (
        attrsFor {
            yield! applyCommonProps props
            yield! buttonAttrs props dispatch
        }
    )
