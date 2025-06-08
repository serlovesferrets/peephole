module Extensions.Vector3Exts

open System.Numerics

type Vector3 with
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
