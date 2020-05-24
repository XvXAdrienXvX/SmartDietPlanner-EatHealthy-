var nodeId, id, protein, carb, fat, cal, serving, image, pref_ID;
var proposed = false;
var proposedString = ""; var loadedString = "";
//Get mealTime of "Add Food" button (breakfast,lunch...)
function Function(Node) {
    nodeId = Node;

}

function removeAllCard() {
    var StringSplit = proposedString.split(";");
    StringSplit.forEach(function (sub) {
        var Stringsub = sub.split(",");
        Stringsub.forEach(function (subID) {
            //$("#" + subID).closest("div").hide();
            removeThis(subID);
        });
    });

    var StringSplit2 = loadedString.split(";");
    StringSplit2.forEach(function (sub2) {
        var Stringsub2 = sub2.split(",");
        Stringsub2.forEach(function (subID2) {
          //  $("#" + subID2).closest("div").hide();
            removeThis(subID2);
        });
    });
}
function removeThis(em) {

 $.ajax(
            {
                type: "GET",
                url: "/User/removeFoods",

                data: {
                    'food': em

                },
                dataType: 'json',
                cache: false,
         success: function (data) {
             removeFromString(em);

                    $("#" + em).closest("div").remove();
                    var ca = 0, cal = 0, p = 0, f = 0;
                    $.map(data, function (item) {
                        ca += item.ncarb;
                        cal += item.ncalorie;
                        p += item.nprotein;
                        f += item.nfat;

                    });
                    $("#Carb span").html(" " + ca.toFixed(1) + " g");
                    $("#Cal span").html(" " + cal.toFixed(1) + " Kcal");
                    $("#Protein span").html(" " + p.toFixed(1) + " g");
                    $("#Fat span").html(" " + f.toFixed(1) + " g");

                }

            });
}

function removeFromString(emID) {
    var temporayString = "";
    if (proposedString !== "") {
        var StringSplit = proposedString.split(";");
        StringSplit.forEach(function (sub) {
            if (sub.includes(emID)) {
                temporayString += "";
            }
            else {
                if (sub !== "") {
                    temporayString += sub + ";";
                }
            }

        });
        proposedString = temporayString;
    }
}

