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
open AvaloniaCommonHelpers
open AvaloniaContainerHelpers
open ColorConverter
open Types

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

let createCalendar : IView = Calendar.create []
let createToggleButton = ToggleButton.create []

let rec convertUIElementToIView element =
    match element with
    | Button(label, propsOpt, _) ->
        createButton label (Option.defaultValue [] propsOpt)
    | TextBlock(label, commonPropsOpt, propsOpt, _) ->
        createTextBlock label (Option.defaultValue [] commonPropsOpt) (Option.defaultValue [] propsOpt)
    | TextBox(label, _) -> createTextBox label
    | CheckBox(label, _) -> createCheckbox label
    | RadioButton(label, _) -> createRadioButton label
    | ToggleSwitch(label, _) -> createToggleSwitch label
    | Calendar _ -> createCalendar
    | ToggleButton _ -> createToggleButton
    | Row(commonPropsOpt, containerPropsOpt, elements, _) ->
        createContainer Orientation.Horizontal (Option.defaultValue [] commonPropsOpt) (Option.defaultValue [] containerPropsOpt) elements
    | Column(commonPropsOpt, containerPropsOpt, elements, _) ->
        createContainer Orientation.Vertical (Option.defaultValue [] commonPropsOpt) (Option.defaultValue [] containerPropsOpt) elements

and createContainer (orientation: Orientation) (commonProps: CommonProp list) (props: ContainerProp list) (elements: UIElement list) : IView =
    let hasWrap =
        props |> List.exists(fun prop-> 
        match prop with 
            | Wrap (true,_)-> true 
            |  _ -> false
        )
    let isBorder =
            props |> List.exists(fun prop -> match prop with | ContainerProp.Border(_,_,_) -> true | _ -> false)
    if hasWrap then
        let wrapPanel = 
            WrapPanel.create (
            [   WrapPanel.orientation orientation
                WrapPanel.children (List.map convertUIElementToIView elements) ]
            @ applyCommonProps commonProps
            @ applyContainerProperties props
        )
        if isBorder then
            let color, thickness =
                props
                |> List.pick (function
                    | Border (c, t, _) -> Some (c, t)
                    | _ -> None)

            Border.create([
                Border.borderBrush(fromColor color)
                Border.borderThickness (createThickness thickness)
                Border.child wrapPanel
            ])
        else
            wrapPanel  
    else
        let stackPanel = 
            StackPanel.create (
            [   StackPanel.orientation orientation
                StackPanel.children (List.map convertUIElementToIView elements) ]
            @ applyCommonProps commonProps
            @ applyContainerProperties props
        )
        if isBorder then
            let color, thickness =
                props
                |> List.pick (function
                    | Border(c, t, _) -> Some(c, t)
                    | _ -> None)

            Border.create ([
                    Border.borderBrush (fromColor color)
                    Border.borderThickness (createThickness thickness)
                    Border.child stackPanel 
                ])
        else
            stackPanel 

let setWindowContent elements (tab : TreeEnv) (window: HostWindow) =
    window.Content <-
        Component(fun _ ->
            ScrollViewer.create
                [ ScrollViewer.content (
                      StackPanel.create [ StackPanel.children (List.map convertUIElementToIView elements) ]
                  ) ])

    window