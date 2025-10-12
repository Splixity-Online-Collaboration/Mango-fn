module MangoUI.AvaloniaHelpers.AvaloniaHelpers

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
open MangoUI.Util.MonadTesting

let doesWrapExist props =
    props
    |> List.exists (fun prop ->
        match prop with
        | Wrap(Some(true, _)) -> true
        | _ -> false)

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
    | Button(propsOpt, _) -> createButton (Option.defaultValue [] propsOpt) dispatch
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
        attrsFor {
            yield WrapPanel.orientation orientation
            yield WrapPanel.children (ui {
                for e in elements -> convertUIElementToIView e tab funcEnv dispatch
            })
            yield! applyCommonProps props
        }
    )

and createStackPanel orientation elements props tab funcEnv dispatch =
    StackPanel.create (
        attrsFor {
            yield StackPanel.orientation orientation
            yield StackPanel.children (ui {
                for e in elements do
                    yield convertUIElementToIView e tab funcEnv dispatch
            })
            yield! applyCommonProps props
        }
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
        attrsFor {
            yield Border.child (convertUIElementToIView element tab funcEnv dispatch)
            yield! applyBorderProperties props
            yield! applyCommonProps props
        }
    )

let createScrollViewerWithContent (content: IView) = 
    ScrollViewer.create [ 
        ScrollViewer.content content
    ]

let createStackPanelWithContent (content: IView list) =
    StackPanel.create [
        StackPanel.children content
    ]

let convertFromAbSynToAvaloniaTree (state: AppState) dispatch =
    createScrollViewerWithContent (
        createStackPanelWithContent (ui {
            for e in state.uiElements do
            yield convertUIElementToIView e state.treeEnv state.funcEnv dispatch
        })
    )
