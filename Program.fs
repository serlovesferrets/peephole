open Renderer.State
open Renderer

[<EntryPoint>]
let main _ =
    State.Default |> Init.init |> Loop.loop |> Deinit.deinit
    0
