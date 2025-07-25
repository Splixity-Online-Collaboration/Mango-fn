module MangoUI.Core.Interpreter

open Avalonia.FuncUI.Hosts
open MangoUI.AvaloniaHelpers.AvaloniaHelpers
open MangoUI.Core.Types
open AbSyn
open MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers
open MangoUI

let rec storeElementsMarkedWithId (elements : UIElement list) (tab : TreeEnv) : UIElement list * TreeEnv =
    List.fold (fun (accElements, accTab) element ->
        match element with
        | Button(_, propsOpt, pos) ->
            let buttonId = getId (Option.defaultValue [] propsOpt)
            match buttonId with
            | Some id ->
                let tab' = SymTab.bind id element accTab
                (accElements @ [Identifier (id, pos)], tab')
            | None -> 
                (accElements @ [element], accTab)
        | TextBlock(_, commonPropsOpt, _, pos) ->
            let textId = getId (Option.defaultValue [] commonPropsOpt)
            match textId with
            | Some id ->
                let tab' = SymTab.bind id element accTab
                (accElements @ [Identifier (id, pos)], tab')
            | None -> 
                (accElements @ [element], accTab)
        | Row (commonPropsOpt, _, elements, pos) ->
            let rowId = getId (Option.defaultValue [] commonPropsOpt)
            let subElements, tab' = storeElementsMarkedWithId elements accTab
            match rowId with
            | Some id ->
                let tab'' = SymTab.bind id element tab'
                (accElements @ [Identifier (id, pos)], tab'')
            | None -> 
                (accElements @ subElements, tab')
        | Column (commonPropsOpt, _, elements, pos) ->
            let columnId = getId (Option.defaultValue [] commonPropsOpt)
            let subElements, tab' = storeElementsMarkedWithId elements accTab
            match columnId with
            | Some id ->
                let tab'' = SymTab.bind id element tab'
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
    