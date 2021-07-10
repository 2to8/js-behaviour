namespace Common.JSAdapter.sqlite3 {

public class sqlite3 {

    public const int OK = 0;
    public const int Error = 1;
    public const int Internal = 2;
    public const int Perm = 3;
    public const int Abort = 4;
    public const int Busy = 5;
    public const int Locked = 6;
    public const int NoMem = 7;
    public const int ReadOnly = 8;
    public const int Interrupt = 9;
    public const int IOError = 10;
    public const int Corrupt = 11;
    public const int NotFound = 12;
    public const int Full = 13;
    public const int CannotOpen = 14;
    public const int LockErr = 15;
    public const int Empty = 16;
    public const int SchemaChngd = 17;
    public const int TooBig = 18;
    public const int Constraint = 19;
    public const int Mismatch = 20;
    public const int Misuse = 21;
    public const int NotImplementedLFS = 22;
    public const int AccessDenied = 23;
    public const int Format = 24;
    public const int Range = 25;
    public const int NonDBFile = 26;
    public const int Notice = 27;
    public const int Warning = 28;
    public const int Row = 100;
    public const int Done = 101;

    public void verbose() { }

}

}