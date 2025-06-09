using GildedRose.Lib;

namespace GildedRose.Tests;

public class Tests
{
    [Test]
    [Arguments("+5 Dexterity Vest")]
    [Arguments("Elixir of the Mongoose")]
    public async Task Normal_item_quality_should_degrade_by_one_then_by_two_once_sell_by_date_has_passed(
        string itemName)
    {
        var item = new Item { Name = itemName, SellIn = 1, Quality = 20 };

        UpdateInventoryCron.Tick([item]);

        await Assert.That(item.SellIn).IsEqualTo(0);
        await Assert.That(item.Quality).IsEqualTo(19);

        UpdateInventoryCron.Tick([item]);

        await Assert.That(item.SellIn).IsEqualTo(-1);
        await Assert.That(item.Quality).IsEqualTo(17);
    }

    [Test]
    [Arguments("+5 Dexterity Vest")]
    [Arguments("Elixir of the Mongoose")]
    public async Task Normal_item_quality_should_never_be_negative(string itemName)
    {
        var item = new Item { Name = itemName, SellIn = 0, Quality = 0 };

        UpdateInventoryCron.Tick([item]);

        await Assert.That(item.SellIn).IsEqualTo(-1);
        await Assert.That(item.Quality).IsEqualTo(0);
    }

    [Test]
    public async Task Conjured_item_quality_should_degrade_by_two_then_by_four_once_sell_by_date_has_passed()
    {
        var item = new Item { Name = "Conjured Mana Cake", SellIn = 1, Quality = 20 };

        UpdateInventoryCron.Tick([item]);

        await Assert.That(item.SellIn).IsEqualTo(0);
        await Assert.That(item.Quality).IsEqualTo(18);

        UpdateInventoryCron.Tick([item]);

        await Assert.That(item.SellIn).IsEqualTo(-1);
        await Assert.That(item.Quality).IsEqualTo(14);
    }

    [Test]
    public async Task Conjured_item_quality_should_never_be_negative()
    {
        var item = new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 0 };

        UpdateInventoryCron.Tick([item]);

        await Assert.That(item.SellIn).IsEqualTo(-1);
        await Assert.That(item.Quality).IsEqualTo(0);
    }

    [Test]
    // Increase in quality by one (but not exceed 50) if sell in is positive
    [Arguments(1, 1, 0, 2)]
    [Arguments(1, 50, 0, 50)]
    // Increase in quality by two (but not exceed 50) if sell in is 0 or negative
    [Arguments(0, 2, -1, 4)]
    [Arguments(-1, 4, -2, 6)]
    [Arguments(-10, 49, -11, 50)]
    public async Task Aged_brie_should_increase_in_quality_as_it_ages(
        int startingSellIn,
        int startingQuality,
        int expectedSellIn,
        int expectedQuality)
    {
        var item = new Item { Name = "Aged Brie", SellIn = startingSellIn, Quality = startingQuality };

        UpdateInventoryCron.Tick([item]);

        await Assert.That(item.SellIn).IsEqualTo(expectedSellIn);
        await Assert.That(item.Quality).IsEqualTo(expectedQuality);
    }

    [Test]
    [Arguments(1, 80)]
    [Arguments(0, 10)]
    [Arguments(-1, 20)]
    public async Task Sulfuras_should_never_change(int startingSellIn, int startingQuality)
    {
        var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = startingSellIn, Quality = startingQuality };

        UpdateInventoryCron.Tick([item]);

        await Assert.That(item.SellIn).IsEqualTo(startingSellIn);
        await Assert.That(item.Quality).IsEqualTo(startingQuality);
    }

    [Test]
    // Increase in quality by one (but not exceed 50) if sell in is greater than 10
    [Arguments(11, 1, 10, 2)]
    [Arguments(11, 50, 10, 50)]
    // Increase in quality by two (but not exceed 50) if sell in is between 10 and 6 inclusive
    [Arguments(10, 1, 9, 3)]
    [Arguments(6, 1, 5, 3)]
    [Arguments(10, 49, 9, 50)]
    // Increase in quality by three (but not exceed 50) if sell in is between 5 and 1 inclusive
    [Arguments(5, 1, 4, 4)]
    [Arguments(1, 1, 0, 4)]
    [Arguments(5, 49, 4, 50)]
    // Drop to 0 quality if sell in is 0 or negative
    [Arguments(0, 10, -1, 0)]
    [Arguments(-1, 10, -2, 0)]
    public async Task Backstage_passes_should_change_quality_as_they_age(
        int startingSellIn,
        int startingQuality,
        int expectedSellIn,
        int expectedQuality)
    {
        var item = new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = startingSellIn,
            Quality = startingQuality
        };

        UpdateInventoryCron.Tick([item]);

        await Assert.That(item.SellIn).IsEqualTo(expectedSellIn);
        await Assert.That(item.Quality).IsEqualTo(expectedQuality);
    }
}