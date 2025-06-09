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

    static member Default = { X = 0f; Y = 0f; Z = -2.5f }

[<Struct>]
type RuntimeFlags = { DebugPoints: bool }

[<Struct>]
type State =
    { Title: string
      ModelFile: string
      Points: array<Vector3>
      Edges: array<array<int>>
      Size: int
      Rotation: Vector3
      FOV: float32
      CameraPos: CameraPosition
      SelectedCamCoord: Coordinate
      RtFlags: RuntimeFlags }

    static member WinX = 1200
    static member WinY = 750

    static member Default =
        { Title = "Renderer (DEBUG)"
          ModelFile = "wolp.obj"
          Points = [||]
          Edges = [||]
          Size = 3
          Rotation = Vector3()
          FOV = 600f
          CameraPos = CameraPosition.Default
          SelectedCamCoord = Coordinate.None
          RtFlags = { DebugPoints = false } }
