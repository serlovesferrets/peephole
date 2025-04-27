module Renderer.Deinit

open State
open Extensions.RaylibExts

let deinit: State -> unit = fun _ -> Rl.CloseWindow()
