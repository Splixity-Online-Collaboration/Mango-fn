module MangoUI.Core.Interpreter

open Avalonia.FuncUI.Hosts
open MangoUI.AvaloniaHelpers.AvaloniaHelpers
open MangoUI.Core.Types
open AbSyn
open MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers
open MangoUI

let rec tryRegisterElement element commonProps pos childElements elements tab =
    let buttonId = getId commonProps
    match buttonId with
        | Some id ->
            let tab' = SymTab.bind id element tab
            (elements @ [Identifier (id, pos)], tab')
        | None -> 
            (elements @ [element], tab)

and storeElementsMarkedWithId (elements : UIElement list) (tab : TreeEnv) : UIElement list * TreeEnv =
    List.fold (fun (accElements, accTab) element ->
        match element with
        | Button(_, propsOpt, pos) -> tryRegisterElement element (Option.defaultValue [] propsOpt) pos None accElements accTab
        | TextBlock(_, commonPropsOpt, _, pos) -> tryRegisterElement element (Option.defaultValue [] commonPropsOpt) pos None accElements accTab
        | Row (commonPropsOpt, props, elements, pos) ->
            let rowId = getId (Option.defaultValue [] commonPropsOpt)
            let subElements, tab' = storeElementsMarkedWithId elements accTab
            match rowId with
            | Some id ->
                let tab'' = SymTab.bind id element tab'
                (accElements @ [Identifier (id, pos)], tab'')
            | None -> 
                (accElements @ [Row (commonPropsOpt, props, subElements, pos)], tab')
        | Column (commonPropsOpt, props, elements, pos) ->
            let columnId = getId (Option.defaultValue [] commonPropsOpt)
            let subElements, tab' = storeElementsMarkedWithId elements accTab
            match columnId with
            | Some id ->
                let tab'' = SymTab.bind id element tab'
                (accElements @ [Identifier (id, pos)], tab'')
            | None -> 
                (accElements @ [Column (commonPropsOpt, props, subElements, pos)], tab')
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
    