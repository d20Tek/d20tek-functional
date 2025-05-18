using D20Tek.LowDb.Repositories;
using WealthTracker.Domain;

namespace WealthTracker.Common;

internal interface IWealthRepository : IRepository<WealthDataEntity>;
