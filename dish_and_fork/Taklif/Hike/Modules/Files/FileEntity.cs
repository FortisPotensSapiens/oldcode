using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.FilesModule.Domain
{
    public record FileEvent : IEntityEvent;
    public record MerchIdAddedeToFile(MerchId merchId) : FileEvent;

    public record FileEntity(
        FileId Id,
        FileHash Hash,
        FileSize Size,
        FileContentType ContentType,
        FileExtention Extention,
        IEnumerable<UserFile> UserFiles,
        IEnumerable<MerchId> MerchIds,
        NotDefaultDateTime Created,
        IEnumerable<FileEvent>? Events
        ) : IEntity<FileId>
    {
        //public FileInfo AddMerchId(MerchId merchId)
        //{
        //    var uf = UserFiles.First();
        //    var merchs = uf.merchs.Append(merchId).Distinct().ToList();
        //    uf = uf with { merchs = merchs };
        //    var ufs = UserFiles
        //          .Where(u => u.UserId != uf.UserId && u.file != uf.file)
        //          .Append(uf)
        //          .ToList()
        //          .AsReadOnly();
        //    var events = Events.Append(new MerchIdAddedeToUserFile(merchId)).ToList();
        //    return this with { Events = events, UserFiles = ufs };
        //}
        public FileName GetDefaultName() => $"{Hash.GetHexString().Value}{Extention.Value}";
        public static FileName GetDefaultName(FileHash hash, FileExtention extention) => $"{hash.GetHexString().Value}{extention.Value}";
    }
}
