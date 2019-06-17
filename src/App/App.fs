module FractalWorkshop.App

open Elmish
open Elmish.Browser.Navigation
open Elmish.React

Program.mkProgram State.init State.update View.view
|> Program.withReact "FractalApp"
|> Program.run


