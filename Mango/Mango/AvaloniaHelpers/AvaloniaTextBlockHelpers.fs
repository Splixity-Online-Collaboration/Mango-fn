module AvaloniaHelpers.AvaloniaTextBlockHelpers
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Controls
open AbSyn
open Avalonia.Media

let applyProp props applied tryExtract =
    props
    |> List.tryPick tryExtract
    |> Option.map (fun attr -> applied @ [attr])
    |> Option.defaultValue applied

let applyColor props applied =
    applyProp props applied (function
        | Color (color, _) ->
            match color with 
            | ColorName (name, _) ->
                match name with
                    | Red -> Some (TextBlock.foreground "red")                     
                    | Blue -> Some (TextBlock.foreground "blue")
                    | Yellow -> Some (TextBlock.foreground "yellow")
                    | Pink -> Some (TextBlock.foreground "pink")
                    | Green -> Some (TextBlock.foreground "green")            
            | Hex((a,r,g,b), _) ->
                Some (TextBlock.foreground (SolidColorBrush (Color.FromArgb (a, r, g, b), 1.0)))
        | _ -> None)


let applyTextBlockProperties props : IAttr<TextBlock> list =
    []
    |> applyColor props

let createTextBlock (text: string) (props: TextBlockProp list) : IView = 
    TextBlock.create  ([ TextBlock.text text ] @ applyTextBlockProperties props)