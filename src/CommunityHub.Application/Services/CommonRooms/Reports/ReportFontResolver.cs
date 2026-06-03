using PdfSharp.Fonts;
using System.Reflection;

namespace CommunityHub.Application.Services.CommonRooms.Reports
{
    public class ReportFontResolver : IFontResolver
    {
        public const string FontFamilyName = "GoogleSans";
        private const string FontFaceName = "GoogleSans#Regular";
        private const string FontResourceName = "CommunityHub.Application.Resources.Fonts.GoogleSans.ttf";

        private readonly byte[] _fontBytes = LoadFont();

        public FontResolverInfo? ResolveTypeface(string familyName, bool mustSimulateBold, bool mustSimulateItalic)
        {
            return new FontResolverInfo(
                FontFaceName,
                mustSimulateBold: mustSimulateBold,
                mustSimulateItalic: mustSimulateItalic
            );
        }

        public byte[]? GetFont(string faceName)
        {
            return faceName == FontFaceName
                ? _fontBytes
                : null;
        }

        private static byte[] LoadFont()
        {
            var fontPath = GetFontPath();
            if (File.Exists(fontPath))
                return File.ReadAllBytes(fontPath);

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(FontResourceName);

            if (stream is null)
                throw new FileNotFoundException(
                    "Report font file was not found as an output file or embedded resource.",
                    fontPath
                );

            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        private static string GetFontPath()
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
