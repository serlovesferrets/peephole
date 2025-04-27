module Renderer.Shapes

open System.Numerics

let cube size =
    let fsize = float32 size
    let cameraZ = 8

    let half = float32 size / 2f

    let res =
        [ 0..size ]
        |> List.allPairs [ 0..size ]
        |> List.allPairs [ 0..size ]
        |> List.map (fun (z, rest) -> z - cameraZ, rest)
        |> List.map (fun (z, (y, x)) -> float32 x, float32 y, float32 z)
        |> List.map (fun (x, y, z) ->
            Vector3((x - half) / fsize, (y - half) / fsize, z))

    printfn "[LOG] Cube rendered"
    res
