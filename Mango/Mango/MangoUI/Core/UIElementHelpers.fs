module MangoUI.Core.UIElementHelpers

open MangoUI.Core.AbSyn
open MangoUI.Util.Logger

let getProperties element =
    match element with
    | Button (Some props, _) -> props
    | TextBlock (Some props, _) -> props
    | Border (Some props, _, _) -> props
    | Row (Some props, _, _) -> props
    | Column (Some props, _, _) -> props
    | _ -> []

let insertProperties element newProps =
    info (sprintf "Inserting properties %A into element %A" newProps element)
    match element with
    | Button (_, pos) ->
        Button (Some newProps, pos)
    | TextBlock (_, pos) ->
        TextBlock (Some newProps, pos)
    | Border (_, element, pos) ->
        Border (Some newProps, element, pos)
    | Row (_, elements, pos) ->
        Row (Some newProps, elements, pos)
    | Column (_, elements, pos) ->
        Column (Some newProps, elements, pos)
    | _ -> element