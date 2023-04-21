using Blog.BLL.ModelsDTOs;
using Blog.DAL.Models;
using Riok.Mapperly.Abstractions;

namespace Blog.BLL.Core;

[Mapper(PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive, UseReferenceHandling = true)]
public partial class MapperlyMapper
{
    public partial void Map(User user, ProfileDto profileDto);
    public partial ProfileDto Map(User user);
    public partial void Map(ProfileDto profileDto, User user);
    public partial User Map(ProfileDto profileDto);

    public partial void Map(Article article, ArticleDto articleDto);
    public partial ArticleDto Map(Article article);
    public partial void Map(ArticleDto articleDto, Article article);
    public partial Article Map(ArticleDto articleDto);

    public partial void Map(Rubric rubric, RubricDto rubricDto);
    public partial RubricDto Map(Rubric rubric);
    public partial void Map(RubricDto rubricDto, Rubric rubric);
    public partial Rubric Map(RubricDto rubricDto);
    
    public partial void Map(Comment comment, CommentDto commentDto);
    public partial CommentDto Map(Comment comment);
    public partial void Map(CommentDto commentDto, Comment comment);
    public partial Comment Map(CommentDto commentDto);
}