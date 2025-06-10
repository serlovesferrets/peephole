module Renderer.Loop

open State
open Shapes
open System
open System.IO
open System.Numerics
open Raylib_cs
open Extensions.RaylibExts

let private speed = 0.03f

let selectModelUnchecked: State -> int -> State =
    fun state index ->
        let points, edges = readModel state.Models[index]
        let points = centerModel points

        { state with
            ModelIndex = index
            ModelName = Path.GetFileName state.Models[index]
            Points = points
            Edges = edges }

[<TailCall>]
let rec loop state =
    state
    |> (fun state ->

        Rl.BeginDrawing()
        let state' = Draw.draw state
        Rl.EndDrawing()

        state')
    |> function
        | state when Rl.WindowShouldClose'() -> state

        | state when Rl.IsMouseButtonDown' MouseButton.Left ->
            loop
                { state with
                    Rotation =
                        state.Rotation * Vector3(1f, 1f, 0f)
                        + Vector3(Rl.GetMouseDelta(), state.Rotation.Z) }

        | state when Rl.IsMouseButtonDown' MouseButton.Right ->
            loop
                { state with
                    Rotation =
                        state.Rotation + Vector3(0f, 0f, Rl.GetMouseDelta().Y) }


        | state when Rl.IsKeyPressed' KeyboardKey.R ->
            loop { state with Rotation = Vector3() }

        | state when Rl.IsKeyPressed' KeyboardKey.F11 ->
            loop
                { state with
                    RtFlags.DebugPoints = not state.RtFlags.DebugPoints }


        | state when Rl.IsKeyPressed' KeyboardKey.Left ->
            if not state.RtFlags.ModelsFound || state.ModelIndex = 0 then
                loop state
            else
                loop
                    { selectModelUnchecked state (state.ModelIndex - 1) with
                        CameraPos = CameraPosition.Default
                        Rotation = Vector3() }

        | state when Rl.IsKeyPressed' KeyboardKey.Right ->
            if
                not state.RtFlags.ModelsFound
                || state.ModelIndex = Array.length state.Models - 1
            then
                loop state
            else
                loop
                    { selectModelUnchecked state (state.ModelIndex + 1) with
                        CameraPos = CameraPosition.Default
                        Rotation = Vector3() }

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
