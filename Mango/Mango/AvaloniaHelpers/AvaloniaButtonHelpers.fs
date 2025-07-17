module AvaloniaHelpers.AvaloniaButtonHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open AbSyn
open AvaloniaCommonHelpers

let createButton (label: string) (props: CommonProp list) : IView =
    Button.create ([ Button.content label ] @ applyCommonProps props)