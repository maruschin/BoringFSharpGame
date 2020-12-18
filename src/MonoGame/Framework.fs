module Framework

open Microsoft.Xna.Framework

type XnaVector2 = Microsoft.Xna.Framework.Vector2

type Vector2 =
    {
        X: float32;
        Y: float32;
    }
    member this.XnaVector2 () =
        XnaVector2(this.X, this.Y)
       
    static member (+) (v1 : Vector2, v2 : Vector2) =
        {X=v1.X + v2.X; Y=v1.Y + v2.Y}
