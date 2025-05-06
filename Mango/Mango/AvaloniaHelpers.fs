module AvaloniaHelpers
open Avalonia.FuncUI.Hosts
open Avalonia.Controls

let setWindowIcon (icon: string option) (window: HostWindow) =
    match icon with
    | Some filename -> 
        window.Icon <- new WindowIcon(filename)
        window
    | None -> window

let setWindowWidthAndHeight width height (window: HostWindow) =
    match width, height with
    | Some w, Some h ->
        window.Width <- float w
        window.Height <- float h
        window
    | _ -> window

let setWindowName name (window: HostWindow) = 
    window.Name <- name
    window

let setWindowProperties (window: HostWindow) name width height icon =
    window |>
    setWindowName name |>
    setWindowWidthAndHeight width height |>
    setWindowIcon icon