/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 20/09/2016 03:33
*/

using System;

public interface ITypeCreator<T, TParam>
{
    T Create(TParam param);
}
