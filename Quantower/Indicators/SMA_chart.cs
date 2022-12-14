using System.Drawing;
using TradingPlatform.BusinessLayer;
namespace QuanTAlib;

public class SMA_chart : Indicator
{
    #region Parameters

    [InputParameter("Smoothing period", 0, 1, 999, 1, 1)]
    private readonly int Period = 10;

    [InputParameter("Data source", 1, variants: new object[]
      { "Open", 0, "High", 1,  "Low", 2,  "Close", 3,  "HL2", 4,  "OC2", 5,
      "OHL3", 6,  "HLC3", 7,  "OHLC4", 8,  "Weighted (HLCC4)", 9 })]
    private readonly int DataSource = 3;

    #endregion Parameters

    private TBars bars;

    ///////
    private SMA_Series indicator;
    ///////

    public SMA_chart()
    {
        this.SeparateWindow = false;
        this.Name = "SMA - Simple Moving Average";
        this.Description = "Simple Moving Average description";
        this.AddLineSeries("SMA", Color.RoyalBlue, 3, LineStyle.Solid);
    }

    protected override void OnInit()
    {
	    this.bars = new(); 
	    this.ShortName =
            "SMA (" + TBars.SelectStr(this.DataSource) + ", " + this.Period + ")";
        this.indicator = new(source: bars.Select(this.DataSource),
                             period: this.Period, useNaN: false);
    }

	protected override void OnUpdate(UpdateArgs args)
    {
        bool update = !(args.Reason == UpdateReason.NewBar ||
                        args.Reason == UpdateReason.HistoricalBar);
        this.bars.Add(this.Time(), this.GetPrice(PriceType.Open),
                      this.GetPrice(PriceType.High), this.GetPrice(PriceType.Low),
                      this.GetPrice(PriceType.Close),
                      this.GetPrice(PriceType.Volume), update);
        double result = this.indicator[this.indicator.Count - 1].v;
        this.SetValue(result);
    }
}
