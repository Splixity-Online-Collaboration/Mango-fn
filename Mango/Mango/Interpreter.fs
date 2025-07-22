module Interpreter
open Avalonia.FuncUI.Hosts
open AvaloniaHelpers.AvaloniaHelpers
open Types

let rec interpret (window: HostWindow) program (tab : TreeEnv) : HostWindow =
    match program with
    | AbSyn.Window (name,width, height, icon, elements, _, _) -> 
        window |>
        setWindowProperties name width height icon |>
        setWindowContent elements tab
    