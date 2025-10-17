module MangoUI.AvaloniaHelpers.AvaloniaBorderHelpers

open Avalonia.FuncUI.DSL
open ColorConverter
open MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers
open MangoUI.Core.AbSyn
open Avalonia
open Avalonia.Controls
open MangoUI.Util.MonadTesting

let createCornerRadius (t) =
    let thickness =
        match t with
        | Uniform x -> CornerRadius(float x)
        | Symmetric(x, y) -> CornerRadius(float x, float y, float x, float y)
        | Custom(l, t, r, b) -> CornerRadius(float l, float t, float r, float b)

    thickness

let applyBorderProperties (props: Property list) =
    attrsFor {
        for prop in props do
            match prop with
            | Corner (Some(m, _)) -> Border.cornerRadius (createCornerRadius m)
            | Color(Some(c, _)) -> Border.borderBrush (fromColor c)
            | Density(Some(t, _)) -> Border.borderThickness (createThickness t)
            | _ -> ()
    }