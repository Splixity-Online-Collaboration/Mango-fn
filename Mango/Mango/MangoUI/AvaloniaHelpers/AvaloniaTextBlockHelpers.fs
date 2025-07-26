module MangoUI.AvaloniaHelpers.AvaloniaTextBlockHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open MangoUI.AvaloniaHelpers.ColorConverter
open AvaloniaCommonHelpers
open MangoUI.Core.AbSyn

let applyColor props applied =
    applyProp props applied (function
        | Color (c, _) -> Some (TextBlock.foreground (fromColor c))
        | _ -> None)

let applyBackgroundColor props applied =
    applyProp props applied (function
        | TextBlockProp.BackgroundColor (c, _) -> Some (TextBlock.background (fromColor c))
        | _ -> None)

let applyFontFamily props applied =
    applyProp props applied (function
        | TextBlockProp.FontFamily(s, _) -> Some(TextBlock.fontFamily s)
        | _ -> None)

let applyFontSize props applied =
    applyProp props applied (function
        | TextBlockProp.FontSize(i, _) -> Some(TextBlock.fontSize (float i))
        | _ -> None)

let applyTextBlockProperties props =
    []
    |> applyColor props
    |> applyBackgroundColor props
    |> applyFontFamily props
    |> applyFontSize props

let createTextBlock (text: string) (commonProps: CommonProp list) (props: TextBlockProp list) : IView =
    TextBlock.create ([ TextBlock.text text ] @ applyCommonProps commonProps @ applyTextBlockProperties props)
