using DB_app.Core.Contracts.Services;
namespace DB_app.Feature;

public class ObjectWrapper<TObjectType>
{
    protected readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    protected TObjectType? _backupData;

    
    public TObjectType ObjectData { get; set; }

    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    public bool IsModified;
}
