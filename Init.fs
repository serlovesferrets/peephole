module Renderer.Init

open State
open Extensions.RaylibExts

let init: State -> State =
    fun state ->
        Rl.InitWindow(State.WinX, State.WinY, state.Title)
        Rl.SetTargetFPS 60
        Rl.HideCursor()

        { state with
            Cube = Shapes.cube state.CubeSize }
