module MangoUI.Core.Interpreter

open Avalonia.FuncUI.Hosts
open MangoUI.AvaloniaHelpers.AvaloniaHelpers
open MangoUI.Core.Types
open AbSyn
open MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers
open MangoUI
open MangoUI.Util.Logger

let rec tryRegisterElement element commonProps pos childElements elements tab =
    let elementId = getId commonProps
    match elementId with
        | Some id ->
            info (sprintf "Registering element: %A" id)
            let tab' = SymTab.bind id element tab
            elements @ [Identifier (id, pos)], tab'
        | None -> 
            elements @ [element], tab

and storeElementsMarkedWithId (elements : UIElement list) (tab : TreeEnv) : UIElement list * TreeEnv =
    List.fold (fun (accElements, accTab) element ->
        match element with
        | Button(propsOpt, pos) -> 
            if not (PropertyValidator.validateProperties element) then
                do printfn $"Failed to validate properties for element at row: {fst pos}, col: {snd pos}"
            tryRegisterElement element (Option.defaultValue [] propsOpt) pos None accElements accTab
        | TextBlock(props, pos) -> 
            if not (PropertyValidator.validateProperties element) then
                do printfn $"Failed to validate properties for element at row: {fst pos}, col: {snd pos}"
            tryRegisterElement element (Option.defaultValue [] props) pos None accElements accTab
        | Row (props, elements, pos) ->
            if not (PropertyValidator.validateProperties element) then
                do printfn $"Failed to validate properties for element at row: {fst pos}, col: {snd pos}"
            let rowId = getId (Option.defaultValue [] props)
            let subElements, tab' = storeElementsMarkedWithId elements accTab
            match rowId with
            | Some id ->
                let tab'' = SymTab.bind id element tab'
                accElements @ [Identifier (id, pos)], tab''
            | None -> 
                accElements @ [Row (props, subElements, pos)], tab'
        | Column (props, elements, pos) ->
            if not (PropertyValidator.validateProperties element) then
                do printfn $"Failed to validate properties for element at row: {fst pos}, col: {snd pos}"
            let columnId = getId (Option.defaultValue [] props)
            info (sprintf "Column elements: %A" elements)
            let subElements, tab' = storeElementsMarkedWithId elements accTab
            info (sprintf "Sub-elements for column: %A" subElements)
            match columnId with
            | Some id ->
                let tab'' = SymTab.bind id (Column (props, subElements, pos)) tab'
                accElements @ [Identifier (id, pos)], tab''
            | None -> 
                accElements @ [Column (props, subElements, pos)], tab'
        | _ -> 
            accElements @ [element], accTab
    ) ([], tab) elements

let initFuncEnv (funcs: FunctionT list) : FuncEnv =
    List.fold (fun acc func ->
        match func with
            | Function (name, body, _) -> SymTab.bind name body acc
    ) (SymTab.empty ()) funcs