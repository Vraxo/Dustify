using Raylib_cs;
using SDL2;

namespace Nodica;

public class ThemedRectangleDrawCommand(Vector2 position, Vector2 size, BoxTheme theme) : DrawCommand
{
    protected void DrawRaylib(Vector2 position, Vector2 size, BoxTheme theme)
    {
        float top = theme.BorderLengthUp;
        float right = theme.BorderLengthRight;
        float bottom = theme.BorderLengthDown;
        float left = theme.BorderLengthLeft;

        Rectangle outerRectangle = new()
        {
            Position = new Vector2(position.X - left, position.Y - top),
            Size = new Vector2(size.X + left + right, size.Y + top + bottom)
        };

        if (top > 0 || right > 0 || bottom > 0 || left > 0)
        {
            Raylib.DrawRectangleRounded(
                outerRectangle,
                theme.Roundness,
                (int)size.Y,
                theme.BorderColor
            );
        }

        Rectangle innerRectangle = new()
        {
            Position = position,
            Size = size
        };

        Raylib.DrawRectangleRounded(
            innerRectangle,
            theme.Roundness,
            (int)size.Y,
            theme.FillColor
        );
    }

    protected void DrawSDL2()
    {
        var renderer = ((SDL2Backend)(App.Instance.GraphicsBackend)).Renderer;  // Use the Renderer from SDL2Backend

        float top = theme.BorderLengthUp;
        float right = theme.BorderLengthRight;
        float bottom = theme.BorderLengthDown;
        float left = theme.BorderLengthLeft;

        // Define outer rectangle with borders
        SDL.SDL_Rect outerRectangle = new SDL.SDL_Rect
        {
            x = (int)(position.X - left),
            y = (int)(position.Y - top),
            w = (int)(size.X + left + right),
            h = (int)(size.Y + top + bottom)
        };

        // Draw border (outline) if needed
        if (top > 0 || right > 0 || bottom > 0 || left > 0)
        {
            // Set border color
            SDL.SDL_SetRenderDrawColor(renderer, theme.BorderColor.R, theme.BorderColor.G, theme.BorderColor.B, theme.BorderColor.A);

            // Draw outer rounded rectangle (SDL2 doesn't have rounded rectangles, so we will need to approximate with circles on the corners and lines)
            DrawRoundedRectangle(outerRectangle, theme.Roundness, renderer);
        }

        // Define inner rectangle (filled)
        SDL.SDL_Rect innerRectangle = new SDL.SDL_Rect
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = (int)size.X,
            h = (int)size.Y
        };

        // Draw the filled rectangle
        SDL.SDL_SetRenderDrawColor(renderer, theme.FillColor.R, theme.FillColor.G, theme.FillColor.B, theme.FillColor.A);
        SDL.SDL_RenderFillRect(renderer, ref innerRectangle);
    }

    private void DrawRoundedRectangle(SDL.SDL_Rect rect, float roundness, IntPtr renderer)
    {
        // Draw the four rounded corners of the rectangle (using circles or arcs)
        int radius = (int)roundness;
        DrawCircle(rect.x + radius, rect.y + radius, radius, renderer);
        DrawCircle(rect.x + rect.w - radius, rect.y + radius, radius, renderer);
        DrawCircle(rect.x + radius, rect.y + rect.h - radius, radius, renderer);
        DrawCircle(rect.x + rect.w - radius, rect.y + rect.h - radius, radius, renderer);

        // Draw the sides of the rectangle (the lines between the corners)
        SDL.SDL_RenderDrawLine(renderer, rect.x + radius, rect.y, rect.x + rect.w - radius, rect.y); // Top
        SDL.SDL_RenderDrawLine(renderer, rect.x + radius, rect.y + rect.h, rect.x + rect.w - radius, rect.y + rect.h); // Bottom
        SDL.SDL_RenderDrawLine(renderer, rect.x, rect.y + radius, rect.x, rect.y + rect.h - radius); // Left
        SDL.SDL_RenderDrawLine(renderer, rect.x + rect.w, rect.y + radius, rect.x + rect.w, rect.y + rect.h - radius); // Right
    }

    private void DrawCircle(int centerX, int centerY, int radius, IntPtr renderer)
    {
        int offsetX = 0;
        int offsetY = radius;
        int d = 1 - radius;

        while (offsetX <= offsetY)
        {
            DrawFilledCirclePoints(centerX, centerY, offsetX, offsetY, renderer);
            if (d < 0)
            {
                d += 2 * offsetX + 3;
            }
            else
            {
                d += 2 * (offsetX - offsetY) + 5;
                offsetY--;
            }
            offsetX++;
        }
    }

    private void DrawFilledCirclePoints(int centerX, int centerY, int offsetX, int offsetY, IntPtr renderer)
    {
        // Draw horizontal lines for each vertical slice of the circle
        SDL.SDL_RenderDrawLine(renderer, centerX - offsetX, centerY + offsetY - 1, centerX + offsetX - 1, centerY + offsetY - 1);
        SDL.SDL_RenderDrawLine(renderer, centerX - offsetX, centerY - offsetY, centerX + offsetX - 1, centerY - offsetY);
        SDL.SDL_RenderDrawLine(renderer, centerX - offsetY, centerY + offsetX - 1, centerX + offsetY - 1, centerY + offsetX - 1);
        SDL.SDL_RenderDrawLine(renderer, centerX - offsetY, centerY - offsetX, centerX + offsetY - 1, centerY - offsetX);
    }
}