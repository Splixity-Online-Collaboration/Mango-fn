module MangoUI.AvaloniaHelpers.AvaloniaHelpers

open Avalonia.FuncUI.Hosts
open Avalonia.Controls
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

let doesWrapExist props =
    props
    |> List.exists (fun prop ->
        match prop with
        | Wrap(Some(true, _)) -> true
        | _ -> false)

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

let rec convertUIElementToIView element (tab: TreeEnv) (funcEnv: FuncEnv) dispatch =
    match element with
    | Button(propsOpt, _) -> createButton (Option.defaultValue [] propsOpt) tab funcEnv dispatch
    | TextBlock(propsOpt, _) -> createTextBlock (Option.defaultValue [] propsOpt)
    | TextBox(label, _) -> createTextBox label
    | CheckBox(label, _) -> createCheckbox label
    | RadioButton(label, _) -> createRadioButton label
    | ToggleSwitch(label, _) -> createToggleSwitch label
    | Calendar _ -> createCalendar
    | ToggleButton _ -> createToggleButton
    | Row(propsOpt, elements, _) ->
        createContainer Orientation.Horizontal (Option.defaultValue [] propsOpt) elements tab funcEnv dispatch
    | Column(propsOpt, elements, _) ->
        createContainer Orientation.Vertical (Option.defaultValue [] propsOpt) elements tab funcEnv dispatch
    | Identifier(id, _) ->
        match SymTab.lookup id tab with
        | Some storedElement -> convertUIElementToIView storedElement tab funcEnv dispatch
        | None -> failwithf "Identifier '%s' not found in symbol table." id
    | Border(propsOpt, element, _) -> createBorderElement (Option.defaultValue [] propsOpt) element tab funcEnv dispatch

and createWrapPanel orientation elements props tab funcEnv dispatch =
    WrapPanel.create (
        [ WrapPanel.orientation orientation
          WrapPanel.children (List.map (fun e -> convertUIElementToIView e tab funcEnv dispatch) elements) ]
        @ applyCommonProps props
    )

and createStackPanel orientation elements props tab funcEnv dispatch =
    StackPanel.create (
        [ StackPanel.orientation orientation
          StackPanel.children (List.map (fun e -> convertUIElementToIView e tab funcEnv dispatch) elements) ]
        @ applyCommonProps props
    )

and createContainer
    (orientation: Orientation)
    (props: Property list)
    (elements: UIElement list)
    (tab: TreeEnv)
    (funcEnv: FuncEnv)
    dispatch
    : IView =
    let hasWrap = doesWrapExist props

    if hasWrap then
        createWrapPanel orientation elements props tab funcEnv dispatch
    else
        createStackPanel orientation elements props tab funcEnv dispatch

and createBorderElement (props: Property list) (element: UIElement) (tab: TreeEnv) (funcEnv: FuncEnv) dispatch : IView =
    Border.create (
        [ Border.child (convertUIElementToIView element tab funcEnv dispatch) ]
        @ applyCommonProps props
        @ applyBorderProperties props
    )

let convertFromAbSynToAvaloniaTree (state: AppState) dispatch =
    ScrollViewer.create
        [ ScrollViewer.content (
              StackPanel.create
                  [ StackPanel.children (
                        List.map
                            (fun e -> convertUIElementToIView e state.treeEnv state.funcEnv dispatch)
                            state.uiElements
                    ) ]
          ) ]
