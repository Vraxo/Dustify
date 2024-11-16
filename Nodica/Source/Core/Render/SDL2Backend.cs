using SDL2;

namespace Nodica;

public class SDL2Backend : GraphicsBackendBase
{
    private IntPtr window;
    public IntPtr Renderer;

    public override void Initialize(int width, int height, string title)
    {
        // Initialize SDL2 with video and audio
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO) < 0)
        {
            Console.WriteLine($"SDL2 Initialization failed: {SDL.SDL_GetError()}");
            return;
        }

        // Create a window
        window = SDL.SDL_CreateWindow(title, SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, width, height, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
        if (window == IntPtr.Zero)
        {
            Console.WriteLine($"Failed to create SDL2 window: {SDL.SDL_GetError()}");
            return;
        }

        // Create a Renderer
        Renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
        if (Renderer == IntPtr.Zero)
        {
            Console.WriteLine($"Failed to create SDL2 renderer: {SDL.SDL_GetError()}");
            return;
        }

        SetWindowIcon("Resources/Icon/Icon.png");
    }

    public override void Run()
    {
        bool running = true;
        while (running)
        {
            // Event handling
            SDL.SDL_Event e;
            while (SDL.SDL_PollEvent(out e) != 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    running = false;
                }
            }

            BeginDrawing();
            ClearBackground(DefaultTheme.Background);
            EndDrawing();
        }

        Cleanup();
    }

    public override void SetWindowIcon(string iconPath)
    {
        IntPtr surface = SDL.SDL_LoadBMP(iconPath);
        if (surface == IntPtr.Zero)
        {
            Console.WriteLine($"Failed to load SDL2 icon: {SDL.SDL_GetError()}");
            return;
        }

        SDL.SDL_SetWindowIcon(window, surface);
        SDL.SDL_FreeSurface(surface);
    }

    public override void BeginDrawing()
    {
        SDL.SDL_RenderClear(Renderer);
    }

    public override void EndDrawing()
    {
        SDL.SDL_RenderPresent(Renderer);
    }

    public override void ClearBackground(Color color)
    {
        SDL.SDL_SetRenderDrawColor(Renderer, color.R, color.G, color.B, color.A);
        SDL.SDL_RenderClear(Renderer);
    }

    public override bool WindowShouldClose()
    {
        // Check if SDL_QUIT event is triggered
        SDL.SDL_Event e;
        while (SDL.SDL_PollEvent(out e) != 0)
        {
            if (e.type == SDL.SDL_EventType.SDL_QUIT)
                return true;
        }
        return false;
    }

    private void Cleanup()
    {
        if (Renderer != IntPtr.Zero)
        {
            SDL.SDL_DestroyRenderer(Renderer);
        }

        if (window != IntPtr.Zero)
        {
            SDL.SDL_DestroyWindow(window);
        }

        SDL.SDL_Quit();
    }
}
