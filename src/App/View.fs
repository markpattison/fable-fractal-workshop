module FractalWorkshop.View

open Fable.Helpers.React
open Fable.Helpers.React.Props

open FractalWorkshop.Types

let showParams model =
    div []
      [ p [] [ sprintf "X = %.6f" model.X |> str ]
        p [] [ sprintf "Y = %.6f" model.Y |> str ]
        p [] [ sprintf "Zoom = %.6f" model.Zoom |> str ]
      ]

let showButtons dispatch =
    div []
      [ div []
          [ button [ OnClick (fun _ -> MoveUp |> dispatch) ] [ str "Up" ]
            button [ OnClick (fun _ -> MoveDown |> dispatch) ] [ str "Down" ]
          ]
        div []
          [ button [ OnClick (fun _ -> ZoomIn |> dispatch) ] [ str "+" ]
          ]
      ]

let hud model dispatch =
    div [] [
        showParams model
        div [] [ showButtons dispatch ]
    ]

let fractalCanvas =
    div [ Id "Fractal" ] []

let view model dispatch =
    div [] [
        hud model dispatch
        fractalCanvas
    ]
