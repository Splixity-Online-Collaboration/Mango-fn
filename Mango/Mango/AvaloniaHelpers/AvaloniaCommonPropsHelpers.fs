module AvaloniaCommonPropsHelpers

open Avalonia.FuncUI.Types

let applyIsVisible props applied =
    applyProp props applied (function
        | IsVisible (b, _) -> Some (Button.isVisible b)
        | _ -> None)

let applyCommonProps props (ctrl: IView list) =
    ctrl
    |> applyIsVisible props
    |> applyWidth props
    |> applyHeight props