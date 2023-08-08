using System;

public enum Action
{
    MoveForward, MoveBackward, MoveRight, MoveLeft, MoveUp, MoveDown,
    Jump, Sit,
    Func0, Func1, Func2, Func3, Func4, Func5, Func6, Func7, Func8, Func9, Func10,
    Escape,
}

public enum ColliderFlug : sbyte
{
    Stay = 0,
    Enter = 1,
    Exit = -1,
}