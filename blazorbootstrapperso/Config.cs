﻿using BlazorBootstrapPerso;

namespace Microsoft.Extensions.DependencyInjection;

public static class Config
{
    #region Methods

    /// <summary>
    /// Adds a bootstrap providers and component mappings.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddBlazorBootstrapPerso(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<BreadcrumbService>();
        serviceCollection.AddScoped<ModalService>();
        serviceCollection.AddScoped<PreloadService>();
        serviceCollection.AddScoped<ToastService>();

        serviceCollection.AddScoped<PdfViewerJsInterop>();
        serviceCollection.AddScoped<SortableListJsInterop>();

        return serviceCollection;
    }

    #endregion
}
