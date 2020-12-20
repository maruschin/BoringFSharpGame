module AnimationDomain

open Microsoft.Xna.Framework.Graphics

type Texture = Texture of Texture2D

type Animation =
    {
        TextureStrip: Texture2D;
        FrameCount: int;
        CurrentFrame: int;
        CurrentTime: int;
        TimePerFrame: int;
    }