using ServicesOfSchoolOfForeignLanguagesApp.Classes;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace ServicesOfSchoolOfForeignLanguagesApp.AppData
{
    public partial class Service
    {
        public string GetCost { get => $"{(int)Cost * (Discount > 0 ? 1 : (1 - Discount))} рублей за {DurationInSeconds / 60} минут"; }
        public string GetDiscount { get => Discount > 0 ? $"* скидка {(int)(Discount * 100)}%" : null; }
        public double? GetRealCost { get => (int)Cost * (Discount > 0 ? 1 : (1 - Discount)); }
        public int GetOnlyCost { get => (int)Cost; }
        public string GetCostTrigger { get => Discount > 0 ? "Visible" : "Collapsed"; }
        public string AdminTest { get => Manager.IsAdminMode == true ? "Visible" : "Collapsed"; }
        public string GetColor { get => Discount > 0 ? "#FFE7FABF" : "Transparent"; }
        public double? DiscountManager { get => Discount != null ? Discount * 100 : 0; set => Discount = value / 100; }
    }
}
