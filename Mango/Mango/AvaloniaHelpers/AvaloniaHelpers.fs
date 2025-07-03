module AvaloniaHelpers.AvaloniaHelpers

open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open AbSyn
open AvaloniaButtonHelpers
open AvaloniaTextBlockHelpers
open Avalonia.FuncUI.Types
open Avalonia.Layout
open AvaloniaRowHelpers
open AvaloniaColumnHelpers

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

let createTextBox text : IView = TextBox.create [ TextBox.text text ]

let createCheckbox (label: string) : IView =
    CheckBox.create [ CheckBox.content label ]

let createRadioButton (label: string) : IView =
    RadioButton.create [ RadioButton.content label ]

let createToggleSwitch (label: string) : IView =
    ToggleSwitch.create [ ToggleSwitch.content label ]

let createCalendar: IView = Calendar.create []
let createToggleButton = ToggleButton.create []

let rec convertUIElementToIView element =
    match element with
    | Button(label, props, _) -> createButton label (defaultArg props [])
    | TextBlock(label, props, _) -> createTextBlock label (defaultArg props [])
    | TextBox(label, _) -> createTextBox label
    | CheckBox(label, _) -> createCheckbox label
    | RadioButton(label, _) -> createRadioButton label
    | ToggleSwitch(label, _) -> createToggleSwitch label
    | Calendar _ -> createCalendar
    | ToggleButton _ -> createToggleButton
    | Row(props, elements, _) -> createRow (defaultArg props [], defaultArg elements [])
    | Column(props, elements, _) -> createColumn (defaultArg props [], defaultArg elements [])

and createRow (props: RowProp list, elements: UIElement list) : IView =
    printfn "Row props: %A" props
    // decide here whether to Stackpanel or wrappanel

    WrapPanel.create (
        [ WrapPanel.orientation Orientation.Horizontal
          WrapPanel.children (List.map convertUIElementToIView elements) ]
        @ applyRowProperties props
    )

and createColumn (props: ColumnProp list, elements: UIElement list) : IView =
    WrapPanel.create (
        [ WrapPanel.orientation Orientation.Vertical
          WrapPanel.children (List.map convertUIElementToIView elements) ]
        @ applyColumnProperties props
    )

let setWindowContent elements (window: HostWindow) =
    window.Content <-
        Component(fun _ ->
            ScrollViewer.create
                [ ScrollViewer.content (
                      StackPanel.create [ StackPanel.children (List.map convertUIElementToIView elements) ]
                  ) ])

    window
