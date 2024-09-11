using D20Tek.Minimal.Functional;

namespace WealthTracker.Persistence;

internal interface IWealthRepository
{
    Maybe<WealthDataEntry> Create(WealthDataEntry entry);

    Maybe<WealthDataEntry> Delete(int id);
    
    WealthDataEntry[] GetWealthEntries();
    
    Maybe<WealthDataEntry> GetWealthEntryById(int id);
    
    Maybe<WealthDataEntry> Update(WealthDataEntry entry);
}
