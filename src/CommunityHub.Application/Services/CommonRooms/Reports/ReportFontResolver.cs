using PdfSharp.Fonts;
using System.Reflection;

namespace CommunityHub.Application.Services.CommonRooms.Reports
{
    public class ReportFontResolver : IFontResolver
    {
        public const string FontFamilyName = "Google Sans";
        private const string RegularFaceName = "GoogleSans#Regular";
        private const string EmbeddedFontResourceName = "CommunityHub.Application.Resources.Fonts.GoogleSans.ttf";

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
            var fontPath = GetOutputFontPath();
            if (File.Exists(fontPath))
                return File.ReadAllBytes(fontPath);

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(EmbeddedFontResourceName);

            if (stream is null)
                throw new FileNotFoundException(
                    "Report font file was not found as an output file or embedded resource.",
                    fontPath
                );

            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        private static string GetOutputFontPath()
        {
            return Path.Combine(
                AppContext.BaseDirectory,
                "Resources",
                "Fonts",
                "GoogleSans.ttf"
            );
        }
    }
}
