module FractalWorkshop.State

open Elmish
open Fable.Import

open FractalWorkshop.Types

let renderCommand =
    let sub dispatch =
        Browser.window.requestAnimationFrame(fun _ -> dispatch RenderMsg) |> ignore
    Cmd.ofSub sub

let init _ =
    {
        Zoom = 0.314
        X = 0.0
        Y = 0.0
        Now = System.DateTime.Now
        Render = None
    }, renderCommand

let update msg model =
    match msg with

    | MoveUp -> { model with Y = model.Y - 0.1 / model.Zoom }, []
    | MoveDown -> { model with Y = model.Y + 0.1 / model.Zoom }, []
    | ZoomIn -> { model with Zoom = model.Zoom * 1.2 }, []

    | RenderMsg ->
        match model.Render with
        | None ->
            let holder = Browser.document.getElementById("Fractal")
            match holder with
            | null -> model, renderCommand
            | h ->
                let renderer, height = FractalRenderer.create h
                { model with Render = Some renderer }, renderCommand
        | Some render ->
            render model
            { model with Now = System.DateTime.Now }, renderCommand
