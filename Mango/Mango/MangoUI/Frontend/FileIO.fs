module MangoUI.Frontend.FileIO

open System.IO

let readContent path =
    try
        File.ReadAllText path |> Ok
    with ex -> Error $"Could not read file: {ex.Message}"