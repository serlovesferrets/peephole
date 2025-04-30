module Renderer.Draw

open State
open System
open System.Numerics
open Extensions.RaylibExts
open Raylib_cs

let toWinRatioX _state x =
    float x / float State.WinX * float Byte.MaxValue
    |> round
    |> fun x -> Math.Clamp(x, 0, int Byte.MaxValue)
    |> int

let toWinRatioY _state y =
    float y / float State.WinY * float Byte.MaxValue
    |> round
    |> fun y -> Math.Clamp(y, 0, int Byte.MaxValue)
    |> int

let toDrawOn =
    [ 0 .. State.WinX ]
    |> List.allPairs [ 0 .. State.WinY ]
    |> List.filter (fun (y, x) -> (y + 10) % 20 = 0 || (x + 10) % 20 = 0)

let drawSquareCursor () =
    let mX, mY = Rl.GetMousePosition'()
    let radius = 2

    [ mY - radius .. mY + radius ]
    |> List.allPairs [ mX - radius .. mX + radius ]
    |> List.iter (fun (x, y) -> Rl.DrawPixel(x, y, Color.LightGray))

let drawPoint (x, y) =
    [ y - 1 .. y + 1 ]
    |> List.allPairs [ x - 1 .. x + 1 ]
    |> List.iter (fun (x, y) -> Rl.DrawPixel(x, y, Color.Violet'))

let ortho_project state (vec3: Vector3) =
    Vector2(vec3.X / vec3.Z * state.FOV, vec3.Y / vec3.Z * state.FOV)

let showCameraCoord (x, y) =
    function
    | Coordinate.None -> Rl.DrawText("None", x, y, 32, Color.LightGray)
    | Coordinate.X -> Rl.DrawText("   X", x, y, 32, Color.Red')
    | Coordinate.Y -> Rl.DrawText("   Y", x, y, 32, Color.Green')
    | Coordinate.Z -> Rl.DrawText("   Z", x, y, 32, Color.Blue')

let draw (state: State) =
    Rl.ClearBackground Color.Black

    toDrawOn |> List.iter (fun (y, x) -> Rl.DrawPixel(x, y, Color.GridGray))

    state.Cube
    |> List.map (fun vec ->
        Vector3(
            Vector2(vec.X + state.CameraPos.X, vec.Y + state.CameraPos.Y),
            vec.Z + state.CameraPos.Z
        ))
    |> List.map (ortho_project state)
    |> List.iter (fun vec ->
        drawPoint (int vec.X + State.WinX / 2, int vec.Y + State.WinY / 2))

    showCameraCoord (800, 20) state.SelectedCamCoord

    drawSquareCursor ()

    state
