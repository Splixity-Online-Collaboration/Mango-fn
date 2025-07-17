module AvaloniaHelpers.AvaloniaContainerHelpers

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open AbSyn
open Avalonia.Layout
open Avalonia
open ViewHelpers
open AvaloniaCommonHelpers

let applyOrientation props applied =
    applyProp props applied (function
        | Layout (layout, _) ->
            match layout with
            | Horizontal -> Some (StackPanel.orientation Orientation.Horizontal)
            | Vertical -> Some (StackPanel.orientation Orientation.Vertical)
        | _ -> None)

let applyContainerProperties (props : ContainerProp list) =
    []
    |> applyOrientation props

let applyContainerCommonProperties (props : CommonProp list) =
    []
    |> applyMargin<StackPanel> props