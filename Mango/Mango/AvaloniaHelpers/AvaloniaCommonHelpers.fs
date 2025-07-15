module AvaloniaCommonHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open Avalonia.FuncUI.Builder
open AbSyn

open Avalonia.FuncUI.Types
open Avalonia.Controls
open Avalonia.FuncUI.Builder

let applyProp props applied tryExtract =
    props
    |> List.tryPick tryExtract
    |> Option.map (fun attr -> applied @ [attr])
    |> Option.defaultValue applied

let applyIsVisible<'t when 't :> Control> (value: bool) (acc: IAttr<'t> list) : IAttr<'t> =
    AttrBuilder<'t>.CreateProperty(Avalonia.Controls.Control.IsVisibleProperty, value, ValueNone)

let applyIsVisibleTest<'a when 'a :> Control> (props: CommonProp list) (applied: IAttr<'a> list) : IAttr<'a> list =
    applyProp props applied (function
        | IsVisible (b, _) -> Some (AttrBuilder<'a>.CreateProperty(Avalonia.Controls.Control.IsVisibleProperty, b, ValueNone)))

let applyCommonProps props = 
    []
    |> applyIsVisible props
