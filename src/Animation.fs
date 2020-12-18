module Animation

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open ActorDomain
open AnimationDomain

let FrameWidth = 72
let FrameHeight = 97

let CreateAnimation (texture: Texture2D) frameLength =
    let frameCount = texture.Width / FrameWidth
    {
        TextureStrip = texture;
        FrameCount = frameCount;
        CurrentFrame = 0;
        CurrentTime = 0;
        TimePerFrame = frameLength;
    }

let UpdateAnimation (gameTime: GameTime) animation =
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

let DrawAnimation (spriteBatch: SpriteBatch) animation (Position position) =
    let rect =
        Rectangle(animation.CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight)
    spriteBatch.Draw(animation.TextureStrip, position, Nullable(rect), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.9f)
    ()

let DrawTexture (spriteBatch:SpriteBatch) texture (Position position) =
    spriteBatch.Draw(texture, position, Nullable(Rectangle(0, 0, 72, 72)), Color.White, 0.0f, Vector2.One, Vector2.One, SpriteEffects.None, 0.5f)
    ()

let DrawBackground (spriteBatch:SpriteBatch) texture (Position position) =
    spriteBatch.Draw(texture, position, Nullable(Rectangle(0, 0, 256, 256)), Color.White, 0.0f, Vector2.One, Vector2.One, SpriteEffects.None, 0.1f)
    ()