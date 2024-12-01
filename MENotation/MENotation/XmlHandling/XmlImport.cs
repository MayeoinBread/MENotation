using System.IO;
using System.IO.Compression;
using System.Xml;

namespace MENotation.XmlHandling
{
    internal class XmlImport
    {
        public static XmlDocument ImportXml()
        {
            XmlDocument scoreXmlData;
            var filePath = "C:\\Users\\eoin2\\OneDrive\\Music\\batb.mxl";
            //var filePath = "C:\\Users\\eoin2\\Documents\\test.mxl";
            //var filePath = "C:\\Users\\eoin2\\Documents\\atm.mxl";
            if (File.Exists(filePath))
            {
                if (filePath.EndsWith(".mxl"))
                {
                    // Need to unzip as it's the compressed format
                    scoreXmlData = UnzipCompressedMxl(filePath);
                }
                else
                {
                    scoreXmlData = new XmlDocument
                    {
                        XmlResolver = null
                    };
                    scoreXmlData.Load(filePath);
                }
            }
            else
            {
                scoreXmlData = new XmlDocument();
            }

            return scoreXmlData;
        }



        private static XmlDocument UnzipCompressedMxl(string filePath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(filePath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (!entry.FullName.Contains("META-INF"))
                    {
                        string destinationPath = Path.GetFullPath(Path.Combine("C:\\Users\\eoin2\\Documents", entry.FullName));
                        entry.ExtractToFile(destinationPath, true);
                        XmlDocument doc = new()
                        {
                            XmlResolver = null
                        };
                        doc.Load(entry.Open());
                        return doc;
                    }
                }
                return null;
            }
        }
    }
}
