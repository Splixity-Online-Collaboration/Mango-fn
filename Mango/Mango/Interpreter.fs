module Interpreter
open Avalonia.FuncUI.Hosts
open AvaloniaHelpers.AvaloniaHelpers

let rec interpret (window: HostWindow) program : HostWindow =
    match program with
    | AbSyn.Window (name,width, height, icon, elements, _) -> 
        window |>
        setWindowProperties name width height icon |>
        setWindowContent elements
    