using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

public static class MyHtml
{
    public static IHtmlContent BackButton(this IHtmlHelper html, string url = "back")
    {
        var jsact = "window.location.href ='" + url + "'; return false;";
        return new HtmlString(
            $"<button class=\"btn btn-primary\" onclick=\"{jsact}\">Back</button>");
    }

    public static IHtmlContent WebAlert(this IHtmlHelper html, string message)
    {
        return new HtmlString($"<script>alert('{message}');</script>");
    }
}
