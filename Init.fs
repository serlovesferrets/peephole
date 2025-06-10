module Renderer.Init

open System.IO
open State
open Renderer.Shapes
open Extensions.RaylibExts

let init: State -> State =
    fun state ->
        let modelsDirName = "./models"

        let mutable dirFound, modelsFound = false, false
        let dirs = Directory.GetDirectories "."
        let mutable models: array<string> = [||]

        if Array.contains modelsDirName dirs then
            dirFound <- true

            let files =
                Directory.GetFiles modelsDirName
                |> Array.filter (fun f ->
                    Path.GetExtension $"{modelsDirName}/{f}" = ".obj")

            models <- files

            if not (Array.isEmpty files) then
                modelsFound <- true

        let points, edges =
            if modelsFound then readModel models[0] else [||], [||]

        let model = centerModel points

        Rl.InitWindow(State.WinX, State.WinY, state.Title)
        Rl.SetTargetFPS 60
        Rl.HideCursor()

        { state with
            Models = models
            ModelIndex = if modelsFound then 0 else -1
            ModelName = if modelsFound then Path.GetFileName models[0] else ""
            Points = model
            Edges = edges
            RtFlags.DirFound = dirFound
            RtFlags.ModelsFound = modelsFound }
