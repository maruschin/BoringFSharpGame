module Core

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

open Actor

type BoringGame () as this =
    inherit Game()

    do this.Content.RootDirectory <- "Content"

    let mutable Graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

    let CreateActor' = CreateActor this.Content

    let mutable WorldObjects = lazy ([("player", Player(Nothing),
                                       Vector2(10.f,28.f), Vector2(32.f,32.f), false);
                                      ("obstacle", Obstacle,
                                       Vector2(10.f,60.f), Vector2(32.f,32.f), true);
                                      ("", Obstacle,
                                       Vector2(42.f, 60.f), Vector2(32.f,32.f), true);] 
                                     |> List.map CreateActor')

    let DrawActor (sb:SpriteBatch) actor =
        if actor.Texture.IsSome then
            do sb.Draw(actor.Texture.Value, actor.Position, Color.White)
        ()

    override this.Initialize() =
        do spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        do base.Initialize()
        // TODO: Add your initialization logic here
        ()

    override this.LoadContent() =
        do WorldObjects.Force () |> ignore
        ()

    override this.Update (gameTime) =
        // TODO: Add your update logic here
        ()

    override this.Draw (gameTime) =
        do this.GraphicsDevice.Clear Color.CornflowerBlue
        let DrawActor' = DrawActor spriteBatch
        do spriteBatch.Begin ()
        WorldObjects.Value |> List.iter DrawActor'
        do spriteBatch.End ()
        ()

