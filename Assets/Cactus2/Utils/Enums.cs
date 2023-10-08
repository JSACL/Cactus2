using System;

public enum ColliderFlug : sbyte
{
    Stay = 0,
    Enter = 1,
    Exit = -1,
}

public enum Judgement
{
    None = 0,
    Invalid = -1,
    Valid = 1,
    Error = -4,
}

[Flags]
public enum Relationship
{
    None = 0b0000_0000,
    Master = 0b1000_0000,
    Slave = 0b0100_0000,
    Server = 0b0010_0000,
    Client = 0b0001_0000,
    Enemy = 0b0000_1000,
    Friend = 0b0000_0100,
    Other = 0b0000_0010,
    Same = 0b0000_0001,
}

public enum TeamRelationShip
{
    Enemy = -1,
    Friend = 1,
}

public enum SceneChangeAction
{
    Add,
    Remove,
}