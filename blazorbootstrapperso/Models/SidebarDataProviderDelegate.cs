namespace BlazorBootstrapPerso;

/// <summary>
/// Data provider (delegate).
/// </summary>
public delegate Task<SidebarDataProviderResult> SidebarDataProviderDelegate(SidebarDataProviderRequest request);
