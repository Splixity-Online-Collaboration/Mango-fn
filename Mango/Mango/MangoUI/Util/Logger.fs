module MangoUI.Util.Logger

let mutable verbose = false

let info o =
    if verbose then
        do printfn "[INFO] %A" o

let warn o = do printfn "[WARN] %A" o
let error o = do printfn "[ERROR] %A" o