$(document).ready(function () {
    var totalCarb = 0, totalCal = 0, totalP = 0, totalF = 0;
    var clicked = false;
    //Ajax call to load latest diet plan from database
    $.ajax(
        {
            type: "GET",
            url: "/User/GetDiet",
            dataType: 'json',
            cache: false,
            success: function (data) {

                $.map(data, function (item) {
                    loadedString += item.food + "," + item.meal_time + ";";

    //                alert(item.food + "\n" + item.calorie);
                    totalCarb += Number(item.carb);
                    totalCal += Number(item.calorie);
                    totalP += Number(item.protein);
                    totalF += Number(item.fat);
                    var elmnt1 = document.getElementById(item.food);
                    var cln1 = elmnt1.cloneNode(true);
                    addCross(cln1);
                    document.getElementById("flex" + " " + item.meal_time).appendChild(cln1);
                });
                $("#Carb span").html(" " + totalCarb.toFixed(1) + " g");
                $("#Cal span").html(" " + totalCal.toFixed(1) + " Kcal");
                $("#Protein span").html(" " + totalP.toFixed(1) + " g");
                $("#Fat span").html(" " +  totalF.toFixed(1) + " g");
            }

        });

    /*Display Apriori Results */
    //$.ajax(
    //   {
    //       type: "GET",
    //       url: "/User/Apriori",
    //       dataType: 'json',
    //       cache: false,
    //       success: function (data) {
    //           var i = 0;
    //           $.map(data, function (item) {
    //               i++;

    //               var elmnt1 = document.getElementById(item.food.replace(/\s/g, ''));
    //               var cln1 = elmnt1.cloneNode(true);
    //               addCross(cln1);
    //               if (item.fgroup===1) {
    //                   document.getElementById("flex" + " " + "Breakfast").appendChild(cln1);
    //                   totalCarb += item.ncarb;
    //                   totalCal += item.ncalorie;
    //                   totalP += item.nprotein;
    //                   totalF += item.nfat;
    //               }
    //               if (item.fgroup === 2) {
    //                   document.getElementById("flex" + " " + "Lunch").appendChild(cln1);
    //                   totalCarb += item.ncarb;
    //                   totalCal += item.ncalorie;
    //                   totalP += item.nprotein;
    //                   totalF += item.nfat;
    //               }

    //           });
    //           $("#Carb span").html(" " + totalCarb.toFixed(1) + " g");
    //           $("#Cal span").html(" " + totalCal.toFixed(1) + " Kcal");
    //           $("#Protein span").html(" " + totalP.toFixed(1) + " g");
    //           $("#Fat span").html(" " +  totalF.toFixed(1) + " g");
    //       }

    //   });

    /* Calculate total amount of macronutrients for foods in preferences
      Display foods in preferences
    */

   


    $("#propose").click(function () {
        //if (loadedString !== "" || proposedString !== "") {
        //    removeAllCard();
        //}
        if (proposed !== true) {
            if (!clicked) {

                $.ajax(
                    {
                        type: "GET",
                        url: "/User/returnPropose",
                        dataType: 'json',
                        success: function (data) {
                            for (var c in data) {
                                if (data.hasOwnProperty(c) && c === "Breakfast") {
                                    $.map(data[c], function (item) {

                                        var el = document.getElementById(item.food.replace(/\s/g, ''));
                                        var cl = el.cloneNode(true);
                                        var idDiv = item.food.replace(/\s/g, '');
                                        addCross(cl);
                                        proposedString += idDiv + ",Breakfast;";
                                        Increment(cl, idDiv);
                                        document.getElementById("flex" + " " + "Breakfast").appendChild(cl);

                                    });
                                }
                                else if (data.hasOwnProperty(c) && c === "Lunch") {
                                    $.map(data[c], function (item) {

                                        var el = document.getElementById(item.food.replace(/\s/g, ''));
                                        var cl = el.cloneNode(true);
                                        var idDiv = item.food.replace(/\s/g, '');
                                        addCross(cl);
                                        proposedString += idDiv + ",Lunch;";
                                        Increment(cl, idDiv);
                                        document.getElementById("flex" + " " + "Lunch").appendChild(cl);
                                    });
                                }
                                else if (data.hasOwnProperty(c) && c === "Dinner") {
                                    $.map(data[c], function (item) {

                                        var el = document.getElementById(item.food.replace(/\s/g, ''));
                                        var cl = el.cloneNode(true);
                                        var idDiv = item.food.replace(/\s/g, '');
                                        addCross(cl);
                                        proposedString += idDiv + ",Dinner;";
                                        Increment(cl, idDiv);
                                        document.getElementById("flex" + " " + "Dinner").appendChild(cl);
                                    });
                                }

                            }
                            proposed = true;
                        }


                    });
                clicked = true;
            }

            else {
                var d = document.getElementsByClassName('.myDiv');
                for (var i = 0; i < d.length; i++) {
                    alert(d[i].getAttribute('id'));
                }
                clicked = false;
            }

        }
      
   
        
    })
    

  
    /* Increment macronutrients for ajax call get foods preferences returnPropose */
    function Increment(cl, idDiv) {
        id = idDiv;
        protein = $(cl).find('span')[1].innerHTML;
        carb = $(cl).find('span')[2].innerHTML;
        fat = $(cl).find('span')[3].innerHTML;
        cal = $(cl).find('span')[4].innerHTML;
        serving = $(cl).find('span')[5].innerHTML;
        image = $(cl).find('img')[0].src;
        pref_ID = $(cl).find('input')[0].value;
        $.ajax(
            {
                type: "GET",
                url: "/User/getFoods",

                data: {
                    'food': id,
                    'protein': protein,
                    'carb': carb,
                    'fat': fat,
                    'cal': cal,
                    'serving': serving,
                    'image': image,
                    'mealTime': nodeId,
                    'prefID': pref_ID
                },
                dataType: 'json',
                cache: false,
                success: function (data) {


                    $.map(data, function (item) {
                        totalCarb += item.ncarb;
                        totalCal += item.ncalorie;
                        totalP += item.nprotein;
                        totalF += item.nfat;

                        var el = document.getElementById(item.food);
                        var cl = el.cloneNode(true);



                    });
                    $("#Carb span").html(" " + totalCarb.toFixed(1) + " g");
                    $("#Cal span").html(" " + totalCal.toFixed(1) + " Kcal");
                    $("#Protein span").html(" " + totalP.toFixed(1) + " g");
                    $("#Fat span").html(" " + totalF.toFixed(1) + " g");

                }

            });
    }
    //$.ajax(
    //    {
    //        type: "GET",
    //        url: "/User/LoadFoods",
    //        dataType: 'json',
    //        cache: false,
    //        success: function (data) {
               
    //            $.map(data, function (item) {
                    
    //                totalCarb += item.ncarb;
    //                totalCal += item.ncalorie;
    //                totalP += item.nprotein;
    //                totalF += item.nfat;
    //                var elmnt1 = document.getElementById(item.food);
    //                var cln1 = elmnt1.cloneNode(true);
    //                addCross(cln1);
    //                document.getElementById("flex" + " " + item.meal_time).appendChild(cln1);
    //            });
    //            $("#Carb span").html(" " + totalCarb.toFixed(1) + " g");
    //            $("#Cal span").html(" " + totalCal.toFixed(1) + " Kcal");
    //            $("#Protein span").html(" " + totalP.toFixed(1) + " g");
    //            $("#Fat span").html(" " + totalF.toFixed(1) + " g");
    //        }

    //    });
   
    //Ajax call to save current diet plan
    $("#Save").click(function () {

        $.ajax(
            {
                type: "GET",
                url: "/User/SaveDiet",

                data: {


                    'food': proposedString
                },
                dataType: 'json',
                cache: false,
                success: function (data) {
                    //alert("Saved");
                }

            });
        $("#exampleModalCenter").modal("hide");
    });

    //Add food to diet plan when clicked from modal "Add Food"
    $(".myDiv").click(function () {

        id = this.id;
        protein = this.getElementsByTagName('span')[1].innerHTML;
        carb = this.getElementsByTagName('span')[2].innerHTML;
        fat = this.getElementsByTagName('span')[3].innerHTML;
        cal = this.getElementsByTagName('span')[4].innerHTML;
        serving = this.getElementsByTagName('span')[5].innerHTML;
        image = this.getElementsByTagName('img')[0].src;
        pref_ID = this.getElementsByTagName("input")[0].value;
        //alert(pref_ID);
        //this.getElementsByTagName('span')[0].style.visibility = "visible";
        $('#myModal').modal('hide');

        proposedString += id + "," + nodeId + ";";
        

        /*
         Ajax call to add all foods added by user from modal to an array
         Return array and count calorie,macronutrients per food
         Display total calorie and macronutrients in respective divs
        */
        $.ajax(
            {
                type: "GET",
                url: "/User/getFoods",

                data: {
                    'food': id,
                    'protein': protein,
                    'carb': carb,
                    'fat': fat,
                    'cal': cal,
                    'serving': serving,
                    'image': image,
                    'mealTime': nodeId,
                    'prefID': pref_ID
                },
                dataType: 'json',
                cache: false,
                success: function (data) {
                   

                    $.map(data, function (item) {
                        totalCarb += item.ncarb;
                        totalCal += item.ncalorie;
                        totalP += item.nprotein;
                        totalF += item.nfat;

                        var elmnt1 = document.getElementById(item.food);
                        var cln1 = elmnt1.cloneNode(true);

                        addCross(cln1);

                        document.getElementById("flex" + " " + item.meal_time).appendChild(cln1);

                        if (totalCal > calorie) {
                            //find food with highest calorie and remove/replace with another food
                            var dict = {};
                            var a = document.getElementById("containerFood").querySelectorAll("p#cal span");
                            var d = document.getElementById("containerFood").getElementsByClassName("myDiv");
                          
                            for (var i = 0; i < d.length; i++) {
                                for (var j = 0; j < a.length; j++) {
                                    if (i === j) {
                                        dict[$(d[i]).attr('id')] = parseFloat(a[j].innerHTML);
                                    }
                                }
                            }
                            
                            var largest = Math.max.apply(null, Object.values(dict));
                            var f = Object.keys(dict).find(key => dict[key] === largest);
                          
                            /*Display modal with foods with lowest calorie/macros*/
                            adjust(f);
                            generate(f);
                           
                            //alert(largest + ":" +f);
                            //alert(JSON.stringify(dict));
                        }

                    });
                    $("#Carb span").html(" " + totalCarb.toFixed(1) + " g");
                    $("#Cal span").html(" " + totalCal.toFixed(1) + " Kcal");
                    $("#Protein span").html(" " + totalP.toFixed(1) + " g");
                    $("#Fat span").html(" " + totalF.toFixed(1) + " g");
                    //alert(totalCarb);

                }

            });

    });

    

    //Ajax call to Remove the food with the highest amount of calorie or macronutrient from diet plan
    function adjust(f) {
        var el = $("#" + f);

        $.ajax(
            {
                type: "GET",
                url: "/User/removeFoods",
             
                data: {
                    'food': f

                },
                dataType: 'json',
                cache: false,
                success: function (data) {
                    $(el).remove();
                    var ca = 0, cal = 0, p = 0, f = 0;
                    $.map(data, function (item) {
                        ca += item.ncarb;
                        cal += item.ncalorie;
                        p += item.nprotein;
                        f += item.nfat;

                    });
                    $("#Carb span").html(" " + ca.toFixed(1) + " g");
                    $("#Cal span").html(" " + cal.toFixed(1) + " Kcal");
                    $("#Protein span").html(" " + p.toFixed(1) + " g");
                    $("#Fat span").html(" " + f.toFixed(1) + " g");

                }

            });
    }

    //Ajax call to Add food with least amount of calorie or macronutrient to diet plan
    function generate(f) {
        $.ajax(
            {
                type: "GET",
                url: "/User/Generate",

                data: {
                    'food': f,
                    'protein': protein,
                    'carb': carb,
                    'fat': fat,
                    'cal': cal,
                    'serving': serving,
                    'image': image,
                    'mealTime': nodeId
                },
                dataType: 'json',
                cache: false,
                success: function (data) {

                    $.map(data, function (item) {
                    var idDiv = item.food.replace(/\s/g, '');

                    var elmnt = document.getElementById(idDiv);
                        var cln = elmnt.cloneNode(true);
                        $(cln).find('div')[0].onclick = function () {
                            Add(cln,idDiv);
                            $('#Adjust').modal('hide');
                        };
                    
                    document.getElementById("modal-body").appendChild(cln);
                    $('#Adjust').modal('show');
                     

                      
                    });

                }

            });
    }

  

    //Add "cross" icon and remove food functionality
    function addCross(cl) {
        $(cl).find('span')[0].style.visibility="visible";

        var cross = $(cl).find('span')[0];
          

    }
    function Add(cln,idDiv) {
        id = idDiv;
        protein = $(cln).find('span')[1].innerHTML;
        carb = $(cln).find('span')[2].innerHTML;
        fat = $(cln).find('span')[3].innerHTML;
        cal = $(cln).find('span')[4].innerHTML;
        serving = $(cln).find('span')[5].innerHTML;
        image = $(cln).find('img')[0].src;
        pref_ID = $(cln).find('input')[0].value;
        $.ajax(
            {
                type: "GET",
                url: "/User/getFoods",

                data: {
                    'food': id,
                    'protein': protein,
                    'carb': carb,
                    'fat': fat,
                    'cal': cal,
                    'serving': serving,
                    'image': image,
                    'mealTime': nodeId,
                    'prefID': pref_ID
                },
                dataType: 'json',
                cache: false,
                success: function (data) {


                    $.map(data, function (item) {
                        totalCarb += item.ncarb;
                        totalCal += item.ncalorie;
                        totalP += item.nprotein;
                        totalF += item.nfat;

                        var el = document.getElementById(item.food);
                        var cl = el.cloneNode(true);

                        addCross(cl);

                        document.getElementById("flex" + " " + item.meal_time).appendChild(cl);

                     

                    });
                    $("#Carb span").html(" " + totalCarb.toFixed(1) + " g");
                    $("#Cal span").html(" " + totalCal.toFixed(1) + " Kcal");
                    $("#Protein span").html(" " + totalP.toFixed(1) + " g");
                    $("#Fat span").html(" " + totalF.toFixed(1) + " g");

                }

            });
    }
   
});

