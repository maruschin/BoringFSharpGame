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
    { TextureStrip = texture
      FrameCount = frameCount
      CurrentFrame = 0
      CurrentTime = 0
      TimePerFrame = frameLength }

let UpdateAnimation (gameTime: GameTime) animation =
    let time =
        animation.CurrentTime
        + (int gameTime.ElapsedGameTime.TotalMilliseconds)

    let newFrame =
        if time > animation.TimePerFrame then
            let newFrame' = animation.CurrentFrame + 1
            if newFrame' >= animation.FrameCount then 0 else newFrame'
        else
            animation.CurrentFrame

    let counter =
        if time > animation.TimePerFrame then 0 else time

    { animation with
          CurrentFrame = newFrame
          CurrentTime = counter }

let Draw (spriteBatch: SpriteBatch)
         (texture: Texture2D) // A texture.
         (destinationRectangle: Rectangle) // The drawing bounds on screen.
         (sourceRectangle: Rectangle) // An optional region on the texture which will be rendered. If null - draws full texture.
         (color: Color) // A color mask.
         (rotation: Single) // A rotation of this sprite.
         (origin: Vector2) // Center of the rotation. 0,0 by default.
         (effects: SpriteEffects) // Modificators for drawing. Can be combined.
         (layerDepth: Single)
         =
    spriteBatch.Draw
        (texture, destinationRectangle, Nullable(sourceRectangle), color, rotation, origin, effects, layerDepth)
    ()

let Draw2 (spriteBatch: SpriteBatch)
          (Texture texture) // A texture.
          (location: Location) // The drawing bounds on screen.
          (sourceRectangle: Rectangle) // An optional region on the texture which will be rendered. If null - draws  full texture.
          (color: Color) // A color mask.
          (rotation: Single) // A rotation of this sprite.
          (origin: Vector2) // Center of the rotation. 0,0 by default.
          (scale: Vector2) // or Single // A scaling of this sprite.
          (effects: SpriteEffects) // Modificators for drawing. Can be combined.
          (layerDepth: Single)
          =
    spriteBatch.Draw
        (texture, location.ToVector2, Nullable(sourceRectangle), color, rotation, origin, scale, effects, layerDepth)
    ()


let DrawAnimation (spriteBatch: SpriteBatch) animation (location: Location) =
    let rect =
        Rectangle(animation.CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight)

    spriteBatch.Draw
        (animation.TextureStrip,
         location.ToVector2,
         Nullable(rect),
         Color.White,
         0.0f,
         Vector2.Zero,
         Vector2.One,
         SpriteEffects.None,
         0.9f)
    ()

let DrawTexture (spriteBatch: SpriteBatch) texture (location: Location) =
    spriteBatch.Draw
        (texture,
         location.ToVector2,
         Nullable(Rectangle(0, 0, 72, 72)),
         Color.White,
         0.f,
         Vector2.One,
         Vector2.One,
         SpriteEffects.None,
         0.5f)
    ()

let DrawBackground (spriteBatch: SpriteBatch) texture (location: Location) =
    spriteBatch.Draw
        (texture,
         location.ToVector2,
         Nullable(Rectangle(0, 0, 256, 256)),
         Color.White,
         0.f,
         Vector2.One,
         Vector2.One,
         SpriteEffects.None,
         0.1f)
    ()
