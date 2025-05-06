module AvaloniaHelpers
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open AbSyn

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

let setWindowProperties (window: HostWindow) name width height icon =
    window |>
    setWindowName name |>
    setWindowWidthAndHeight width height |>
    setWindowIcon icon

let extractFirstIsVisible (props: ButtonProp list) =
    props
    |> List.tryPick (function
        | IsVisible (b, _) -> Some b
        | _ -> None)
    |> Option.defaultValue false

let createButton (label: string) (props: ButtonProp list) = 
    Button.create [
        Button.content label
        Button.isVisible (extractFirstIsVisible props)
    ]

let createTextBlock text = 
    TextBlock.create [
        TextBlock.text text
    ]

let createTextBox text = 
    TextBox.create [
        TextBox.text text
    ]

let createCheckbox (label: string) = 
    CheckBox.create [
        CheckBox.content label
    ]

let createRadioButton (label: string) =
    RadioButton.create [
        RadioButton.content label
    ]

let createToggleSwitch (label: string) =
    ToggleSwitch.create [
        ToggleSwitch.content label
    ]

let createCalendar = Calendar.create []

let createToggleButton = ToggleButton.create []