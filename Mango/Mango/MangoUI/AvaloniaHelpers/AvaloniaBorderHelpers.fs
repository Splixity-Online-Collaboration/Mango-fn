module MangoUI.AvaloniaHelpers.AvaloniaBorderHelpers

open Avalonia.FuncUI.DSL
open ColorConverter
open MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers
open MangoUI.Core.AbSyn
open Avalonia
open Avalonia.Controls

let createCornerRadius (t) = 
    let thickness =
        match t with 
        | Thickness.Uniform x -> CornerRadius(float x)
        | Thickness.Symmetric (x,y) -> CornerRadius(float x, float y, float x, float y)
        | Thickness.Custom (l,t,r,b) -> CornerRadius(float l,float t, float r,float b)
    thickness

let applyCorner props applied =
    applyProp props applied (function
        |  Corner (m,_) -> Some(Border.cornerRadius(createCornerRadius m))     
        | _ -> None )

let applyColor props applied =
    applyProp props applied (function
        |  Color(c, _) -> Some(Border.borderBrush(fromColor c))
        | _ -> None )

let applyThickness props applied =
    applyProp props applied (function
        |   Density(t,_) -> Some(Border.borderThickness(createThickness t))
        | _ -> None )

let applyBorderProperties (props: Property list) =
    []
    |> applyCorner props
    |> applyColor props
    |> applyThickness props