#if NETFRAMEWORK

return;

#else

#pragma warning disable CA2007 // Do not directly await a Task

using System.Globalization;
using Statiq.App;
using Statiq.Razor;

// Currently, HTML templates are only written in Japanese.
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

return await Bootstrapper
    .Factory
    .CreateDefaultWithout(args, DefaultFeatures.Namespaces)
    .BuildPipeline("Render Razor", builder => builder
        .WithInputReadFiles("**/*.cshtml")
        .WithProcessModules(new RenderRazor())
        .WithOutputWriteFiles(".html"))
    .BuildPipeline("Copy Assets", builder => builder
        .WithInputReadFiles("**/*.css")
        .WithOutputWriteFiles())
    .RunAsync();

#endif
