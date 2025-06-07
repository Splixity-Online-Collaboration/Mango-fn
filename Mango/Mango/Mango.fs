module Program

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open System.IO
open FSharp.Text.Lexing
open System.Text
open Interpreter

let mutable filepath = ""

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

/// <summary> Lexes and parses a mango file</summary>
/// <param name="s">string representing mango file</param>
/// <returns>Abstract Syntax tree or error message</returns>
let parseString (s : string) =
    try 
        Ok (Parser.Prog Lexer.Token <| LexBuffer<_>.FromBytes (Encoding.UTF8.GetBytes s))
    with 
        | Lexer.LexicalError (info,(line,col)) ->
            Error (sprintf "%s at line %d, position %d\n" info line col)
        | ex -> Error ex.Message

/// <summary>Read the file at the path given as parameter and return the result</summary>
/// <param name="path">Path to mango program</param>
/// <returns>Result containing string of the read file or IO error</returns>
let readContent path = 
    try // read text from file given as parameter with added extension
        let inStream = File.OpenText path
        let txt = inStream.ReadToEnd()
        inStream.Close()
        Ok txt
    with // or return the exception
        | ex -> Error ex

/// <summary>Open file and perform lexing and parsing on the mango file</summary>
/// <param name="filename">Path to mango file</param>
/// <returns> The abstract syntax tree or an error message</returns>
let parseMangoFile (filename : string) =
  let txt = readContent filename
  match txt with
  | Ok content -> parseString content
  | Error error -> Error error.Message

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())

    override this.OnFrameworkInitializationCompleted() =
        let absyn = parseMangoFile filepath

        match absyn with
        | Ok syntax_tree ->
            match this.ApplicationLifetime with
            | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
                desktopLifetime.MainWindow <- interpret (new HostWindow()) syntax_tree
            | _ -> ()
        | Error message -> failwith message

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        if args.Length = 0 then
            do filepath <- "examples/window.mango"
        else
            do filepath <- args[0]
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)