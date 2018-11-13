module Core

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type BoringGame () as this =
    inherit Game()

    let mutable Graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

    override this.Initialize() =
        do spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        do base.Initialize()
        // TODO: Add your initialization logic here
        ()

    override this.LoadContent() =
        // TODO: use this.Content to load your game content here
        ()

    override this.Update (gameTime) =
        // TODO: Add your update logic here
        ()

    override this.Draw (gameTime) =
        do this.GraphicsDevice.Clear Color.CornflowerBlue
        // TODO: Add your drawing code here
        ()

