namespace MIB_FILM_CLD_INT.Infrastructure
{
    public static class LegacyHtmlPage
    {
        public static string Create(string title)
        {
            return $$"""
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>{{title}}</title>
</head>
<body>
    <h1>INTEGRATION DONE.</h1>
</body>
</html>
""";
        }
    }
}