function proposedMeal(type) {
    //alert(type);
    //$.ajax(
    //    {
    //        type: "GET",
    //        url: "/User/proposingFood",
    //        data: {
    //            'AllocatedProtein': AllocatedProtein,
    //            'AllocatedCarb': AllocatedCarb,
    //            'AllocatedFat': AllocatedFat,
    //            'AllocatedCalorie': AllocatedCalorie

    //        },
    //        success: function (data) {
    //            $.map(data, function (item) {
    //                //totalCarb += item.ncarb;
    //                //totalCal += item.ncalorie;
    //                //totalP += item.nprotein;
    //                //totalF += item.nfat;

    //                var el = document.getElementById(item.replace(/\s/g, ''));
    //                var cl = el.cloneNode(true);

    //                addCross(cl);

    //                document.getElementById("flex" + " " + "Breakfast").appendChild(cl);



    //            });
    //        }
           

    //    });

    

}
/* Calculate total amount of macronutrients for foods in preferences
   Display foods in preferences
 */
function Preferences()
{
     //$.ajax(
     //   {
     //       type: "GET",
     //        url: "/User/returnPropose",
     //        dataType: 'json',
     //       success: function (data) {
     //           for (var c in data) {
     //               if (data.hasOwnProperty(c) && c=== "Breakfast") {
     //                   $.map(data[c], function (item) {
     //                       //totalCarb += item.ncarb;
     //                       //totalCal += item.ncalorie;
     //                       //totalP += item.nprotein;
     //                       //totalF += item.nfat;

     //                       var el = document.getElementById(item.food.replace(/\s/g, ''));
     //                       var cl = el.cloneNode(true);

     //                       addCross(cl);

     //                       document.getElementById("flex" + " " + "Breakfast").appendChild(cl);



     //                   });
     //               }
     //           }
     //       }


     //   });
}
