using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace DotNetAtom;

public class DefaultHostEnvironment : IHostEnvironment
{
    private string? _contentRootPath;
    private IFileProvider? _contentRootFileProvider;

    public string EnvironmentName { get; set; } = "Production";
    public string ApplicationName { get; set; } = "dotnetnuke";

    public string ContentRootPath
    {
        get => _contentRootPath ??= Directory.GetCurrentDirectory();
        set
        {
            _contentRootPath = value;
            _contentRootFileProvider = null;
        }
    }

    public IFileProvider ContentRootFileProvider
    {
        get => _contentRootFileProvider ??= new PhysicalFileProvider(ContentRootPath);
        set => _contentRootFileProvider = value;
    }
}
