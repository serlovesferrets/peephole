module Renderer.State

open System.Numerics

[<RequireQualifiedAccess>]
type Coordinate =
    | X
    | Y
    | Z
    | None

[<Struct>]
type CameraPosition =
    { X: float32
      Y: float32
      Z: float32 }

    static member Default = { X = 0f; Y = 0f; Z = 0f }

[<Struct>]
type State =
    { Title: string
      ShouldDisplayText: bool
      ShouldDebugPoints: bool
      Points: list<Vector3>
      CubeSize: int
      CubeRot: float32
      FOV: float32
      CameraPos: CameraPosition
      SelectedCamCoord: Coordinate }

    static member WinX = 1200
    static member WinY = 750

    static member Default =
        { Title = "Renderer (DEBUG)"
          ShouldDisplayText = false
          ShouldDebugPoints = false
          Points = []
          CubeSize = 3
          CubeRot = 0f
          FOV = 600f
          CameraPos = CameraPosition.Default
          SelectedCamCoord = Coordinate.None }
