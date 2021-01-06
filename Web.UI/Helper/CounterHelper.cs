using Core;
using Domain;
using System;
using System.Linq;
using System.Transactions;
using Web.UI.Areas.PRT;
using Web.UI.Mappers;

namespace Web.UI.Helper
{
    public class CounterHelper
    {
        IRepo<Counter> counterRepo;
        IRepo<TonerChange> tonerChangeRepo;

        IMapper mapper;
        public CounterHelper(IRepo<Counter> counterRepo, IRepo<TonerChange> tonerChangeRepo, IMapper mapper)
        {
            this.counterRepo = counterRepo;
            this.mapper = mapper;
            this.tonerChangeRepo = tonerChangeRepo;
        }


        public void DeleteCounter(int id)
        {
            var entity = counterRepo.Get(id);

            if (entity == null)
                throw new Exception("Sayaç bulunamadı");

            var tonerChangeList = tonerChangeRepo.Where(s => s.CounterId == entity.RowId).ToList();

            foreach (var change in tonerChangeList)
                tonerChangeRepo.Delete(change);

            counterRepo.Delete(entity);

            using (TransactionScope scope = new TransactionScope())
            {
                counterRepo.Save();
                tonerChangeRepo.Save();
                scope.Complete();
            }
        }

        public int SetCounter(CounterInput input)
        {
            var counters = counterRepo.Where(s => s.PrinterId == input.PrinterId).ToList();
            Counter entity = null;
            if (counters != null && counters.Count > 0)
                entity = counters.LastOrDefault();

            if(!input.Mono.HasValue)
                throw new Exception("Lütfen siyah sayaç bilgisi giriniz");

            input.RowId = Guid.NewGuid();
            if (entity != null)
            {
                if (entity.Mono > input.Mono)
                    throw new Exception("Lütfen son siyah sayaçdan daha büyük bir değer giriniz. Bu yazıcının son siyah sayacı : " + entity.Mono);

                if (entity.Color > input.Color)
                    throw new Exception("Lütfen son renkli sayaçdan daha büyük bir değer giriniz. Bu yazıcının son renkli sayacı : " + entity.Color);

                //toner değeri girilmişse ve veritabanındaki değerden büyükse yada veritabanında değer yoksa
                if (input.Cyan.HasValue && ((entity.Cyan.HasValue && entity.Cyan < input.Cyan) || !entity.Cyan.HasValue))
                    tonerChangeRepo.Insert(new TonerChange { Date = DateTime.Now, OldValue = entity.Cyan.HasValue ? entity.Cyan.Value : 0, NewValue = input.Cyan.Value, PrinterId = entity.PrinterId, Toner = "C", CounterId = input.RowId });

                if (input.Magenta.HasValue && ((entity.Magenta.HasValue && entity.Magenta < input.Magenta) || !entity.Magenta.HasValue))
                    tonerChangeRepo.Insert(new TonerChange { Date = DateTime.Now, OldValue = entity.Magenta.HasValue ? entity.Magenta.Value : 0, NewValue = input.Magenta.Value, PrinterId = entity.PrinterId, Toner = "M", CounterId = input.RowId });

                if (input.Yellow.HasValue && ((entity.Yellow.HasValue && entity.Yellow < input.Yellow) || !entity.Yellow.HasValue))
                    tonerChangeRepo.Insert(new TonerChange { Date = DateTime.Now, OldValue = entity.Yellow.HasValue ? entity.Yellow.Value : 0, NewValue = input.Yellow.Value, PrinterId = entity.PrinterId, Toner = "Y", CounterId = input.RowId });

                if (input.Black.HasValue && ((entity.Black.HasValue && entity.Black < input.Black) || !entity.Black.HasValue))
                    tonerChangeRepo.Insert(new TonerChange { Date = DateTime.Now, OldValue = entity.Black.HasValue ? entity.Black.Value : 0, NewValue = input.Black.Value, PrinterId = entity.PrinterId, Toner = "B", CounterId = input.RowId });
            }
            else
            {
                if (input.Cyan.HasValue)
                    tonerChangeRepo.Insert(new TonerChange { Date = DateTime.Now, OldValue = 0, NewValue = input.Cyan.Value, PrinterId = entity.PrinterId, Toner = "C", CounterId = input.RowId });
                if (input.Magenta.HasValue)
                    tonerChangeRepo.Insert(new TonerChange { Date = DateTime.Now, OldValue = 0, NewValue = input.Magenta.Value, PrinterId = entity.PrinterId, Toner = "M", CounterId = input.RowId });
                if (input.Yellow.HasValue)
                    tonerChangeRepo.Insert(new TonerChange { Date = DateTime.Now, OldValue = 0, NewValue = input.Yellow.Value, PrinterId = entity.PrinterId, Toner = "Y", CounterId = input.RowId });
                if (input.Black.HasValue)
                    tonerChangeRepo.Insert(new TonerChange { Date = DateTime.Now, OldValue = 0, NewValue = input.Black.Value, PrinterId = entity.PrinterId, Toner = "B", CounterId = input.RowId });
            }
            var ent = counterRepo.Insert(new Counter
            {
                Mono = input.Mono.Value, //TO DO: Patlayabilir
                Date = DateTime.Now,
                PrinterId = input.PrinterId,
                Color = input.Color,
                Black = input.Black,
                Cyan = input.Cyan,
                Magenta = input.Magenta,
                Yellow = input.Yellow,
                RowId = input.RowId
            }); //TO DO: Kontrol edilecek mapper a çevrilebilir.

            using (TransactionScope scope = new TransactionScope())
            {
                counterRepo.Save();
                tonerChangeRepo.Save();
                scope.Complete();
            }
            return ent.Id;
        }
    }
}