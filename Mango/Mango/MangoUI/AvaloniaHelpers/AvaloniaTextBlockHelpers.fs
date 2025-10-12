module MangoUI.AvaloniaHelpers.AvaloniaTextBlockHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open MangoUI.AvaloniaHelpers.ColorConverter
open AvaloniaCommonHelpers
open MangoUI.Core.AbSyn
open MangoUI.Util.MonadTesting

let textblockAttrs (props: Property list) =
    attrsFor {
        for prop in props do
            match prop with
            | Label(Some(l, _)) -> TextBlock.text l
            | FontSize(Some(i, _)) -> TextBlock.fontSize (float i)
            | FontFamily(Some(s, _)) -> TextBlock.fontFamily s
            | BackgroundColor(Some(c, _)) -> TextBlock.background (fromColor c)
            | Color(Some(c, _)) -> TextBlock.foreground (fromColor c)
            | _ -> ()
    }

let createTextBlock (props: Property list) : IView =
    TextBlock.create (attrsFor {
            yield! applyCommonProps props
            yield! textblockAttrs props
        })
