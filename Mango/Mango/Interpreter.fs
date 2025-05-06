module Interpreter
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open AvaloniaHelpers

let rec interpret (window: HostWindow) program : HostWindow =
    match program with
    | AbSyn.Window (name,width, height, icon, elements, _) -> 
        window |>
        setWindowProperties name width height icon |>
        setWindowContent elements
    