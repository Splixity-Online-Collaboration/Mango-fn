module MangoUI.AvaloniaHelpers.AvaloniaTextBlockHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open MangoUI.AvaloniaHelpers.ColorConverter
open AvaloniaCommonHelpers
open MangoUI.Core.AbSyn

let applyColor props applied =
    applyProp props applied (function
        | Color (Some (c, _)) -> Some (TextBlock.foreground (fromColor c))
        | _ -> None)

let applyBackgroundColor props applied =
    applyProp props applied (function
        | BackgroundColor (Some (c, _)) -> Some (TextBlock.background (fromColor c))
        | _ -> None)

let applyFontFamily props applied =
    applyProp props applied (function
        | FontFamily(Some (s, _)) -> Some(TextBlock.fontFamily s)
        | _ -> None)

let applyFontSize props applied =
    applyProp props applied (function
        | FontSize(Some (i, _)) -> Some(TextBlock.fontSize (float i))
        | _ -> None)

let applyTextBlockProperties props =
    []
    |> applyColor props
    |> applyBackgroundColor props
    |> applyFontFamily props
    |> applyFontSize props

let createTextBlock (text: string) (props: Property list) : IView =
    TextBlock.create ([ TextBlock.text text ] @ applyCommonProps props @ applyTextBlockProperties props)
