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
open Avalonia.FuncUI.Types

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

let createButton (label: string) (props: ButtonProp list) : IView = 
    Button.create [
        Button.content label
        Button.isVisible (extractFirstIsVisible props)
    ]

let createTextBlock text : IView = 
    TextBlock.create [
        TextBlock.text text
    ]

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

let convertUIElementToIView element =
    match element with
    | Button (label,props, _) -> createButton label props
    | TextBlock (label, _) -> createTextBlock label
    | TextBox (label, _) -> createTextBox label
    | CheckBox (label, _) -> createCheckbox label
    | RadioButton (label, _) -> createRadioButton label
    | ToggleSwitch (label, _) -> createToggleSwitch label
    | Calendar _ -> createCalendar
    | ToggleButton _ -> createToggleButton

let setWindowContent elements (window: HostWindow) =
    window.Content <- Component(fun _ ->
        DockPanel.create [
            DockPanel.children (List.map convertUIElementToIView elements)
            ]
        )

    window
