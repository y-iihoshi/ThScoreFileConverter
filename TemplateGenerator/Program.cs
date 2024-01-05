#if NETFRAMEWORK

return;

#else

#pragma warning disable CA2007 // Do not directly await a Task

using Statiq.App;

return await Bootstrapper
    .Factory
    .CreateDefault(args)
    .BuildPipeline("Copy Assets", builder => builder
        .WithInputReadFiles("**/*.css")
        .WithOutputWriteFiles())
    .RunAsync();

#endif
