using System.Drawing;
using TradingPlatform.BusinessLayer;
namespace QuanTAlib;

public class SDEV_chart : Indicator
{
    #region Parameters

    [InputParameter("Smoothing period", 0, 1, 999, 1, 1)]
    private int Period = 10;

    [InputParameter("Data source", 1, variants: new object[]
      { "Open", 0, "High", 1,  "Low", 2,  "Close", 3,  "HL2", 4,  "OC2", 5,
      "OHL3", 6,  "HLC3", 7,  "OHLC4", 8,  "Weighted (HLCC4)", 9 })]
    private int DataSource = 8;

    #endregion Parameters

    private TBars bars;

    ///////dotnet
    private SSDEV_Series indicator;
    ///////

    public SDEV_chart()
    {
        this.SeparateWindow = true;
        this.Name = "SDEV - Sample Standard Deviation (Unbiased)";
        this.Description = "SDEV description";
        this.AddLineSeries("SDEV", Color.RoyalBlue, 3, LineStyle.Solid);
    }

    protected override void OnInit()
    {
	    this.bars = new(); 
	    this.ShortName =
            "SDEV (" + TBars.SelectStr(this.DataSource) + ", " + this.Period + ")";
        this.indicator = new(source: bars.Select(this.DataSource),
                             period: this.Period, useNaN: true);
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

        this.SetValue(result, 0);
    }
}
