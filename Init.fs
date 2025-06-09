module Renderer.Init

open State
open System.IO
open System.Numerics
open Extensions.RaylibExts
open Extensions.Vector3Exts

let readModel path =
    path
    |> File.ReadAllLines
    |> Array.map (fun s -> s.Split ' ')
    |> Array.filter (fun s -> s[0] = "v")
    |> Array.map (Array.skip 1 >> Array.map float32 >> Vector3.FromArr)

let centerModel (model: array<Vector3>) : array<Vector3> =
    let invSize = model |> Array.length |> float32 |> fun s -> 1f / s
    let mean = Vector3(invSize, invSize, invSize).Dot(Array.sum model)
    model |> Array.map (fun v -> v - mean)

let init: State -> State =
    fun state ->
        let model = (readModel >> centerModel) "models/wolp.obj"

        Rl.InitWindow(State.WinX, State.WinY, state.Title)
        Rl.SetTargetFPS 60
        Rl.HideCursor()

        { state with Points = model }
