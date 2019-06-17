module FractalWorkshop.FractalRenderer

open System
open Fable.Import

open FractalWorkshop.Types
open FractalWorkshop.WebGLHelper

let vertexShader = """
    precision highp float;
    precision highp int;
    
    attribute vec4 aVertexPosition;
    attribute vec2 aTextureCoord;

    varying vec2 vTextureCoord;

    void main()
    {
      gl_Position = aVertexPosition;
      vTextureCoord = aTextureCoord;
    }
"""

let simpleShader = """
    precision highp float;
    precision highp int;

    uniform float uWidthOverHeight;
    uniform float uZoom;
    uniform vec2 uOffset, uJuliaSeed;

    varying vec2 vTextureCoord;


    void main(void)
    {
        gl_FragColor = vec4(vTextureCoord.x, vTextureCoord.y, 0.0, 1.0);
    }
"""

let fragmentShader = """
    precision highp float;
    precision highp int;

    uniform float uWidthOverHeight;
    uniform float uZoom;
    uniform vec2 uOffset, uJuliaSeed;

    varying vec2 vTextureCoord;

    vec2 calculatePosition(vec2 inputCoords, float zoom, float widthOverHeight, vec2 offset)
    {
        // calculates the position in fractal-space from the position in screen-space

        return (inputCoords - 0.5) * vec2(widthOverHeight, 1.0) / zoom + offset;
    }

    vec4 applyColourMap(float x)
    {
        // applies a nice colour gradient

        return vec4(sin(x * 4.0), sin (x * 5.0), sin (x * 6.0), 1.0);
    }

    vec2 complexMultiply(vec2 a, vec2 b)
    {
        return vec2(a.x * b.x - a.y * b.y, a.x * b.y + a.y * b.x);
    }

    vec2 complexSquare(vec2 z)
    {
        return complexMultiply(z, z);
    }

    vec2 complexCube(vec2 z)
    {
        return complexMultiply(complexSquare(z), z);
    }

    float pixelResult(vec2 zOrig, vec2 seed)
    {
        float result = 0.0;
        vec2 z = zOrig;
        vec2 zsq = z * z;

        int iterations = 0;

        for (int i = 0; i < 128; i++)
        {
            iterations = i;
            zsq = z * z;

            if (zsq.x + zsq.y > 49.0)
            {
                break;
            }

            // the fractal function

            z = complexSquare(z) + seed;
        }

        if (iterations == 127)
        {
            // in this case our pixel is within the set, so render it black

            result = 0.0;
        }
        else
        {
            // these calculations are just used to give a smooth gradient outside of the set

            result = float(iterations) + (log(2.0 * log(7.0)) - log(log(zsq.x + zsq.y))) / log(2.0);
            result = log(result * 0.4) / log(128.0);
        }

        return result;
    }

    void main(void)
    {
        vec2 z = calculatePosition(vTextureCoord, uZoom, uWidthOverHeight, uOffset);
        float result = pixelResult(z, uJuliaSeed);
        gl_FragColor = applyColourMap(result);
    }
"""

let initBuffers gl =
    let positions =
        createBuffer
            [|
                 -1.0; -1.0;
                  1.0; -1.0;
                 -1.0;  1.0;
                  1.0;  1.0
            |] gl
    let textureCoords =
        createBuffer
            [|
                0.0; 0.0;
                1.0; 0.0;
                0.0; 1.0;
                1.0; 1.0
            |] gl
    positions, textureCoords

let create (holder : Browser.Element) =

    let canvas = Browser.document.createElement_canvas()
    let width = 640
    let height = 480

    canvas.width <- float width
    canvas.height <- float height
    canvas.style.width <- width.ToString() + "px"
    canvas.style.height <- height.ToString() + "px"

    holder.appendChild(canvas) |> ignore

    let context = getWebGLContext canvas

    let program = createShaderProgram context vertexShader simpleShader

    let positionBuffer, colourBuffer = initBuffers context
    let vertexPositionAttribute = createAttributeLocation context program "aVertexPosition"
    let textureCoordAttribute = createAttributeLocation context program "aTextureCoord"
    let widthOverHeightUniform = createUniformLocation context program "uWidthOverHeight"
    let zoomUniform = createUniformLocation context program "uZoom"
    let offsetUniform = createUniformLocation context program "uOffset"
    let juliaSeedUniform = createUniformLocation context program "uJuliaSeed"

    let draw widthOverHeight zoom x y jx jy =
        context.useProgram(program)

        context.bindBuffer(context.ARRAY_BUFFER, positionBuffer)
        context.vertexAttribPointer(vertexPositionAttribute, 2.0, context.FLOAT, false, 0.0, 0.0)
        context.bindBuffer(context.ARRAY_BUFFER, colourBuffer)
        context.vertexAttribPointer(textureCoordAttribute, 2.0, context.FLOAT, false, 0.0, 0.0)

        context.uniform1f(widthOverHeightUniform, widthOverHeight)
        context.uniform1f(zoomUniform, zoom)
        context.uniform2f(offsetUniform, x, y)
        context.uniform2f(juliaSeedUniform, jx, jy)

        context.drawArrays (context.TRIANGLE_STRIP, 0., 4.0)

    let clear = clear context

    let mutable last = DateTime.Now

    let render model =
        match model with
        | model when model.Now <> last ->
            last <- model.Now

            let resolution = canvas.width, canvas.height
            let timeloop = sin (float model.Now.Millisecond * Math.PI * 2.0 / 1000.0)
            let widthOverHeight = if canvas.height = 0.0 then 1.0 else canvas.width / canvas.height
            clear resolution

            draw widthOverHeight model.Zoom model.X model.Y -0.637 -0.414

        | _ -> ignore()
    
    render, height
