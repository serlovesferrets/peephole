module Extensions.RaylibExts

#nowarn "3391"

open Raylib_cs

type Raylib with
    static member WindowShouldClose'() = Raylib.WindowShouldClose(): bool

    static member IsMouseButtonPressed' mb =
        Raylib.IsMouseButtonPressed mb: bool

    static member GetMousePosition'() =
        Raylib.GetMousePosition() |> fun v -> int v.X, int v.Y

    static member IsKeyPressed' kk = Raylib.IsKeyPressed kk: bool
    static member IsKeyDown' kk = Raylib.IsKeyDown kk: bool

type Color with
    static member GridGray = Color(30, 30, 30)
    static member Violet' = Color(133, 122, 240)

    static member Red' = Color(237, 69, 57)
    static member Green' = Color(57, 227, 82)
    static member Blue' = Color(66, 84, 245)

type Rl = Raylib
