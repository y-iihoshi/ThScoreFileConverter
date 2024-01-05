#if NETFRAMEWORK

return;

#else

#pragma warning disable CA2007 // Do not directly await a Task

using Statiq.App;
using Statiq.Razor;

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
