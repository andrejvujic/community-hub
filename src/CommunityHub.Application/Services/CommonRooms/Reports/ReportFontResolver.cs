using PdfSharp.Fonts;

namespace CommunityHub.Application.Services.CommonRooms.Reports
{
    public class ReportFontResolver : IFontResolver
    {
        public const string FontFamilyName = "Google Sans";
        private const string RegularFaceName = "GoogleSans#Regular";

        private readonly Lazy<byte[]> _regularFontBytes = new(LoadRegularFont);

        public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
        {
            return new FontResolverInfo(
                RegularFaceName,
                mustSimulateBold: bold,
                mustSimulateItalic: italic
            );
        }

        public byte[]? GetFont(string faceName)
        {
            return faceName == RegularFaceName
                ? _regularFontBytes.Value
                : null;
        }

        private static byte[] LoadRegularFont()
        {
            var fontPath = Path.Combine(
                AppContext.BaseDirectory,
                "Resources",
                "Fonts",
                "GoogleSans.ttf"
            );

            if (!File.Exists(fontPath))
                throw new FileNotFoundException("Report font file was not found.", fontPath);

            return File.ReadAllBytes(fontPath);
        }
    }
}
