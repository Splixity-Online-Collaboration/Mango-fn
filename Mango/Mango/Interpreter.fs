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
    | AbSyn.Button (label,props, _) ->
        let isVisibleProp = List.map (fun elem ->
            match elem with
            | AbSyn.IsVisible (v, _) -> v
        )
        let v = isVisibleProp props
        Button.create [
            Button.content label
            Button.isVisible v.Head
        ]
    | AbSyn.TextBlock (label, _) ->
        TextBlock.create [
            TextBlock.text label
        ]
    | AbSyn.TextBox (label, _) ->
        TextBox.create [
            TextBox.text label
        ]
    | AbSyn.CheckBox (label, _) ->
        CheckBox.create [
            CheckBox.content label
        ]
    | AbSyn.RadioButton (label, _) ->
        RadioButton.create [
            RadioButton.content label
        ]
    | AbSyn.Calendar _ -> Calendar.create []
    