using Blog.BLL.ModelsDTO;
using Blog.DAL.Models;
using Riok.Mapperly.Abstractions;

namespace Blog.BLL.Core;

[Mapper(PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive)]
public partial class MapperlyMapper
{
    public partial void Map(User user, UserDto userDto);
    public partial UserDto Map(User user);
    public partial void Map(UserDto userDto, User user);
    public partial User Map(UserDto user);
}