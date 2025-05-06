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
        let newWindow = setWindowProperties window name width height icon
        newWindow.Content <- 
            Component(fun _ ->
                DockPanel.create [
                    DockPanel.children (List.map convertUIElementToIView elements)
                ]
        )
        window

and convertUIElementToIView element =
    match element with
    | AbSyn.Button (label,props, _) -> createButton label props
    | AbSyn.TextBlock (label, _) -> createTextBlock label
    | AbSyn.TextBox (label, _) -> createTextBox label
    | AbSyn.CheckBox (label, _) -> createCheckbox label
    | AbSyn.RadioButton (label, _) -> createRadioButton label
    | AbSyn.ToggleSwitch (label, _) -> createToggleSwitch label
    | AbSyn.Calendar _ -> createCalendar
    | AbSyn.ToggleButton _ -> createToggleButton

    