public struct Request
{
    private int misses;

    public int diagnostics;
    public int gear;
    public int fuel;
    public int repairs;

    public override bool Equals(object obj)
    {
        misses = 0;
        if(diagnostics != ((Request)obj).diagnostics)
        {
            misses++;
        }
        if (gear != ((Request)obj).gear)
        {
            misses++;
        }
        if (fuel != ((Request)obj).fuel)
        {
            misses++;
        }
        if (repairs != ((Request)obj).repairs)
        {
            misses++;
        }
        return misses == 0;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public int GetMisses()
    {
        return misses;
    }

    public void ResetOrder()
    {
        diagnostics = 0;
        fuel = 0;
        gear = 0;
        repairs = 0;
    }
}
