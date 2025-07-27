module MangoUI.AvaloniaHelpers.AvaloniaBorderHelpers

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open MangoUI.AvaloniaHelpers.AvaloniaCommonHelpers
open MangoUI.Core.AbSyn
open Avalonia
open Avalonia.Controls

let createCornerRadius (t) = 
    let thickness =
        match t with 
        | Uniform x -> CornerRadius(float x)
        | Symmetric (x,y) -> CornerRadius(float x, float y, float x, float y)
        | Custom (l,t,r,b) -> CornerRadius(float l,float t, float r,float b)
    thickness

let applyCorner props applied =
    applyProp props applied (function
        |  Corner (m,_) -> Some(Border.cornerRadius(createCornerRadius m))
    )

let applyBorderProperties (props: BorderProp list) =
    []
    |> applyCorner props