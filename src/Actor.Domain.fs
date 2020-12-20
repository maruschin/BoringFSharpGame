module ActorDomain

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open AnimationDomain

type Direction =
    | Forward
    | Backward

type ActionType =
    | Idle
    | Moving of Direction
    | Jumping

type AnimationType =
    | Animated of Animation
    | NotAnimated of Texture2D

type ActorType =
    | Player of ActionType
    | Enemy of ActionType
    | Obstacle
    | Background

type Velocity =
    { X: float32
      Y: float32 }
    static member FromVector2(vector2: Vector2) = { X = vector2.X; Y = vector2.Y }
    static member Zero = { X = 0.f; Y = 0.f }
    member this.ToVector2 = Vector2(this.X, this.Y)

type BodyType =
    | Static
    | Dynamic of Velocity

type Size =
    { Width: int32
      Height: int32 }
    static member FromPoint(point: Point) = { Width = point.X; Height = point.Y }
    member this.ToPoint = Point(this.Width, this.Height)

type Location =
    { X: float32
      Y: float32 }
    static member FromVector2(vector2: Vector2) =
        { Location.X = vector2.X
          Location.Y = vector2.Y }

    static member FromPoint(point: Point) =
        let vector2 = point.ToVector2()
        { Location.X = vector2.X
          Location.Y = vector2.Y }

    member this.ToVector2 = Vector2(this.X, this.Y)
    member this.ToPoint = this.ToVector2.ToPoint()


type Destination = Destination of Rectangle

type Bound =
    { Location: Location
      Size: Size }
    member this.GetRectangle =
        Rectangle(this.Location.ToPoint, this.Size.ToPoint)

type TextureName = TextureName of string

type Actor =
    { Type: ActorType
      Bound: Bound
      Animation: AnimationType
      Body: BodyType }
    member this.CurrentBounds = this.Bound.GetRectangle

    member this.DesiredBounds =
        match this.Body with
        | Static -> this.CurrentBounds
        | Dynamic velocity ->
            let newLocation =
                Location.FromVector2(this.Bound.Location.ToVector2 + velocity.ToVector2)

            let newBound =
                { this.Bound with
                      Location = newLocation }

            newBound.GetRectangle
