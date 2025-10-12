module MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers

open Avalonia.FuncUI.Types
open Avalonia.Controls
open Avalonia.FuncUI.Builder
open Avalonia
open MangoUI.Core.AbSyn
open Avalonia.Layout
open MangoUI.AvaloniaHelpers.ColorConverter
open MangoUI.Util

let getId props =
    props
    |> List.tryPick (function
        | Id(Some(id, _)) -> Some id
        | _ -> None)

// Thickness helper function
let createThickness (t: Thickness) =
    let thickness =
        match t with
        | Uniform x -> Thickness(float x)
        | Symmetric(x, y) -> Thickness(float x, float y, float x, float y)
        | Custom(l, t, r, b) -> Thickness(float l, float t, float r, float b)

    thickness

let applyCommonProps props =
    MonadTesting.attrsFor {
        for prop in props do
            match prop with
            | BackgroundColor(Some(c, _)) -> AttrBuilder<'a>.CreateProperty(Panel.BackgroundProperty, fromColor c, ValueNone)
            | Hidden(Some(b, _)) -> AttrBuilder<'a>.CreateProperty(Control.IsVisibleProperty, not b, ValueNone)
            | Height(Some(size, _)) -> 
                match size with
                | Pixels num -> AttrBuilder<'a>.CreateProperty(Control.HeightProperty, float num, ValueNone)
                | Fill -> AttrBuilder<'a>.CreateProperty(Control.VerticalAlignmentProperty, VerticalAlignment.Stretch, ValueNone)
                | Hug -> AttrBuilder<'a>.CreateProperty(Control.VerticalAlignmentProperty, VerticalAlignment.Top, ValueNone)      
            | Width(Some(size, _)) ->
                match size with
                | Pixels num -> AttrBuilder<'a>.CreateProperty(Control.WidthProperty, float num, ValueNone)
                | Fill -> AttrBuilder<'a>.CreateProperty(Control.HorizontalAlignmentProperty, HorizontalAlignment.Stretch, ValueNone)
                | Hug -> AttrBuilder<'a>.CreateProperty(Control.HorizontalAlignmentProperty, HorizontalAlignment.Left, ValueNone)
            | Margin(Some(m, _)) -> AttrBuilder<'a>.CreateProperty(Control.MarginProperty, createThickness m, ValueNone)
            | _ -> ()
    }
