//var brands = brands || {};

//brands.funcCheckRow = function () {
//    var days;
//    days += daysOfMonth;
//    return days;
//}


//$(document).ready(function () {
//    var temp = $("tr.calendar-row td");
//    brands.funcCheckRow();
//});

$("tr.calendar-row td").click(function (e) {//   //function_td
    var date = $(this).data('value'); 
    var origin = window.location.origin;   // Returns base URL (https://example.com)
    if (origin != undefined && origin != null) {
        window.location = origin + '/ManhourInput/Day?dateSt=' + date;
    }
});

//$("tr.calendar-row td").each(function (index) {
//    alert(index);
  
//});

