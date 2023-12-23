namespace DncyTemplate.Mvc.Models.Role;

public class PermissionTreeViewModel
{
    public PermissionTreeViewModel()
    {
        Children = [];
    }

    public string Id { get; set; }

    public string ParentId { get; set; }

    public string Title { get; set; }

    public List<PermissionTreeViewModel> Children { get; set; }

    private bool _isAssigned;
    public bool IsAssigned
    {
        get => _isAssigned;
        set
        {
            _isAssigned = value;
            if (_isAssigned)
            {
                CheckArr = "1";
            }
        }
    }

    public string CheckArr { get; set; } = "0";
}