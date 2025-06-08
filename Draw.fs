module Renderer.Draw

open State
open System
open System.Numerics
open Extensions.RaylibExts
open Extensions.Vector3Exts
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

let showCameraCoord (textX, textY) state =
    let x, y, z =
        round (state.CameraPos.X * 100f),
        round (state.CameraPos.Y * 100f),
        round (state.CameraPos.Z * 100f)

    let text = $"{x}; {y}; {z} | "
    Rl.DrawText(text, textX, textY, 16, Color.LightGray)

    let space = Rl.MeasureText(text, 16)
    let textX = space + textX

    function
    | Coordinate.None -> Rl.DrawText("None", textX, textY, 16, Color.LightGray)
    | Coordinate.X -> Rl.DrawText("X", textX, textY, 16, Color.Red')
    | Coordinate.Y -> Rl.DrawText("Y", textX, textY, 16, Color.Green')
    | Coordinate.Z -> Rl.DrawText("Z", textX, textY, 16, Color.Blue')

let inline debugPoints (points: list<Vector3>) =
    let mutable offset = 0
    let mutable count = 0

    for point in points do
        offset <- offset + 22
        count <- count + 1

        let x, y, z =
            round (point.X * 1000f),
            round (point.Y * 1000f),
            round (point.Z * 1000f)

        Rl.DrawText(
            $"#{count}. X: {x} | Y: {y} | Z: {z}",
            15,
            5 + offset,
            16,
            Color.LightGray
        )

    points

let draw (state: State) =
    Rl.ClearBackground Color.Black

    toDrawOn |> List.iter (fun (y, x) -> Rl.DrawPixel(x, y, Color.GridGray))

    state.Points
    |> List.map _.RotX(state.CubeRot)
    |> List.map _.RotY(state.CubeRot)
    |> List.map _.RotZ(state.CubeRot)
    |> fun pts -> if state.ShouldDebugPoints then debugPoints pts else pts
    |> List.map (fun vec ->
        Vector3(
            X = vec.X + state.CameraPos.X,
            Y = vec.Y + state.CameraPos.Y,
            Z = vec.Z + state.CameraPos.Z
        ))
    |> List.filter (fun v -> v.Z < 0.0f)
    |> List.map (ortho_project state)
    |> List.iter (fun vec ->
        drawPoint (int vec.X + State.WinX / 2, int vec.Y + State.WinY / 2))

    // Bottom left
    showCameraCoord (20, State.WinY - 40) state state.SelectedCamCoord

    // Bottom right
    Rl.DrawFPS(1100, State.WinY - 40)

    drawSquareCursor ()

    state
