module MangoUI.AvaloniaHelpers.AvaloniaButtonHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers
open MangoUI.Core.AbSyn

let createButton (label: string) (props: CommonProp list) : IView =
    Button.create ([ Button.content label ] @ applyCommonProps props)