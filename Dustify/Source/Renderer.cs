using Nodica;
using Raylib_cs;
using System.Diagnostics;

namespace Dustify;

public class Renderer : Node
{
    public enum VideoQuality
    {
        Low,
        Medium,
        High
    }

    private ImageDisplayer imageDisplayer;
    private int currentFrame;
    private int totalFrames;
    private int processFrequency = 10;
    private int saveFrequency = 4;
    private int processCounter;
    private List<Image> framesToSave = new();
    private int saveCounter;
    private SemaphoreSlim saveSemaphore = new(1);
    private List<VideoQuality> qualitiesToGenerate;
    private ProgressBar progressBar;

    private bool rendering;

    public void Render(Texture2D texture, params VideoQuality[] qualities)
    {
        if (rendering || texture.Width == 0)
        {
            return;
        }

        rendering = true;
        InitializeImageDisplayer(texture);
        totalFrames = CalculateTotalFrames(texture.Height);
        qualitiesToGenerate = new(qualities);
        progressBar = GetNode<ProgressBar>("/root/ProgressBar");
        progressBar.Percentage = 0;
    }

    public override void Update()
    {
        if (imageDisplayer is null || processCounter >= totalFrames)
        {
            return;
        }

        if (currentFrame % processFrequency == 0)
        {
            ProcessFrame();
            processCounter++;
            UpdateProgressBar();
        }

        currentFrame++;

        if (processCounter == totalFrames)
        {
            StartSavingFrames();
        }

        base.Update();
    }

    private void InitializeImageDisplayer(Texture2D texture)
    {
        imageDisplayer = new();
        var textureRectangle = App.Instance.RootNode.GetNode<ImageDisplayer>("ImageSelectionButton/ARC/TextureRectangle");

        imageDisplayer.InheritPosition = false;
        imageDisplayer.GlobalPosition = new(500);
        imageDisplayer.OriginPreset = OriginPreset.TopLeft;
        imageDisplayer.Texture = texture;
        imageDisplayer.BitmapData = textureRectangle.BitmapData;
        imageDisplayer.Size = new(texture.Width, texture.Height);
        imageDisplayer.StartDisintegration();
        imageDisplayer.Speed = imageDisplayer.Speed * processFrequency;
    }

    private int CalculateTotalFrames(int textureHeight)
    {
        return textureHeight / 4 * 2;
    }

    private void ProcessFrame()
    {
        RenderTexture2D renderTexture = Raylib.LoadRenderTexture(imageDisplayer.Texture.Width, imageDisplayer.Texture.Height);

        Raylib.BeginTextureMode(renderTexture);
        Raylib.ClearBackground(Color.Black);
        imageDisplayer.Process();
        Raylib.EndTextureMode();

        if (processCounter % saveFrequency == 0)
        {
            SaveCurrentFrame(renderTexture);
        }
    }

    private void SaveCurrentFrame(RenderTexture2D renderTexture)
    {
        Image frame = Raylib.LoadImageFromTexture(renderTexture.Texture);
        framesToSave.Add(frame);
    }

    private void StartSavingFrames()
    {
        Thread saveThread = new(SaveAllFrames);
        saveThread.Start();
    }

    private void SaveAllFrames()
    {
        saveSemaphore.Wait();

        if (!Directory.Exists("Resources/Output"))
        {
            Directory.CreateDirectory("Resources/Output");
        }

        try
        {
            foreach (Image frame in framesToSave)
            {
                SaveFrame(frame);
                UpdateProgressBar();
            }

            framesToSave.Clear();
            GenerateVideos();
        }
        finally
        {
            saveSemaphore.Release();
        }
    }

    private void SaveFrame(Image frame)
    {
        string filePath = $"Resources/Output/output_frame_{saveCounter:D4}.png";
        Raylib.ImageFlipVertical(ref frame);
        Raylib.ExportImage(frame, filePath);
        //Raylib.UnloadImage(frame);
        saveCounter++;
    }

    private void GenerateVideos()
    {
        string ffmpegPath = "Resources/ffmpeg.exe";
        string outputFolderPath = "Resources/Output";

        if (qualitiesToGenerate.Contains(VideoQuality.Low))
        {
            string outputVideoPathLow = $"{outputFolderPath}/output_video_low.mp4";
            DeleteExistingVideos(outputVideoPathLow);
            string lowQualityArguments = $"-r 10 -i {outputFolderPath}/output_frame_%04d.png " +
                                         "-c:v libx264 -crf 35 -b:v 300k " +
                                         "-vf fps=60 -pix_fmt yuv420p " +
                                         outputVideoPathLow;
            StartFFmpegProcess(ffmpegPath, lowQualityArguments);
        }

        if (qualitiesToGenerate.Contains(VideoQuality.Medium))
        {
            string outputVideoPathMedium = $"{outputFolderPath}/output_video_medium.mp4";
            DeleteExistingVideos(outputVideoPathMedium);
            string mediumQualityArguments = $"-r 10 -i {outputFolderPath}/output_frame_%04d.png " +
                                            "-c:v libx264 -crf 28 -b:v 500k " +
                                            "-vf fps=60 -pix_fmt yuv420p " +
                                            outputVideoPathMedium;
            StartFFmpegProcess(ffmpegPath, mediumQualityArguments);
        }

        if (qualitiesToGenerate.Contains(VideoQuality.High))
        {
            string outputVideoPathHigh = $"{outputFolderPath}/output_video_high.mp4";
            DeleteExistingVideos(outputVideoPathHigh);
            string highQualityArguments = $"-r 10 -i {outputFolderPath}/output_frame_%04d.png " +
                                          "-c:v libx264 -crf 14 -b:v 2000k " +
                                          "-vf fps=60 -pix_fmt yuv420p " +
                                          outputVideoPathHigh;
            StartFFmpegProcess(ffmpegPath, highQualityArguments);
        }

        DeleteRenderedFrames();
        UpdateProgressBar(true);

        progressBar.Percentage = 0;
        rendering = false;

        if (Environment.GetCommandLineArgs().Length > 1)
        {
            Environment.Exit(0);
        }
        else
        {
            OpenOutputFolder(outputFolderPath);
        }

        Destroy();
    }

    private void OpenOutputFolder(string path)
    {
        string fullPath = Path.GetFullPath(path);

        if (Directory.Exists(fullPath))
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"\"{fullPath}\"",
                UseShellExecute = true
            });
        }
    }

    private void DeleteExistingVideos(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private void StartFFmpegProcess(string ffmpegPath, string arguments)
    {
        Process ffmpegProcess = new()
        {
            StartInfo = new()
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        ffmpegProcess.Start();
        ffmpegProcess.StandardOutput.ReadToEnd();
        ffmpegProcess.WaitForExit();
    }

    private void DeleteRenderedFrames()
    {
        for (int i = 0; i < saveCounter; i++)
        {
            string framePath = $"Resources/Output/output_frame_{i:D4}.png";

            if (File.Exists(framePath))
            {
                File.Delete(framePath);
            }
        }
    }

    private void UpdateProgressBar(bool complete = false)
    {
        if (progressBar == null) return;

        if (complete)
        {
            progressBar.Percentage = 1.0f;
            return;
        }

        float processProgress = (float)processCounter / totalFrames;
        float saveProgress = saveCounter > 0 ? (float)saveCounter / totalFrames : 0;
        float videoGenerationWeight = 0.1f;

        float combinedProgress = processProgress * 0.7f + saveProgress * 0.2f;

        if (processCounter == totalFrames && saveCounter == totalFrames)
        {
            combinedProgress += videoGenerationWeight;
        }

        progressBar.Percentage = combinedProgress;
    }
}