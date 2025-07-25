module Interpreter
open Avalonia.FuncUI.Hosts
open AvaloniaHelpers.AvaloniaHelpers
open Core.Types
open AbSyn
open AvaloniaHelpers.AvaloniaCommonHelpers

let rec temp (elements : UIElement list) (tab : TreeEnv) : UIElement list * TreeEnv =
    List.fold (fun (accElements, accTab) element ->
        match element with
        | Button(label, propsOpt, pos) ->
            if hasProp (Option.defaultValue [] propsOpt) (function
                | Id _ -> Some true
                | _ -> None) then
                let buttonId = getId (Option.defaultValue [] propsOpt)
                match buttonId with
                | Some id ->
                    let tab' = SymTab.bind id (Button (label, propsOpt, pos)) accTab
                    (accElements @ [Identifier (id, pos)], tab')
                | None -> failwith "Expected button to have an ID"
            else
                (accElements, accTab)
        | TextBlock(label, commonPropsOpt, propsOpt, pos) ->
            if hasProp (Option.defaultValue [] commonPropsOpt) (function
                | Id _ -> Some true
                | _ -> None) then
                let textBlockId = getId (Option.defaultValue [] commonPropsOpt)
                match textBlockId with
                | Some id ->
                    let tab' = SymTab.bind id (TextBlock (label, commonPropsOpt, propsOpt, pos)) accTab
                    (accElements @ [Identifier (id, pos)], tab')
                | None -> failwith "Expected TextBlock to have an ID"
            else
                (accElements, accTab)
        | _ -> 
            // Handle other UIElement types similarly
            (accElements, accTab)
    ) ([], tab) elements

let rec interpret (window: HostWindow) program (tab : TreeEnv) : HostWindow =
    match program with
    | AbSyn.Window (name,width, height, icon, elements, _, _) ->
        do printfn "elements: %A" elements
        do printfn "tab: %A" tab
        let elements', tab' = temp elements tab
        do printfn "elements': %A" elements'
        do printfn "tab': %A" tab'
        window |>
        setWindowProperties name width height icon |>
        setWindowContent elements' tab'
    