module Extensions.Vector3Exts

open System.Numerics

type Vector3 with
    static member FromArr(arr: array<float32>) = Vector3(arr[0], arr[1], arr[2])

    member inline self.RotX beta =
        Vector3(
            X = self.X,
            Y = self.Y * cos beta - self.Z * sin beta,
            Z = self.Y * sin beta + self.Z * cos beta
        )

    member inline self.RotY beta =
        Vector3(
            X = self.X * cos beta - self.Z * sin beta,
            Y = self.Y,
            Z = self.X * sin beta + self.Z * cos beta
        )

    member inline self.RotZ beta =
        Vector3(
            X = self.X * cos beta - self.Y * sin beta,
            Y = self.X * sin beta + self.Y * cos beta,
            Z = self.Z
        )

    member inline self.Dot(other: Vector3) =
        Vector3(self.X * other.X, self.Y * other.Y, self.Z * other.Z)
