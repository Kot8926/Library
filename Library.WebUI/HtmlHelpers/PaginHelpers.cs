using System;
using System.Text;
using System.Web.Mvc;
using Library.WebUI.Models;

namespace Library.WebUI.HtmlHelpers
{
    public static class PaginHelpers
    {
        //Формирует набор ссылок html на основе модели представления PageInfo
        public static  MvcHtmlString PageLinks(
            this HtmlHelper html,
            PagingInfo pageInfo,
            Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            //Для всех страниц pageInfo.TotalPages
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                //Строим html тег
                TagBuilder tag = new TagBuilder("a");
                //Добавляет атрибут
                tag.MergeAttribute("href", pageUrl(i));
                //Указываем значение
                tag.InnerHtml = i.ToString();
                //Если выбрана текущая страница
                if (i == pageInfo.CurrentPage)
                    tag.AddCssClass("selected");
                result.Append(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        } 
    }
}