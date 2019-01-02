module Animation

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

let FrameWidth = 32
let FrameHeight = 32

type Animation =
    {
        TextureStrip: Texture2D;
        FrameCount: int;
        CurrentFrame: int;
        CurrentTime: int;
        TimePerFrame: int;
    }

let CreateAnimation (texture:Texture2D) frameLength =
    let frameCount = texture.Width / FrameWidth
    {
        TextureStrip = texture;
        FrameCount = frameCount;
        CurrentFrame = 0;
        CurrentTime = 0;
        TimePerFrame = frameLength;
    }

let UpdateAnimation (gameTime:GameTime) animation =
    let time = animation.CurrentTime + (int gameTime.ElapsedGameTime.TotalMilliseconds)
    let newFrame =
        if time > animation.TimePerFrame then
            let newFrame' = animation.CurrentFrame + 1
            if newFrame' >= animation.FrameCount then 0
            else newFrame'
        else animation.CurrentFrame
    let counter =
        if time > animation.TimePerFrame then 0
        else time
    { animation with CurrentFrame = newFrame;  CurrentTime = counter; }

let DrawAnimation (sb:SpriteBatch) animation (position:Vector2) =
    let rect =
        Rectangle(animation.CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight)
    sb.Draw(animation.TextureStrip, position, Nullable(rect), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.9f)

let DrawTexture (sb:SpriteBatch) texture (position:Vector2) =
    sb.Draw(texture, position, Nullable(), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.1f)
