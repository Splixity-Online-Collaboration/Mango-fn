module AvaloniaHelpers.AvaloniaTextBlockHelpers
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types

let createTextBlock (text: string) : IView = 
    TextBlock.create [ ]