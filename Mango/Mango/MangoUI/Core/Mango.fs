namespace MangoUI.Core

open MangoUI

module Globals =
    let filepath = ref None

module AppMain =

    open Avalonia
    open Avalonia.Controls.ApplicationLifetimes
    open Avalonia.FuncUI.Hosts
    open Avalonia.Themes.Fluent
    open MangoUI.Frontend.FileIO
    open MangoUI.Frontend.ParserWrapper
    open Interpreter

    type App() =
        inherit Application()
        override this.Initialize() =
            this.Styles.Add(FluentTheme())

        override this.OnFrameworkInitializationCompleted() =
            match readContent Globals.filepath.Value.Value with
            | Error err -> failwithf "File error: %s" err
            | Ok source ->
                match parseString source with
                | Error msg -> failwithf "Parse error: %s" msg
                | Ok syntax_tree ->
                    match this.ApplicationLifetime with
                    | :? IClassicDesktopStyleApplicationLifetime as desktop ->
                        desktop.MainWindow <- interpret (HostWindow()) syntax_tree (SymTab.empty ())
                    | _ -> ()

module Program =

    open Avalonia
    open AppMain

    [<EntryPoint>]
    let main (args: string[]) =
        let path = if args.Length > 0 then args[0] else "examples/idTest.mango"
        Globals.filepath.Value <- Some path
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)