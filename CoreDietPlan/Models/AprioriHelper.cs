using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;

namespace CoreDietPlan.Models
{
    public static class AprioriHelper
    {
        public static HtmlString FoodDiv(Meal items)
        {

            StringBuilder sb = new StringBuilder();
            string trim = items.food.Replace(" ", string.Empty);

            sb.Append($"  <div class=\"col-lg-3 col-md-3 col-sm-3 col-xs-12 myDiv\" id={trim} style=\"cursor:pointer;margin-top:5%;\">");
            sb.Append($"                                                <div class=\"card card-product\" style=\"border:1px solid lightgrey;border-radius:3px;height:100%;width:100%;\">");
            sb.Append($"                                                    <span  id={trim} style=\"visibility:hidden;\" class=\"pull-right clickable\" data-effect=\"fadeOut\"><i class=\"fa fa-times\"></i></span>");
            sb.Append($"                                                    <div class=\"img-wrap\" id={items.img} style=\"border-radius: 3px 3px 0 0;overflow: hidden;position: relative;height:20vh;text-align: center;\"><img style=\"display:block;margin:auto;width:100%;height:100%;object-fit:contain;\" src={items.img} class=\"img-responsive\"></div>");
            sb.Append($"                                                    <figcaption class=\"info-wrap\" style=\"overflow: hidden;padding: 15px;border-top: 1px solid #eee;height:100%;\">");
            sb.Append($"                                                        <h3 readonly class=\"text-center\" id=\"food\">{items.food}</h3>");
            sb.Append($"                                                        <p readonly class=\"text-center\" id=\"protein\">Protein:<span>{items.protein}</span></p>");
            sb.Append($"                                                        <p class=\"text-center\" id=\"carb\">Carbs:<span>{items.carb}</span></p>");
            sb.Append($"                                                        <p class=\"text-center\" id=\"fat\">Fat:<span>{items.fat}</span></p>");
            sb.Append($"                                                        <p class=\"text-center\" id=\"cal\">Calorie:<span>{items.calorie}</span></p>");
            sb.Append($"                                                        <p class=\"text-center\">Serving:{items.servingQty} <span>{items.serving}</span></p>");
            sb.Append($"                                                        <input type=\"hidden\" id=\"prefid\" name={items.PrefID} value={items.PrefID}/>");
            sb.Append($"                                                        <div class=\"rating-wrap\">");
            sb.Append($"                                                            <div class=\"label-rating\"></div>");
            sb.Append($"                                                            <div class=\"label-rating\"></div>");
            sb.Append($"                                                        </div>");
            sb.Append($"                                                    </figcaption>");
            sb.Append($"                                                    <div class=\"bottom-wrap\">");
            sb.Append($"                                                        <div class=\"price-wrap h5\">");
            sb.Append($"                                                        </div>");
            sb.Append($"                                                    </div>");
            sb.Append($"                                                </div>");
            sb.Append($"                                            </div>");


            return new HtmlString(sb.ToString());
        }
    }
}
