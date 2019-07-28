// <copyright file="SQObjectType.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace ThScoreFileConverter.Squirrel
{
    /// <summary>
    /// Represents an object type defined by Squirrel 3.1.
    /// Refer to https://github.com/albertodemichelis/squirrel/blob/master/include/squirrel.h for details.
    /// </summary>
    public enum SQObjectType
    {
        /// <summary>
        /// The type Null has exactly one value, called null.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#null for details.
        /// </summary>
        Null = RawType.Null | SQObjectAttributes.CanBeFalse,

#pragma warning disable CA1720 // Identifier contains type name
        /// <summary>
        /// An integer represents a 32 bits (or better) signed number.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#integer for details.
        /// </summary>
        Integer = RawType.Integer | SQObjectAttributes.Numeric | SQObjectAttributes.CanBeFalse,

        /// <summary>
        /// A float represents a 32 bits (or better) floating point number.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#float for details.
        /// </summary>
        Float = RawType.Float | SQObjectAttributes.Numeric | SQObjectAttributes.CanBeFalse,
#pragma warning restore CA1720 // Identifier contains type name

        /// <summary>
        /// The Bool data type can have only two, the literals <c>true</c> and <c>false</c>.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#bool for details.
        /// </summary>
        Bool = RawType.Bool | SQObjectAttributes.CanBeFalse,

#pragma warning disable CA1720 // Identifier contains type name
        /// <summary>
        /// A String is an immutable sequence of characters; to modify a string is necessary create a new one.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#string for details.
        /// </summary>
        String = RawType.String | SQObjectAttributes.RefCounted,
#pragma warning restore CA1720 // Identifier contains type name

        /// <summary>
        /// Tables are associative containers implemented as pairs of key/value (called a slot).
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#table for details.
        /// </summary>
        Table = RawType.Table | SQObjectAttributes.RefCounted | SQObjectAttributes.Delegable,

        /// <summary>
        /// Arrays are simple sequence of objects, their size is dynamic and their index starts always from 0.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#array for details.
        /// </summary>
        Array = RawType.Array | SQObjectAttributes.RefCounted,

        /// <summary>
        /// Userdata objects are blobs of memory (or pointers) defined by the host application but stored into Squirrel variables.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#userdata for details.
        /// </summary>
        UserData = RawType.UserData | SQObjectAttributes.RefCounted | SQObjectAttributes.Delegable,

        /// <summary>
        /// Closure.
        /// </summary>
        Closure = RawType.Closure | SQObjectAttributes.RefCounted,

        /// <summary>
        /// Native closure.
        /// </summary>
        NativeClosure = RawType.NativeClosure | SQObjectAttributes.RefCounted,

        /// <summary>
        /// Generators are functions that can be suspended with the statement <c>yield</c> and resumed later.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#generator for details.
        /// </summary>
        Generator = RawType.Generator | SQObjectAttributes.RefCounted,

        /// <summary>
        /// This type is not a memory chunk like the normal userdata, but just a <c>void*</c> pointer.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/embedding/userdata_and_userpointers.html for details.
        /// </summary>
        UserPointer = RawType.UserPointer,

        /// <summary>
        /// Threads are objects that represents a cooperative thread of execution, also known as coroutines.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#thread for details.
        /// </summary>
        Thread = RawType.Thread | SQObjectAttributes.RefCounted,

        /// <summary>
        /// Functions are similar to those in other C-like languages and to most programming languages in general,
        /// however there are a few key differences.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#function for details.
        /// </summary>
        FuncProto = RawType.FuncProto | SQObjectAttributes.RefCounted,

        /// <summary>
        /// Classes are associative containers implemented as pairs of key/value.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#class for details.
        /// </summary>
        Class = RawType.Class | SQObjectAttributes.RefCounted,

        /// <summary>
        /// Class instances are created by calling a class object.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#class-instance for details.
        /// </summary>
        Instance = RawType.Instance | SQObjectAttributes.RefCounted | SQObjectAttributes.Delegable,

        /// <summary>
        /// Weak References are objects that point to another(non scalar) object but do not own a strong reference to it.
        /// Refer to http://www.squirrel-lang.org/squirreldoc/reference/language/datatypes.html#weak-reference for details.
        /// </summary>
        WeakRef = RawType.WeakRef | SQObjectAttributes.RefCounted,

        /// <summary>
        /// Internal usage only.
        /// </summary>
        Outer = RawType.Outer | SQObjectAttributes.RefCounted,
    }
}
