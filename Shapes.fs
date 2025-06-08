module Renderer.Shapes

open System.Numerics

let cube size =
    let fsize = float32 size

    let half = float32 size / 2f

    let res =
        [ 0..size ]
        |> List.allPairs [ 0..size ]
        |> List.allPairs [ 0..size ]
        |> List.map (fun (z, (y, x)) -> float32 x, float32 y, float32 z)
        |> List.map (fun (x, y, z) ->
            Vector3((x - half) / fsize, (y - half) / fsize, (z - half) / fsize))

    printfn "[LOG] Cube rendered"
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

    printfn "[LOG] Plane rendered"
    res
