using System.IO;
using NUnit.Framework;

namespace InformantTest.Implementation; 

public static class TestUtils {
    public static readonly string ModFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(
        TestContext.CurrentContext.TestDirectory)))! // Informant.Test/bin/Debug/net5.0
        .Replace("Informant.Test", "Informant");
}