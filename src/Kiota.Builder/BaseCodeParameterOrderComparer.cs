﻿using System;
using System.Collections.Generic;
using Kiota.Builder.CodeDOM;

namespace Kiota.Builder;
public class BaseCodeParameterOrderComparer : IComparer<CodeParameter>
{
    public int Compare(CodeParameter? x, CodeParameter? y)
    {
        return (x, y) switch
        {
            (null, null) => 0,
            (null, _) => -1,
            (_, null) => 1,
#pragma warning disable CA1062
            _ => x.Optional.CompareTo(y.Optional) * OptionalWeight +
                 GetKindOrderHint(x.Kind).CompareTo(GetKindOrderHint(y.Kind)) * KindWeight +
                 StringComparer.OrdinalIgnoreCase.Compare(x.Name, y.Name).CompareTo(0) * NameWeight,//normalize result from StringComparer.OrdinalIgnoreCase.Compare to the set {0,1,-1}
                                                                                                    //as it can return much larger numbers and mess up with the whole comparison
#pragma warning restore CA1062
        };
    }
    protected virtual int GetKindOrderHint(CodeParameterKind kind)
    {
        return kind switch
        {
            CodeParameterKind.PathParameters => 1,
            CodeParameterKind.RawUrl => 2,
            CodeParameterKind.RequestAdapter => 3,
            CodeParameterKind.Path => 4,
            CodeParameterKind.RequestConfiguration => 5,
            CodeParameterKind.RequestBody => 6,
            CodeParameterKind.ResponseHandler => 7,
            CodeParameterKind.Serializer => 8,
            CodeParameterKind.BackingStore => 9,
            CodeParameterKind.SetterValue => 10,
            CodeParameterKind.ParseNode => 11,
            CodeParameterKind.Custom => 12,
            _ => 13,
        };
    }
    private const int OptionalWeight = 10000;
    private const int KindWeight = 100;
    private const int NameWeight = 10;
}
