module MangoUI.AvaloniaHelpers.AvaloniaHelpers

open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open AvaloniaButtonHelpers
open AvaloniaTextBlockHelpers
open Avalonia.FuncUI.Types
open Avalonia.Layout
open AvaloniaBorderHelpers
open MangoUI.Core.AbSyn
open MangoUI
open MangoUI.Core.Types
open AvaloniaCommonHelpers

let doesWrapExist props = props |> List.exists(fun prop -> match prop with | Wrap (true,_)-> true |  _ -> false)

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

let rec convertUIElementToIView element (tab : TreeEnv) =
    match element with
    | Button(label, propsOpt, _) ->
        createButton label (Option.defaultValue [] propsOpt)
    | TextBlock(label, propsOpt, _) ->
        createTextBlock label (Option.defaultValue [] propsOpt)
    | TextBox(label, _) -> createTextBox label
    | CheckBox(label, _) -> createCheckbox label
    | RadioButton(label, _) -> createRadioButton label
    | ToggleSwitch(label, _) -> createToggleSwitch label
    | Calendar _ -> createCalendar
    | ToggleButton _ -> createToggleButton
    | Row(propsOpt, elements, _) ->
        createContainer Orientation.Horizontal (Option.defaultValue [] propsOpt) elements tab
    | Column(propsOpt, elements, _) ->
        createContainer Orientation.Vertical (Option.defaultValue [] propsOpt) elements tab
    | Identifier (id, _) ->
        match SymTab.lookup id tab with
        | Some storedElement -> convertUIElementToIView storedElement tab
        | None -> failwithf "Identifier '%s' not found in symbol table." id
    | Border(propsOpt, element, _) ->
        createBorderElement (Option.defaultValue [] propsOpt)element tab

and createWrapPanel orientation elements props tab =
    WrapPanel.create (
            [   WrapPanel.orientation orientation
                WrapPanel.children (List.map (fun e -> convertUIElementToIView e tab) elements) ]
            @ applyCommonProps props
        )

and createStackPanel orientation elements props tab =
    StackPanel.create (
        [StackPanel.orientation orientation
         StackPanel.children (List.map (fun e -> convertUIElementToIView e tab) elements) 
        ] @ applyCommonProps props
    )

and createContainer (orientation: Orientation) (props: Property list) (elements: UIElement list)  (tab : TreeEnv) : IView =
    let hasWrap = doesWrapExist props
    if hasWrap then createWrapPanel orientation elements props tab
    else createStackPanel orientation elements props tab

and createBorderElement (props: Property list) (element: UIElement) (tab : TreeEnv): IView = 
  Border.create([
    Border.child (convertUIElementToIView element tab)
  ] @ applyCommonProps props @ applyBorderProperties props)

let setWindowContent elements (tab : TreeEnv) (window: HostWindow) =
    window.Content <-
        Component(fun _ ->
            ScrollViewer.create
                [ ScrollViewer.content (
                      StackPanel.create [ StackPanel.children (List.map (fun e -> convertUIElementToIView e tab) elements) ]
                  ) ])

    window