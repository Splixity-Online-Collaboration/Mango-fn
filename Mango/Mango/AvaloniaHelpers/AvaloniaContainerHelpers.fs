module AvaloniaHelpers.AvaloniaContainerHelpers

open Avalonia.Controls
open AbSyn
open Avalonia.FuncUI.Builder
open AvaloniaCommonHelpers
open ColorConverter

let applyBackgroundColor props applied =
    applyProp props applied (function
        | ContainerProp.BackgroundColor (c, _) -> Some (AttrBuilder<'a>.CreateProperty(Panel.BackgroundProperty, fromColor c, ValueNone))
        | _ -> None)

let applyContainerCommonProperties (props : CommonProp list) =
    []
    |> applyMargin<StackPanel> props

let applyContainerProperties (props: ContainerProp list) =
    []
    |> applyBackgroundColor props