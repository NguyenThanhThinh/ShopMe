using System;

namespace ShopMe.Domains
{
  public  class ReportInfoViewModel
    {
        public int iGroup { get; set; }
        public String Group { get; set; }
        public int Count { get; set; }
        public double Sum { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Avg { get; set; }
    }
    
}
