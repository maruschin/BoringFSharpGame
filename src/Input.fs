module Input

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open ActorDomain

let HandleInput (kbState:KeyboardState) actor =
    let newSpeed velocity direction =
        let isLowVelocity = velocity - 0.1f < -1.f
        match direction with
        | Forward ->
            if isLowVelocity then 1.f
            else velocity + 0.3f
        | Backward ->
            if isLowVelocity then -1.f
            else velocity - 0.3f
    let rec handleKeys keys (currentVelocity: Velocity, state) =
        match keys with
        | [] -> currentVelocity
        | x::xs ->
            match x with
            | Keys.Left ->
                let newV = {Velocity.X=(newSpeed currentVelocity.X Backward); Y=currentVelocity.Y}
                handleKeys xs (newV, state)
            | Keys.Right ->
                let newV = {Velocity.X=(newSpeed currentVelocity.X Forward); Y=currentVelocity.Y}
                handleKeys xs (newV, state)
            | Keys.Space ->
                match state with
                | Idle ->
                    let newV = {Velocity.X=currentVelocity.X; Y=currentVelocity.Y - 3.f}
                    handleKeys xs (newV, Jumping)
                | Jumping -> handleKeys xs (currentVelocity, state)
                | _ -> handleKeys xs (currentVelocity, state)
            |_ -> handleKeys xs (currentVelocity, state)
    match actor.Type with
    | Player(s) ->
        let initialVelocity =
            match actor.Body with
            | Dynamic(velocity) -> velocity
            | _ -> Velocity.Zero
        let velocity =
            handleKeys (kbState.GetPressedKeys() |> Array.toList) (initialVelocity, s)
        { actor with Body = Dynamic(velocity); Type = Player(Jumping) }
    | _ -> actor

