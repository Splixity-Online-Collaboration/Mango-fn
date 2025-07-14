module Globals =
    let filepath = ref None

module FileIO =

    open System.IO

    let readContent path =
        try
            File.ReadAllText(path) |> Ok
        with ex -> Error $"Could not read file: {ex.Message}"

module ParserWrapper =

    open System.Text
    open FSharp.Text.Lexing

    let parseString (source: string) =
        try
            let lexbuf = LexBuffer<_>.FromBytes(Encoding.UTF8.GetBytes(source))
            Ok (Parser.Prog Lexer.Token lexbuf)
        with
        | Lexer.LexicalError (msg, (line, col)) ->
            Error $"Lexical error: {msg} at line {line}, column {col}"
        | ex -> Error $"Parser error: {ex.Message}"

module AppMain =

    open Avalonia
    open Avalonia.Controls.ApplicationLifetimes
    open Avalonia.FuncUI.Hosts
    open Avalonia.Themes.Fluent
    open FileIO
    open ParserWrapper
    open Interpreter

    let mutable filepath = "examples/window.mango"

    type App() =
        inherit Application()
        override this.Initialize() =
            this.Styles.Add(FluentTheme())

        override this.OnFrameworkInitializationCompleted() =
            match readContent filepath with
            | Error err -> failwithf "File error: %s" err
            | Ok source ->
                match parseString source with
                | Error msg -> failwithf "Parse error: %s" msg
                | Ok syntax_tree ->
                    match this.ApplicationLifetime with
                    | :? IClassicDesktopStyleApplicationLifetime as desktop ->
                        desktop.MainWindow <- interpret (HostWindow()) syntax_tree
                    | _ -> ()

module Program =

    open Avalonia
    open AppMain

    [<EntryPoint>]
    let main (args: string[]) =
        let path = if args.Length > 0 then args[0] else "examples/test.mango"
        Globals.filepath.Value <- Some path
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)