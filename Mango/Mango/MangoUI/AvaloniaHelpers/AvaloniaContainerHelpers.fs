module MangoUI.AvaloniaHelpers.AvaloniaContainerHelpers

open Avalonia.Controls
open Avalonia.FuncUI.Builder
open AvaloniaCommonHelpers
open ColorConverter
open MangoUI.Core.AbSyn

let applyBackgroundColor props applied =
    applyProp props applied (function
        | ContainerProp.BackgroundColor (c, _) -> Some (AttrBuilder<'a>.CreateProperty(Panel.BackgroundProperty, fromColor c, ValueNone))
        | _ -> None)
        
let applyContainerProperties (props: ContainerProp list) =
    []
    |> applyBackgroundColor props