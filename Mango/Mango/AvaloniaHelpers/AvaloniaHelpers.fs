﻿module AvaloniaHelpers.AvaloniaHelpers

open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open AbSyn
open AvaloniaButtonHelpers
open AvaloniaTextBlockHelpers
open Avalonia.FuncUI.Types
open Avalonia.Layout
open AvaloniaContainerHelpers

let setWindowIcon (icon: string option) (window: HostWindow) =
    match icon with
    | Some filename -> 
        window.Icon <- new WindowIcon(filename)
        window
    | None -> window

let setWindowWidthAndHeight width height (window: HostWindow) =
    match width, height with
    | Some w, Some h ->
        window.Width <- float w
        window.Height <- float h
        window
    | _ -> window

let setWindowName name (window: HostWindow) = 
    window.Name <- name
    window

let setWindowProperties name width height icon (window: HostWindow) =
    window
    |> setWindowName name
    |> setWindowWidthAndHeight width height
    |> setWindowIcon icon

let createTextBox text : IView = 
    TextBox.create [
        TextBox.text text
    ]

let createCheckbox (label: string) : IView = 
    CheckBox.create [
        CheckBox.content label
    ]

let createRadioButton (label: string) : IView =
    RadioButton.create [
        RadioButton.content label
    ]

let createToggleSwitch (label: string) : IView =
    ToggleSwitch.create [
        ToggleSwitch.content label
    ]

let createCalendar : IView = Calendar.create []
let createToggleButton = ToggleButton.create []

let rec convertUIElementToIView element =
    match element with
    | Button (label, Some props, _) -> createButton label props
    | Button (label, None, _) -> createButton label []
    | TextBlock (label, Some commonProps, Some props, _) -> createTextBlock label commonProps props
    | TextBlock (label, Some commonProps, None, _) -> createTextBlock label commonProps []
    | TextBlock (label, None, Some props, _) -> createTextBlock label [] props
    | TextBlock (label, None, None, _) -> createTextBlock label [] []
    | TextBox (label, _) -> createTextBox label
    | CheckBox (label, _) -> createCheckbox label
    | RadioButton (label, _) -> createRadioButton label
    | ToggleSwitch (label, _) -> createToggleSwitch label
    | Calendar _ -> createCalendar
    | ToggleButton _ -> createToggleButton
    | Container (Some commonProps, Some containerProps, elements, _) -> createContainer commonProps containerProps elements
    | Container (Some commonProps, None, elements, _) -> createContainer commonProps [] elements
    | Container (None, Some containerProps, elements, _) -> createContainer [] containerProps elements
    | Container (None, None, elements, _) -> createContainer [] [] elements

and createContainer (commonProps: CommonProp list) (props: ContainerProp list) (elements: UIElement list): IView =
    StackPanel.create (
      [ StackPanel.children (List.map convertUIElementToIView elements) ] @ applyContainerCommonProperties commonProps @ applyContainerProperties props
    )

let setWindowContent elements (window: HostWindow) =
    window.Content <- Component(fun _ ->
        StackPanel.create [
            StackPanel.orientation Orientation.Vertical
            StackPanel.children (List.map convertUIElementToIView elements)
        ])
    window
