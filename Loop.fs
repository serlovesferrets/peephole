module Renderer.Loop

open State
open System
open Raylib_cs
open Extensions.RaylibExts

let private speed = 0.03f

[<TailCall>]
let rec loop state =
    state
    |> (fun state ->

        Rl.BeginDrawing()
        let state' = Draw.draw state
        Rl.EndDrawing()

        state')
    |> (fun state ->
        { state with
            CubeRot = state.CubeRot + 0.005f })
    |> function
        | state when Rl.WindowShouldClose'() -> state
        | state when Rl.IsKeyPressed' KeyboardKey.F12 ->
            loop
                { state with
                    ShouldDebugPoints = not state.ShouldDebugPoints }


        | state when Rl.IsKeyPressed' KeyboardKey.Left ->
            if state.CubeSize = 0 then
                loop state
            else
                let newState =
                    { state with
                        Points = Shapes.cube (state.CubeSize - 1)
                        CubeSize = state.CubeSize - 1 }

                loop newState

        | state when Rl.IsKeyPressed' KeyboardKey.Right ->
            let newState =
                { state with
                    Points = Shapes.cube (state.CubeSize + 1)
                    CubeSize = state.CubeSize + 1 }

            loop newState
        | state when Rl.IsKeyDown' KeyboardKey.J ->
            let newState =
                match state.SelectedCamCoord with
                | Coordinate.None -> state
                | Coordinate.X ->
                    { state with
                        CameraPos.X = state.CameraPos.X - speed }
                | Coordinate.Y ->
                    { state with
                        CameraPos.Y = state.CameraPos.Y - speed }
                | Coordinate.Z ->
                    { state with
                        CameraPos.Z = state.CameraPos.Z - speed }

            loop newState
        | state when Rl.IsKeyDown' KeyboardKey.K ->
            let newState =
                match state.SelectedCamCoord with
                | Coordinate.None -> state
                | Coordinate.X ->
                    { state with
                        CameraPos.X = state.CameraPos.X + speed }
                | Coordinate.Y ->
                    { state with
                        CameraPos.Y = state.CameraPos.Y + speed }
                | Coordinate.Z ->
                    { state with
                        CameraPos.Z = state.CameraPos.Z + speed }

            loop newState
        | state when Rl.IsKeyPressed' KeyboardKey.N ->
            let newState =
                { state with
                    SelectedCamCoord = Coordinate.None }

            loop newState
        | state when Rl.IsKeyPressed' KeyboardKey.X ->
            let newState =
                { state with
                    SelectedCamCoord = Coordinate.X }

            loop newState
        | state when Rl.IsKeyPressed' KeyboardKey.Y ->
            let newState =
                { state with
                    SelectedCamCoord = Coordinate.Y }

            loop newState
        | state when Rl.IsKeyPressed' KeyboardKey.Z ->
            let newState =
                { state with
                    SelectedCamCoord = Coordinate.Z }

            loop newState
        | state when Rl.IsKeyDown' KeyboardKey.Down ->
            loop
                { state with
                    FOV = Math.Clamp(state.FOV - 10f, 50.0f, Single.MaxValue) }
        | state when Rl.IsKeyDown' KeyboardKey.Up ->
            loop { state with FOV = state.FOV + 10f }

        | state -> loop state
