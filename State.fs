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
      Cube: list<Vector3>
      CubeSize: int
      FOV: float32
      CameraPos: CameraPosition
      SelectedCamCoord: Coordinate }

    static member WinX = 900
    static member WinY = 600

    static member Default =
        { Title = "Renderer (DEBUG)"
          ShouldDisplayText = false
          Cube = []
          CubeSize = 5
          FOV = 600f
          CameraPos = CameraPosition.Default
          SelectedCamCoord = Coordinate.None }
