module Renderer.Init

open State
open System
open System.IO
open System.Numerics
open Extensions.RaylibExts
open Extensions.Vector3Exts

let readModel path =
    let points, edges =
        path
        |> File.ReadAllLines
        |> Array.map (fun s -> s.Split ' ')
        |> Array.partition (fun s -> s[0] = "v")

    let splitOnSlash (str: String) = str.Split "/"

    let edges =
        edges
        |> Array.filter (fun s -> s[0] = "f")
        |> Array.map (
            Array.skip 1
            >> Array.map (splitOnSlash >> Array.head >> int >> (fun n -> n - 1))
        )

    let points =
        points
        |> Array.map (Array.skip 1 >> Array.map float32 >> Vector3.FromArr)

    points, edges

let centerModel (model: array<Vector3>) : array<Vector3> =
    let invSize = model |> Array.length |> float32 |> (fun s -> 1f / s)
    let mean = Vector3(invSize, invSize, invSize).Dot(Array.sum model)
    model |> Array.map (fun v -> v - mean)

let init: State -> State =
    fun state ->
        let points, edges = readModel $"models/{state.ModelFile}"
        let model = centerModel points

        Rl.InitWindow(State.WinX, State.WinY, state.Title)
        Rl.SetTargetFPS 60
        Rl.HideCursor()

        { state with
            Points = model
            Edges = edges }
