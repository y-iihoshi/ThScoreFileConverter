using System.Globalization;
using Statiq.App;
using Statiq.Razor;
using ThScoreFileConverter.Core.Resources;

// Currently, HTML templates are only written in Japanese.
StringResources.Culture = CultureInfo.GetCultureInfo("ja-JP");

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
    .RunAsync()
    .ConfigureAwait(true);  // to avoid CA2007
