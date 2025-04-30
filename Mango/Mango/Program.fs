module Program

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open System.IO
open Parser
open System
open FSharp.Text.Lexing
open System.Text

exception SyntaxError of int * int

let printPos (errString : string) : unit =
    let rec state3 (s : string) (p : int) (lin : string) (col : int) =
        (* read digits until not *)
        let c = s.[p]
        if System.Char.IsDigit c
        then state3 s (p-1) (System.Char.ToString c + lin) col
        else raise (SyntaxError (System.Int32.Parse lin, col))

    let rec state2 (s : string) (p : int) (col : string) =
        (* skip from position until digit *)
        let c = s.[p]
        if System.Char.IsDigit c
        then state3 s (p-1) (System.Char.ToString c) (System.Int32.Parse col)
        else state2 s (p-1) col

    let rec state1 (s : string) (p : int) (col : string) =
        (* read digits until not *)
        let c = s.[p]
        if System.Char.IsDigit c
        then state1 s (p-1) (System.Char.ToString c + col)
        else state2 s (p-1) col

    let rec state0 (s : string) (p : int) =
        (* skip from end until digit *)
        let c = s.[p]
        if System.Char.IsDigit c
        then state1 s (p-1) (System.Char.ToString c)
        else state0 s (p-1)

    state0 errString (String.length errString - 1)

// Parse program from string.
let parseString (s : string) : AbSyn.Prog =
    Parser.Prog Lexer.Token
    <| LexBuffer<_>.FromBytes (Encoding.UTF8.GetBytes s)

let parseFastoFile (filename : string) : AbSyn.Prog =
  let txt = try  // read text from file given as parameter with added extension
              let inStream = File.OpenText (filename + ".fo")
              let txt = inStream.ReadToEnd()
              inStream.Close()
              txt
            with  // or return empty string
              | ex -> ""
  if txt <> "" then // valid file content
    let program =
      try
        parseString txt
      with
        | Lexer.LexicalError (info,(line,col)) ->
            printfn "%s at line %d, position %d\n" info line col
            System.Environment.Exit 1
            []
        | ex ->
            if ex.Message = "parse error"
            then printPos Parser.ErrorContextDescriptor
            else printfn "%s" ex.Message
            System.Environment.Exit 1
            []
    program
  else failwith "Invalid file name or empty file"

module Main =

    let view () =
        Component(fun ctx ->
            let state = ctx.useState 0

            DockPanel.create [
                DockPanel.children [
                    Button.create [
                        Button.dock Dock.Bottom
                        Button.onClick (fun _ -> state.Set(state.Current - 1))
                        Button.content "-"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                    ]
                    Button.create [
                        Button.dock Dock.Bottom
                        Button.onClick (fun _ -> state.Set(state.Current + 1))
                        Button.content "+"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                    ]
                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.fontSize 48.0
                        TextBlock.verticalAlignment VerticalAlignment.Center
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                        TextBlock.text (string state.Current)
                    ]
                ]
            ]
        )

type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        base.Content <- Main.view ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)