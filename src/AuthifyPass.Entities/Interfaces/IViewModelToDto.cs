namespace AuthifyPass.Entities.Interfaces;
public interface IViewModelToDto<DtoType>
{
    DtoType ToDto();
}
