//-----------------------------------------------------------------------
// <copyright file="SceneParser.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Core.Models.Th143;

/// <summary>
/// Provides the parser of ISC scenes.
/// </summary>
public sealed class SceneParser : IntegerParser
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SceneParser"/> class.
    /// </summary>
    public SceneParser()
        : base(@"\d")
    {
    }

    /// <summary>
    /// Converts from the group matched with the pattern to a value indicating a scene.
    /// </summary>
    /// <param name="group">The group matched by <see cref="IntegerParser.Pattern"/>.</param>
    /// <returns>The parsed value indicating a scene.</returns>
    public override int Parse(Group group)
    {
        var scene = base.Parse(group);
        return scene == 0 ? 10 : scene;
    }
}
