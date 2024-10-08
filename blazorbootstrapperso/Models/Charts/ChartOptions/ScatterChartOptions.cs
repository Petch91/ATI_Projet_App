﻿namespace BlazorBootstrapPerso;

public class ScatterChartOptions : ChartOptions
{
    #region Properties, Indexers

    //hover -> mode, intersect
    //maintainAspectRatio
    //plugins -> title -> display, text

    /// <summary>
    /// The base axis of the dataset. 'x' for horizontal lines and 'y' for vertical lines.
    /// </summary>
    /// <remarks>
    /// Default value is 'x'.
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IndexAxis { get; set; }

    public Interaction Interaction { get; set; } = new();

    public ChartLayout Layout { get; set; } = new();

    public ScatterChartPlugins Plugins { get; set; } = new();

    public Scales Scales { get; set; } = new();

    #endregion

    //tooltips -> mode, intersect
}
