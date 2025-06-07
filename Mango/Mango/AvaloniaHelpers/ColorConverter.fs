module AvaloniaHelpers.ColorConverter

open AbSyn
open Avalonia.Media

let fromPredefined = function
    | Blue -> Colors.Blue
    | Red -> Colors.Red
    | Yellow -> Colors.Yellow
    | Pink -> Colors.Pink
    | Green -> Colors.Green
    | Black -> Colors.Black
    | White -> Colors.White

let fromColor (color: AbSyn.ColorT) : IBrush =
    match color with
    | ColorName (name, _) ->
        SolidColorBrush(fromPredefined name) :> IBrush
    | Hex ((r, g, b, a), _) ->
        SolidColorBrush(Color.FromArgb(a, r, g, b)) :> IBrush
