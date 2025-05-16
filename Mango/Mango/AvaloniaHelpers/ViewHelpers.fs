module AvaloniaHelpers.ViewHelpers

let applyProp props applied tryExtract =
    props
    |> List.tryPick tryExtract
    |> Option.map (fun attr -> applied @ [attr])
    |> Option.defaultValue applied
