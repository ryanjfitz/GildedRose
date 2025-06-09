namespace GildedRose.Lib;

public class Item
{
    public required string Name { get; init; }

    public int SellIn { get; set; }

    public int Quality { get; set; }
}