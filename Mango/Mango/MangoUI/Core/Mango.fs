namespace MangoUI.Core

open MangoUI

module Globals =
    let filepath = ref None

module Evaluator =
    open MangoUI.Core.Types
    open MangoUI.Core.AbSyn
    open MangoUI.Core.Interpreter
    open MangoUI.SymTab
    open MangoUI.Util.Logger
    open MangoUI.Core.PropertyValidator
    open MangoUI.Core.UIElementHelpers
    open MangoUI.AvaloniaHelpers.AvaloniaHelpers

    let evaluateStatement (stmt: Stmt) (state: AppState) : AppState =
        match stmt with
        | Set(prop, id, exp, _) ->
            match lookup id state.treeEnv with
            | Some element ->
                let currentProps = getProperties element
                let newProp = createProp (propertyKind prop) exp
                let updatedProps = upsertProperty newProp currentProps
                let updatedElement = insertProperties element updatedProps
                let treeEnv' = remove id state.treeEnv |> bind id updatedElement
                { state with treeEnv = treeEnv' }
            | None ->
                info (sprintf "Element %A not found" id)
                state
        | Update(id, props, _) ->
            match lookup id state.treeEnv with
            | Some element ->
                let currentProps = getProperties element

                let updatedProps =
                    List.fold (fun acc prop -> upsertProperty prop acc) currentProps props

                let updatedElement = insertProperties element updatedProps
                let treeEnv' = remove id state.treeEnv |> bind id updatedElement
                { state with treeEnv = treeEnv' }
            | None ->
                info (sprintf "Element %A not found" id)
                state
        | _ -> state

    let evaluateStatements (stmts: Stmt list) state : AppState =
        // let mutable temp = state
        // for i in stmts do
        //   temp <- evaluateStatement stmts[i] temp
        // return temp
        List.fold (fun acc stmt -> evaluateStatement stmt acc) state stmts

    let evaluateFunction (handle: string) state : AppState =
        match lookup handle state.funcEnv with
        | Some body -> evaluateStatements body state
        | None -> failwithf "Function %s not found" handle

    let init window () =
        match window with
        | Window(_, _, _, _, elements, funcs, _) ->
            let funcEnv' = initFuncEnv funcs
            let elements', treeEnv' = storeElementsMarkedWithId elements (empty ())

            { treeEnv = treeEnv'
              funcEnv = funcEnv'
              uiElements = elements' }

    let update (msg: Msg) (state: AppState) : AppState =
        info (sprintf "Updating state with message %A" msg)

        match msg with
        | UpdateFuncEnv(_, _, _) -> { state with funcEnv = state.funcEnv }
        | UpdateTreeEnv(_, _, _) -> { state with treeEnv = state.treeEnv }
        | UpdateUIElements newElements -> { state with uiElements = newElements }
        | EvalFunc funcName ->
            info (sprintf "Evaluating function %A" funcName)
            evaluateFunction funcName state
        | EvalLambda stmts ->
            info (sprintf "Evaluating lambda %A" stmts)
            evaluateStatements stmts state

    let view (state: AppState) dispatch =
        convertFromAbSynToAvaloniaTree state dispatch

module AppMain =

    open Avalonia
    open Avalonia.Controls.ApplicationLifetimes
    open Avalonia.FuncUI.Hosts
    open Avalonia.Themes.Fluent
    open Avalonia.FuncUI.Elmish
    open Elmish
    open Avalonia.Controls

    type MainWindow() as this =
        inherit HostWindow()

        do
            let source =
                Frontend.FileIO.readContent Globals.filepath.Value.Value
                |> Result.defaultValue ""

            let parseRes = Frontend.ParserWrapper.parseString source

            match parseRes with
            | Ok window ->
                match window with
                | AbSyn.Window(title, Some width, Some height, Some filepath, _, _, _) ->
                    base.Title <- title
                    base.Width <- width
                    base.Height <- height
                    base.Icon <- WindowIcon filepath
                | AbSyn.Window(title, Some width, Some height, None, _, _, _) ->
                    base.Title <- title
                    base.Width <- width
                    base.Height <- height
                | AbSyn.Window(title, None, None, None, _, _, _) ->
                    base.Title <- title
                    base.Width <- 800
                    base.Height <- 600
                | _ -> failwith "Window should not be able to have other combinations"

                Program.mkSimple (Evaluator.init window) Evaluator.update Evaluator.view
                |> Program.withHost this
                |> Program.withConsoleTrace
                |> Program.run
            | Error msg -> failwith msg

    type App() =
        inherit Application()

        override this.Initialize() =
            this.Styles.Add(FluentTheme())
            this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

        override this.OnFrameworkInitializationCompleted() =
            match this.ApplicationLifetime with
            | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
                let mainWindow = MainWindow()
                desktopLifetime.MainWindow <- mainWindow
            | _ -> ()

module Program =

    open Avalonia
    open AppMain

    [<EntryPoint>]
    let main (args: string[]) =
        let path = if args.Length > 0 then args[0] else "examples/window.mango"
        Globals.filepath.Value <- Some path

        let verboseFlag =
            if Array.contains "--verbose" args || Array.contains "-v" args then
                true
            else
                false

        if verboseFlag then
            Util.Logger.verbose <- true

        AppBuilder.Configure<App>().UsePlatformDetect().UseSkia().StartWithClassicDesktopLifetime args
