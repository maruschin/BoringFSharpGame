open Core

[<EntryPoint>]
let main argv =
    use g = new BoringGame()
    g.Run()
    0
