module Interpreter
open Avalonia.FuncUI.Hosts
open AvaloniaHelpers.AvaloniaHelpers
open SymTab
open AbSyn

type FuncEnv = SymTab<FunctionT>

let rec interpret (window: HostWindow) program : HostWindow =
    match program with
    | AbSyn.Window (name,width, height, icon, elements, _, _) -> 
        window |>
        setWindowProperties name width height icon |>
        setWindowContent elements
    