using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace BusinessLayer.Services
{
    public class MaintenanceService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private IProcCall _procCall;
        public MaintenanceService(IServiceProvider serviceProvider, IProcCall procCall)
        {
            _serviceProvider = serviceProvider;
            _procCall = procCall;
        }

        /// <summary>
        /// 타이머를 시작합니다.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        /// <summary>
        /// 정해진 시간마다 동작하는 이벤트 함수
        /// </summary>
        /// <param name="state"></param>
        private void DoWork(object state)
        {
            DateTime dt = DateTime.Now;
            int hour = dt.Hour;
            int minute = dt.Minute;
            if (hour == 0)
            {
                if(minute == 3) // 0시 3분에 캐시메모리에 새로운 데이터를 불러옵니다.
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        try
                        {
                            var stockService = scope.ServiceProvider.GetRequiredService<StockService>();
                            var indexService = scope.ServiceProvider.GetRequiredService<IndexService>();
                            var etfService = scope.ServiceProvider.GetRequiredService<ETFService>();
                            stockService.LoadDataAsync().Wait();
                            indexService.LoadDataAsync().Wait();
                            etfService.LoadDataAsync().Wait();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error during scheduled task: {ex.Message}");
                        }
                    }

                }
                else if (minute == 5) // 0시 5분에 데이터베이스 인덱스를 생성합니다.
                {

                        UpdateIndex().Wait();

                }


            }
        }

        /// <summary>
        /// 타이머를 멈춥니다.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0); // 서버동작하는 동안 계속 타이머가 동작합니다.
            return Task.CompletedTask;
        }

        /// <summary>
        /// 데이터베이스 인덱스를 다시 생성합니다.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateIndex()
        {
            var configuration = new MapperConfiguration(cfg => { });
            Mapper mapper = new Mapper(configuration);
            UpsertIndexKeyDTO dto = new UpsertIndexKeyDTO
            {
                type = 'a' // 모든 타입
            };
            Dictionary<string, object> dict = mapper.Map<UpsertIndexKeyDTO, Dictionary<string, object>>(dto);
            DataTable dt = await _procCall.RequestProcedure("ManageIndexes", dict); // dt에는 result가 있다.


        }   

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}