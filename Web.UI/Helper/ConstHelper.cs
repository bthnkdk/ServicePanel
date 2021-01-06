using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Web.UI.Helper
{
    public static class ConstHelper
    {
        public static string StatusClass(int id)
        {
            switch (id)
            {
                case ServiceStatus.TAMAMLANDI:
                    return "bg-success";
                case ServiceStatus.ISLEMDE:
                    return "bg-info";
                default:
                    return "";
            }
        }
        public static string StatusName(int id)
        {
            switch (id)
            {
                case ServiceStatus.TAMAMLANDI:
                    return "Tamamlandı";
                case ServiceStatus.ISLEMDE:
                    return "İşlemde";
                default:
                    return "Seçilmedi";
            }
        }

        public static string ServiceStockStatusName(int id)
        {
            switch (id)
            {
                case ServiceStockStatus.DEPOCIKISIBEKLENIYOR:
                    return "Depo çıkışı bekleniyor";
                case ServiceStockStatus.ZIMMETLENDI:
                    return "Zimmetlendi";
                case ServiceStockStatus.TESLIMEDILDI:
                    return "Teslim Edildi";
                default:
                    return "";
            }
        }

        public static string VisitStatusClass(int id)
        {
            switch (id)
            {
                case VisitStatus.YENI:
                    return "Yeni";
                case ServiceStatus.ISLEMDE:
                    return "İşlemde";
                case VisitStatus.BEKLIYOR:
                    return "Bekliyor";
                case VisitStatus.TAMAMLANDI:
                    return "Tamamlandı";
                case VisitStatus.IPTAL:
                    return "İptal";
                default:
                    return "";
            }
        }

        public static class ServiceStatus
        {
            public const int YENI = 1;
            public const int ISLEMDE = 2;
            public const int ATANDI = 3;
            public const int URUNONAYIBEKLIYOR = 4;
            public const int KAPATILDI = 5;
            public const int TAMAMLANDI = 6;
        }

        public static class ServiceStockStatus
        {
            public const int DEPOCIKISIBEKLENIYOR = 1;
            public const int ZIMMETLENDI = 2;
            public const int TESLIMEDILDI = 3;

        }

        public static class ServicePriority
        {
            public const int DUSUK = 1;
            public const int NORMAL = 2;
            public const int YUKSEK = 3;
        }

        public static class CurrencyCode
        {
            public const string TL = "₺";
            public const string DOLAR = "$";
            public const string EURO = "€";

        }

        public static class OfferStatus
        {
            public const int YENI = 1;
            public const int ISLEMDE = 2;
            public const int YANITBEKLIYOR = 3;
            public const int OLUMLU = 4;
            public const int OLUMSUZ = 5;
            public const int NOTR = 6;
        }

        public static class VisitStatus
        {
            public const int YENI = 1;
            public const int ISLEMDE = 2;
            public const int BEKLIYOR = 3;
            public const int TAMAMLANDI = 4;
            public const int IPTAL = 5;
        }

        public static class Title
        {
            public const int YONETICI = 1;
            public const int TEKNIK_PERSONEL = 3;
            public const int SATIS_PERSONEL = 4;
            public const int PLANLAMACI = 5;
            public const int TEKNIK_YONETICI = 6;
            public const int SATIS_YONETICI = 7;
        }

        public static class StockMovementAction
        {
            public const int GIRIS = 1;
            public const int CIKIS = 2;
            public const int ZIMMET = 3;
            public const int IADE = 4;
        }

        public static Dictionary<int, string> AuthCodeList()
        {
            DataTable dataTable = new DataTable();
            DbHelper.FillTable(dataTable, "select Id,Name from AuthCode");
            Dictionary<int, string> items = new Dictionary<int, string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
                items.Add(Convert.ToInt32(dataTable.Rows[i][0]), dataTable.Rows[i][1].ToString());

            return items;
        }
        public static Dictionary<int, string> ServiceStatusList()
        {
            Dictionary<int, string> items = new Dictionary<int, string>();
            items.Add(ServiceStatus.YENI, "Yeni");
            items.Add(ServiceStatus.ISLEMDE, "İşlemde");
            items.Add(ServiceStatus.ATANDI, "Atandı");
            items.Add(ServiceStatus.URUNONAYIBEKLIYOR, "Ürün Onayı Bekliyor");
            items.Add(ServiceStatus.KAPATILDI, "Kapatıldı");
            items.Add(ServiceStatus.TAMAMLANDI, "Tamamlandı");
            return items;
        }
        public static Dictionary<int, string> ServicePrinterStatusList()
        {
            Dictionary<int, string> items = new Dictionary<int, string>();
            items.Add(ServiceStatus.ISLEMDE, "İşlemde");
            items.Add(ServiceStatus.TAMAMLANDI, "Tamamlandı");
            return items;
        }
        public static Dictionary<int, string> ServicePriorityList()
        {
            Dictionary<int, string> items = new Dictionary<int, string>();
            items.Add(ServicePriority.DUSUK, "Düşük");
            items.Add(ServicePriority.NORMAL, "Normal");
            items.Add(ServicePriority.YUKSEK, "Yüksek");
            return items;
        }
        public static Dictionary<int, string> OfferStatusList()
        {
            Dictionary<int, string> items = new Dictionary<int, string>();
            items.Add(1, "Yeni");
            items.Add(2, "İşlemde");
            items.Add(3, "Yanıt Bekliyor");
            items.Add(4, "Olumlu");
            items.Add(5, "Olumsuz");
            items.Add(6, "Nötr");
            return items;
        }
        public static Dictionary<int, string> VisitStatusList()
        {
            Dictionary<int, string> items = new Dictionary<int, string>();
            items.Add(VisitStatus.YENI, "Yeni");
            items.Add(VisitStatus.ISLEMDE, "İşlemde");
            items.Add(VisitStatus.BEKLIYOR, "Bekliyor");
            items.Add(VisitStatus.TAMAMLANDI, "Tamamlandı");
            items.Add(VisitStatus.IPTAL, "İptal");
            return items;
        }

        public static Dictionary<int, string> StockMovementActionList()
        {
            Dictionary<int, string> items = new Dictionary<int, string>();
            items.Add(StockMovementAction.GIRIS, "Giriş");
            items.Add(StockMovementAction.CIKIS, "Çıkış");
            items.Add(StockMovementAction.ZIMMET, "Zimmet");
            items.Add(StockMovementAction.IADE, "İade");
            return items;
        }
        public static Dictionary<string, string> AuthActionCodeList()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            items.Add("A", "Yetkili");
            items.Add("S", "Liste");
            items.Add("I", "Ekle");
            items.Add("U", "Güncelle");
            items.Add("D", "Sil");
            return items;
        }

        public static Dictionary<int, string> List100()
        {
            Dictionary<int, string> items = new Dictionary<int, string>();
            foreach (var item in Enumerable.Range(1, 100))
                items.Add(item, item.ToString());
            return items;
        }
        public static double GetCurrency(string code)
        {
            if (code == CurrencyCode.TL)
                return 1;

            if (CacheHelper.Get(code) == null)
            {
                var query = DbHelper.ExecuteReader("select top 1 Buy,Sell from CurrencyRate CR join Currency C on C.Id = CR.CurrencyId where C.Code = @code order by CR.Id Desc", code);
                if (query.Read())
                {
                    CurrencyModel currency = new CurrencyModel
                    {
                        Buy = Convert.ToDouble(query[0]),
                        Sell = Convert.ToDouble(query[1])
                    };
                    CacheHelper.Add(code, currency);
                }
            }
            return ((CurrencyModel)CacheHelper.Get(code)).Sell;
        }
    }
}