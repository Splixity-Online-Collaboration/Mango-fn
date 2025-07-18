module AvaloniaHelpers.AvaloniaTextBlockHelpers

open AvaloniaHelpers.ViewHelpers
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open AbSyn
open AvaloniaHelpers.ColorConverter

let applyForeGround props applied =
    applyProp props applied (function
        | Foreground (c, _) -> Some (TextBlock.foreground (fromColor c))
        | _ -> None)

let applyBackGround props applied =
    applyProp props applied (function
        | Background (c, _) -> Some (TextBlock.background (fromColor c))
        | _ -> None)

let applyFontFamily props applied =
    applyProp props applied (function
        | FontFamily (s, _) -> Some (TextBlock.fontFamily s)
        | _ -> None)

let applyFontSize props applied =
    applyProp props applied (function
        | FontSize (i,_) -> Some (TextBlock.fontSize (float i))
        | _ -> None)

let applyTextBlockProperties props =
    []
    |> applyForeGround props
    |> applyBackGround props
    |> applyFontFamily props
    |> applyFontSize props

let createTextBlock (text: string) (commonProps: CommonProp list) (props: TextBlockProp list) : IView =
    TextBlock.create ([ TextBlock.text text ] @ applyTextBlockProperties props)

