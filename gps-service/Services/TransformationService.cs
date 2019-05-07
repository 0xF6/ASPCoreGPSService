namespace gps_service.Services
{
    using System;
    using Core;
    using Microsoft.Extensions.Hosting;
    using Models;
    using MoreLinq;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class TransformationService : BackgroundService
    {
        private DBContext Ctx { get; }
        public TransformationService(DBContext ctx) => this.Ctx = ctx;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(450, stoppingToken);

                var collection = Ctx.Positions.Where(x => x.State == StateOf.NotProcessed);
                if (!collection.Any())
                    return;

                foreach (var groups in collection.GroupBy(x => x.SessionID))
                {
                    var sessionID = groups.Key;

                    var list = from elements in groups.Pipe(x => x.State = StateOf.Complete).Batch(2)
                        where elements.Count() == 2
                        let pos1 = elements.First()
                        let pos2 = elements.Last()
                        select new ProcessedGeo(pos1, pos2, sessionID);

                    
                    StorageManager.CreateStorage(list.ToArray());

                    await Ctx.SaveChangesAsync(stoppingToken);
                }
            }
        }

    }
}