module Renderer.Draw

open State
open System
open System.Numerics
open Extensions.RaylibExts
open Extensions.Vector3Exts
open Extensions.ArrayExts
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
    [| 0 .. State.WinX |]
    |> Array.allPairs [| 0 .. State.WinY |]
    |> Array.filter (fun (y, x) -> (y + 10) % 20 = 0 || (x + 10) % 20 = 0)

let inline drawSquareCursor () =
    let mX, mY = Rl.GetMousePosition'()
    let radius = 2

    let color =
        if Rl.IsMouseButtonDown' MouseButton.Left then
            Color.Orange'
        else if Rl.IsMouseButtonDown' MouseButton.Right then
            Color.LightGreen'
        else
            Color.LightGray

    [| mY - radius .. mY + radius |]
    |> Array.allPairs [| mX - radius .. mX + radius |]
    |> Array.iter (fun (x, y) -> Rl.DrawPixel(x, y, color))

let inline drawPoint (x, y) =
    [| y - 1 .. y + 1 |]
    |> Array.allPairs [| x - 1 .. x + 1 |]
    |> Array.iter (fun (x, y) -> Rl.DrawPixel(x, y, Color.Violet'))

let inline drawPointV (vec: Vector2) = drawPoint (int vec.X, int vec.Y)

let inline ddaLine (v1: Vector2, v2: Vector2) =
    let deltaX, deltaY = v2.X - v1.X, v2.Y - v1.Y
    let sideLen = max (abs deltaX) (abs deltaY)
    let xStep, yStep = deltaX / sideLen, deltaY / sideLen

    let mutable xc, yc = v1.X, v1.Y

    for _ in [| 0f .. sideLen |] do
        Rl.DrawPixel(int xc, int yc, Color.Violet')
        xc <- xc + xStep
        yc <- yc + yStep


let inline ortho_project state (vec3: Vector3) =
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

let inline debugPoints (points: array<Vector3>) =
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

let inline centerVecToWin (vec: Vector2) =
    Vector2(
        X = vec.X + float32 State.WinX / 2f,
        Y = vec.Y + float32 State.WinY / 2f
    )

let drawEdges: State -> array<Vector2> -> unit =
    fun state points ->
        for face in state.Edges do
            let faceLen = Array.length face

            for i in 0 .. (faceLen - 2) do
                ddaLine (points[face[i]], points[face[i + 1]])

            ddaLine (points[face[faceLen - 1]], points[face[0]])

let drawErrMsg state =
    if not (state.RtFlags.DirFound && state.RtFlags.ModelsFound) then
        let message =
            if not state.RtFlags.DirFound then
                "No \"models\" directory found!"
            else
                "No models found in the \"models\" directory!"

        let size = 48

        Rl.DrawText(
            message,
            (State.WinX - Rl.MeasureText(message, size)) / 2,
            (State.WinY - size) / 2,
            size,
            Color.Red'
        )


let draw (state: State) =
    // Grid
    Rl.ClearBackground Color.Black
    toDrawOn |> Array.iter (fun (y, x) -> Rl.DrawPixel(x, y, Color.GridGray))

    drawErrMsg state

    // Vertices
    let points =
        state.Points
        |> Array.map _.RotX(state.Rotation.Y / 200f)
        |> Array.map _.RotY(state.Rotation.X / 200f)
        |> Array.map _.RotZ(state.Rotation.Z / 200f)
        |> Array.tap (fun pts ->
            if state.RtFlags.DebugPoints then
                debugPoints pts)
        |> Array.map (fun vec ->
            Vector3(
                X = vec.X + state.CameraPos.X,
                Y = vec.Y + state.CameraPos.Y,
                Z = vec.Z + state.CameraPos.Z
            ))
        |> Array.filter (fun v -> v.Z < 0.0f)
        |> Array.map (ortho_project state >> centerVecToWin)

    // Edges
    drawEdges state points

    // Top right
    let fileNameTextSize = 24

    Rl.DrawText(
        state.ModelName,
        State.WinX - 30 - Rl.MeasureText(state.ModelName, fileNameTextSize),
        20,
        fileNameTextSize,
        Color.LightGray
    )

    // Bottom left
    Rl.DrawText($"FOV: {state.FOV}", 20, State.WinY - 65, 14, Color.LightGray)
    showCameraCoord (20, State.WinY - 40) state state.SelectedCamCoord

    // Bottom right
    Rl.DrawFPS(1100, State.WinY - 40)

    // Cursor
    drawSquareCursor ()

    // Loop
    state
