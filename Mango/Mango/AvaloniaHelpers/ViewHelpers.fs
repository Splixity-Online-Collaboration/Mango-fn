module AvaloniaHelpers.ViewHelpers

open AbSyn
open Avalonia.Controls
open Avalonia.FuncUI.DSL

let applyProp props applied tryExtract =
    props
    |> List.tryPick tryExtract
    |> Option.map (fun attr -> applied @ [ attr ])
    |> Option.defaultValue applied
