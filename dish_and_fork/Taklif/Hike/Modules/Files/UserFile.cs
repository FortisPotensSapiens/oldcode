using Daf.SharedModule.Domain;

namespace Daf.FilesModule.Domain
{
    public record UserFile(FileName File, UserId UserId) { }
}
