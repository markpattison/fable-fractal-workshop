module FractalWorkshop.Types

type Msg =
    | MoveUp
    | MoveDown
    | ZoomIn
    | RenderMsg

type Model =
    {
        Zoom: float
        X: float
        Y: float
        Now: System.DateTime
        Render: (Model -> unit) option
    }
