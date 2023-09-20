using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class ImageClipboard
{
    // can be invoked not from MainThread!
    public static readonly bool IsSupported =
#if UNITY_EDITOR_WIN && UNITY_EDITOR_64
        true;
#elif UNITY_STANDALONE_WIN && UNITY_64
        true;
#else
        false;
#endif

    // can be invoked not from MainThread!
    private static readonly string DataPath = Application.dataPath;

    // can be invoked not from MainThread!
    private static void RequireSupport()
    {
        if (!IsSupported)
            throw new NotSupportedException(
                "ImageClipboard is not supported on the current platform. Use 'ImageClipboard.IsSupported' to determine platform support");
    }

    
    
    // can be invoked not from MainThread! 
    public static string PathToClipboardCliApp
    {
        get
        {
            if (Application.isEditor)
            {
                var pathToProject = Path.GetDirectoryName(DataPath);
                var pathToProjectBinaries = Path.Combine(pathToProject, "Binaries");
                var pathToClipboardCliApp = Path.Combine(pathToProjectBinaries, "clipboard-image-win-x64.exe");

                return pathToClipboardCliApp;
            }
            else
            {
                RequireSupport();
                var pathToClipboardCliApp = Path.Combine(Application.dataPath, "clipboard-image-win-x64.exe");
                return pathToClipboardCliApp;
            }
        }
    }

    // can be invoked not from MainThread!
    private static Process StartImageClipboard(string pathToImage)
    {
        var processStartInfo = new ProcessStartInfo
        {
            Arguments = pathToImage,
            CreateNoWindow = true,
            FileName = PathToClipboardCliApp,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };

        return Process.Start(processStartInfo);
    }

    public static async UniTask SetClipboardImageAsync(Texture2D texture, CancellationToken cancellationToken)
    {
        RequireSupport();

        var texturePng = texture.EncodeToPNG();

        await UniTask.SwitchToThreadPool();

        var pathToImage = Path.GetTempFileName();
        File.WriteAllBytes(pathToImage, texturePng);

        using var clipboardProcess = StartImageClipboard(pathToImage);

        while (!clipboardProcess.HasExited)
        {
            if (cancellationToken.IsCancellationRequested)
                clipboardProcess.Kill();

            await UniTask.NextFrame(cancellationToken);
        }

        clipboardProcess.WaitForExit(200);
        File.Delete(pathToImage);

        cancellationToken.ThrowIfCancellationRequested();
        if (clipboardProcess.ExitCode != 0)
            throw new InvalidOperationException();
    }

    public static void SetClipboardImage(Texture2D texture, int timeoutMilliseconds)
    {
        RequireSupport();
        
        var texturePng = texture.EncodeToPNG();

        var pathToImage = Path.GetTempFileName();
        File.WriteAllBytes(pathToImage, texturePng);

        using var clipboardProcess = StartImageClipboard(pathToImage);

        if (!clipboardProcess.WaitForExit(timeoutMilliseconds))
        {
            File.Delete(pathToImage);
            throw new OperationCanceledException();
        }

        File.Delete(pathToImage);

        if (clipboardProcess.ExitCode != 0)
            throw new InvalidOperationException();
    }
}