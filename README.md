# fable-fractal-workshop

Clone this repo from <https://github.com/markpattison/fable-fractal-workshop>

This example is based on the [Fable 2.1 Minimal App](https://github.com/fable-compiler/fable2-samples/tree/master/minimal).

## Requirements

* [dotnet SDK](https://www.microsoft.com/net/download/core) 2.1 or higher
* [node.js](https://nodejs.org) with [npm](https://www.npmjs.com/)
* An F# editor like Visual Studio, Visual Studio Code with [Ionide](http://ionide.io/) or [JetBrains Rider](https://www.jetbrains.com/rider/).

## Building and running the app

* Install JS dependencies: `npm install`
* Start Webpack dev server: `npx webpack-dev-server` or `npm start`
* After the first compilation is finished, in your browser open: http://localhost:8080/

Any modification you do to the F# code will be reflected in the web page after saving.

## Workshop instructions

### 1. Use our fractal shader

In the line beginning `let program =...` in `FractalRender.fs`, replace `simpleShader` with `fragmentShader`.

### 2. Add buttons to move left & right plus zoom out

You'll need to:

* add more cases in the `Msg` discriminated union in `Types.fs`
* handle the new cases in `State.fs`
* add the buttons in `View.fs`

### 3. Allow the seed value to vary

For this task you'll have to:

* add `SeedX` and `SeedY` values to the `Model` type and initialise them
* display the seed values in the view
* use the seed values in the `render` function in `FractalRender.fs`
* add and handle new message cases to change the seed's X and Y values
* add buttons to the view

### 4. Change the fractal function

Find where the `seed` value is used in the `pixelResult` method and replace it with `zOrig`.  Do you recognise this fractal?

Change it back afterwards!

### 5. Experiment with rendering the fractal

Try changing the fractal function or colour map, or varying the seed values passed to the `draw` message in line with `timeloop`.

Share your favourites on Twitter! `#fsharpfractal`
