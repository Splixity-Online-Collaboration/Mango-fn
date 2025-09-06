module MangoUI.AvaloniaHelpers.ColorConverter

open Avalonia.Media
open MangoUI.Core.AbSyn

let fromPredefined =
    function
    | Blue -> Colors.Blue
    | Red -> Colors.Red
    | Yellow -> Colors.Yellow
    | Pink -> Colors.Pink
    | Green -> Colors.Green
    | Black -> Colors.Black
    | White -> Colors.White

let fromColor (color: ColorT) : IBrush =
    match color with
    | ColorName(name, _) -> SolidColorBrush(fromPredefined name) :> IBrush
    | Hex((r, g, b, a), _) -> SolidColorBrush(Color.FromArgb(a, r, g, b)) :> IBrush
