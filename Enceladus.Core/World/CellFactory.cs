using Enceladus.Core.Config;

namespace Enceladus.Core.World
{
	public interface ICellFactory
	{
		Cell CreateCell(CellType cellType, int x, int y);
	}

	public class CellFactory : ICellFactory
	{
		private readonly IConfigService _configService;
		private Lazy<Dictionary<int, CellTypeConfig>> _cellTypeConfigs;

        public CellFactory(IConfigService configService)
		{
			_configService = configService;

			Init();
		}

		private void Init()
		{
            // One-time mapping: list to dictionary for O(1) lookups
            _cellTypeConfigs = new(() => _configService.Config.Cell.ToDictionary(c => c.Id));
        }

		public Cell CreateCell(CellType cellType, int x, int y)
		{
			var cellTypeConfig = _cellTypeConfigs.Value[cellType.Id];

			return new Cell
			{
				X = x,
				Y = y,
				CellType = cellType,
				Health = cellTypeConfig.MaxHealth
            };
		}
	}
}
