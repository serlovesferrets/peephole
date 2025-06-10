module Renderer.Shapes

open System
open System.IO
open System.Numerics
open Extensions.Vector3Exts

let cube size =
    let fsize = float32 size

    let half = float32 size / 2f

    let res =
        [| 0..size |]
        |> Array.allPairs [| 0..size |]
        |> Array.allPairs [| 0..size |]
        |> Array.map (fun (z, (y, x)) -> float32 x, float32 y, float32 z)
        |> Array.map (fun (x, y, z) ->
            Vector3((x - half) / fsize, (y - half) / fsize, (z - half) / fsize))

    printfn "[LOG] Cube loaded"
    res

// this is genuinely so sloppy, i'm speechless
let plane size =
    let fsize = float32 size

    let half = float32 size / 2f

    let res =
        [ 0..size ]
        |> List.allPairs [ 0..size ]
        |> List.allPairs [ 0..size ]
        |> List.map (fun (z, (y, x)) -> float32 x, float32 y, float32 z)
        |> List.filter (fun (_, _, z) -> z = 0f)
        |> List.map (fun (x, y, z) ->
            Vector3((x - half) / fsize, (y - half) / fsize, (z - half) / fsize))
        |> Array.ofList

    printfn "[LOG] Plane loaded"
    res

let something =
    [| Vector3(2f, -2f, -2f); Vector3(-2f, 2f, -2f); Vector3(-2f, 2f, 2f) |]

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

    printfn "[LOG] Model loaded"
    points, edges

let centerModel (model: array<Vector3>) : array<Vector3> =
    let invSize = model |> Array.length |> float32 |> (fun s -> 1f / s)
    let mean = Vector3(invSize, invSize, invSize).Dot(Array.sum model)
    model |> Array.map (fun v -> v - mean)
