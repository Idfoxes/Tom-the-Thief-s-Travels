using System;using System;

internal class DllimportAttribute : Attribute
{
    private string v;

    public DllimportAttribute(string v)
    {
        this.v = v;
    }
}