namespace GildedRose.Lib;

public static class UpdateInventoryCron
{
    public static void Tick(IEnumerable<Item> items)
    {
        foreach (var item in items)
        {
            switch (item.Name)
            {
                case "Aged Brie":
                    UpdateAgedBrie(item);
                    break;
                case "Backstage passes to a TAFKAL80ETC concert":
                    UpdateBackstagePasses(item);
                    break;
                case "Sulfuras, Hand of Ragnaros":
                    UpdateLegendaryItem(item);
                    break;
                case "Conjured Mana Cake":
                    UpdateConjuredItem(item);
                    break;
                default:
                    UpdateNormalItem(item);
                    break;
            }
        }
    }

    public static void TickOld(List<Item> items)
    {
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].Name != "Aged Brie" && items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
            {
                if (items[i].Quality > 0)
                {
                    if (items[i].Name != "Sulfuras, Hand of Ragnaros")
                    {
                        items[i].Quality = items[i].Quality - 1;
                    }
                }
            }
            else
            {
                if (items[i].Quality < 50)
                {
                    items[i].Quality = items[i].Quality + 1;

                    if (items[i].Name == "Backstage passes to a TAFKAL80ETC concert")
                    {
                        if (items[i].SellIn < 11)
                        {
                            if (items[i].Quality < 50)
                            {
                                items[i].Quality = items[i].Quality + 1;
                            }
                        }

                        if (items[i].SellIn < 6)
                        {
                            if (items[i].Quality < 50)
                            {
                                items[i].Quality = items[i].Quality + 1;
                            }
                        }
                    }
                }
            }

            if (items[i].Name != "Sulfuras, Hand of Ragnaros")
            {
                items[i].SellIn = items[i].SellIn - 1;
            }

            if (items[i].SellIn < 0)
            {
                if (items[i].Name != "Aged Brie")
                {
                    if (items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
                    {
                        if (items[i].Quality > 0)
                        {
                            if (items[i].Name != "Sulfuras, Hand of Ragnaros")
                            {
                                items[i].Quality = items[i].Quality - 1;
                            }
                        }
                    }
                    else
                    {
                        items[i].Quality = items[i].Quality - items[i].Quality;
                    }
                }
                else
                {
                    if (items[i].Quality < 50)
                    {
                        items[i].Quality = items[i].Quality + 1;
                    }
                }
            }
        }
    }

    private static void UpdateAgedBrie(Item item)
    {
        item.Quality++;

        if (item.SellIn <= 0)
        {
            item.Quality++;
        }

        item.Quality = Math.Min(item.Quality, 50);
        item.SellIn -= 1;
    }

    private static void UpdateBackstagePasses(Item item)
    {
        item.Quality++;

        if (item.SellIn <= 10)
        {
            item.Quality++;
        }

        if (item.SellIn <= 5)
        {
            item.Quality++;
        }

        if (item.SellIn <= 0)
        {
            item.Quality = 0;
        }

        item.Quality = Math.Min(item.Quality, 50);
        item.SellIn -= 1;
    }

    private static void UpdateLegendaryItem(Item item)
    {
    }

    private static void UpdateConjuredItem(Item item)
    {
        item.Quality -= 2;

        if (item.SellIn <= 0)
        {
            item.Quality -= 2;
        }

        item.Quality = Math.Max(item.Quality, 0);
        item.SellIn -= 1;
    }

    private static void UpdateNormalItem(Item item)
    {
        item.Quality--;

        if (item.SellIn <= 0)
        {
            item.Quality--;
        }

        item.Quality = Math.Max(item.Quality, 0);
        item.SellIn -= 1;
    }
}