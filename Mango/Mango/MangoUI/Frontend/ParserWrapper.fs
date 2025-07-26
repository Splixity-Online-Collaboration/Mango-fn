module MangoUI.Frontend.ParserWrapper

open System.Text
open FSharp.Text.Lexing

let parseString (source: string) =
    try
        let lexbuf = LexBuffer<_>.FromBytes(Encoding.UTF8.GetBytes source)
        Ok (Parser.Prog Lexer.Token lexbuf)
    with
    | Lexer.LexicalError (msg, (line, col)) -> 
           Error $"Lexical error: {msg} at line {line}, column {col}"
    | ex -> Error $"Parser error: {ex.Message}"