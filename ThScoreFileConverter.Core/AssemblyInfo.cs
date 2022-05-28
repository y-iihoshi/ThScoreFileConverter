//-----------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

[assembly: CLSCompliant(true)]

#if DEBUG
[assembly: InternalsVisibleTo("ThScoreFileConverter.Core.Tests")]
#endif
