module Interpreter
open Avalonia.FuncUI.Hosts
open AvaloniaHelpers.AvaloniaHelpers
open Core.Types
open AbSyn
open AvaloniaHelpers.AvaloniaCommonHelpers

let rec storeElementsMarkedWithId (elements : UIElement list) (tab : TreeEnv) : UIElement list * TreeEnv =
    List.fold (fun (accElements, accTab) element ->
        match element with
        | Button(label, propsOpt, pos) ->
            let buttonId = getId (Option.defaultValue [] propsOpt)
            match buttonId with
            | Some id ->
                let tab' = SymTab.bind id element tab
                (accElements @ [Identifier (id, pos)], tab')
            | None -> 
                (accElements @ [element], tab)
        | TextBlock(label, commonPropsOpt, propsOpt, pos) ->
            let textId = getId (Option.defaultValue [] commonPropsOpt)
            match textId with
            | Some id ->
                let tab' = SymTab.bind id element tab
                (accElements @ [Identifier (id, pos)], tab')
            | None -> 
                (accElements @ [element], tab)
        | Row (commonPropsOpt, containerPropsOpt, elements, pos) ->
            let rowId = getId (Option.defaultValue [] commonPropsOpt)
            let subElements, tab' = storeElementsMarkedWithId elements accTab
            match rowId with
            | Some id ->
                let tab'' = SymTab.bind id (Row (commonPropsOpt, containerPropsOpt, subElements, pos)) tab'
                (accElements @ [Identifier (id, pos)], tab'')
            | None -> 
                (accElements @ subElements, tab')
        | Column (commonPropsOpt, containerPropsOpt, elements, pos) ->
            let columnId = getId (Option.defaultValue [] commonPropsOpt)
            let subElements, tab' = storeElementsMarkedWithId elements accTab
            match columnId with
            | Some id ->
                let tab'' = SymTab.bind id (Column (commonPropsOpt, containerPropsOpt, subElements, pos)) tab'
                (accElements @ [Identifier (id, pos)], tab'')
            | None -> 
                (accElements @ subElements, tab')
        | _ -> 
            (accElements @ [element], accTab)
    ) ([], tab) elements

let rec interpret (window: HostWindow) program (tab : TreeEnv) : HostWindow =
    match program with
    | AbSyn.Window (name,width, height, icon, elements, _, _) ->
        do printfn "elements: %A" elements
        do printfn "tab: %A" tab
        let elements', tab' = storeElementsMarkedWithId elements tab
        do printfn "elements': %A" elements'
        do printfn "tab': %A" tab'
        window |>
        setWindowProperties name width height icon |>
        setWindowContent elements' tab'
    