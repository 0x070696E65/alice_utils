using System.ComponentModel;

namespace aLice_utils.Shared.Models.Transaction;

public class TransactionMeta: INotifyPropertyChanged
{
    public string Deadline { get; set; } = "7200";
    public string FeeMultiplier { get; set; } = "100";

    public string FixedDeadline { get; set; } = "Flexible";
    
    private string _networkType = "TestNet";
    public string NetworkType
    {
        get => _networkType;
        set
        {
            if (_networkType == value) return;
            _networkType = value;
            OnPropertyChanged(nameof(NetworkType));
        }
    }
    public string Node { get; set; } = "";
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}