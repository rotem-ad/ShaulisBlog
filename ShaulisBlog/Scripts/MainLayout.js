$(document).ready(function (e) {
    //start update weather data loop
    window.setInterval(MainLayout.GetWeatherReport, MainLayout.Timeout); // repeat forever, polling every #Timeout
    MainLayout.GetWeatherReport();



});




//$(document).ready(function () {
//    $("nav > ul > li > a").click(function () {
//        $(this).parent().removeClass("selected");
//        $(this).parnet().addClass("selected");
//    });
//})

////$(document).ready(function () {
////    if (typeof selected != 'undefined') {
////        $(selected).closest('li').addClass('selected');
////    }
////    $("nav ul li:"+ selcted).addClass('selected');
    
////    //if (typeof crap != 'undefined') {
////    //    $(crap).closest('li').addClass('selected');
////    //}
////    //$('nav ul li').on('click', changeClass);
////});

////function changeClass() {
////    $('nav ul li').removeClass('selected');
////    $(this).closest('li').addClass('selected');
////    if (crap == null) {
////        var crap = this;
////    }    
////}

////$(document).ready(function () {
////    $('nav ul li').on('click', changeClass);
////});

////function changeClass() {
////    $('nav ul li').removeClass('selected');
////    $(this).closest('li').addClass('selected');
////}

function Selected(data) {
    $('nav ul li').removeClass('selected');
    $("nav ul " + data).addClass('selected');
}






var MainLayout = {
    //OpenWeatherMap service link
    WeatherServiceUrl: "http://api.openweathermap.org/data/2.5/group",

    Timeout: 10000, //10 seconds - update weather data time interval

    //list of ids we can get from: http://openweathermap.org/help/city_list.txt. 
    //293397-Tel Aviv,295277-Eilat,281184-Jerusalem,294801-Haifa,293322-Tiberias
    Cities: {id: '293397,295277,281184,294801,293322',units: 'metric'},


    //get weather report for service
    GetWeatherReport: function () {
        $.get(MainLayout.WeatherServiceUrl,
                MainLayout.Cities,
                MainLayout.OnGetWeatherReportSuccess).fail(MainLayout.OnGetWeatherReportError);
    },

    OnGetWeatherReportSuccess: function (dataList) {
        var innerHtml = "";
        $('#weatherDiv').html(innerHtml);
        //generate weather data content
        if (dataList != null && dataList.cnt != null)
        {
            for (var i = 0; i < dataList.cnt; i++) {
                var data = dataList.list[i];
                innerHtml += '<b>' + data.name +
                        '</b><img class=\'weatherImg\' src=\'http://openweathermap.org/img/w/' + data.weather[0].icon + '.png\'/><div class=\'weatherTemp\'>' +
                        data.main.temp_min + '-' + data.main.temp_max + '</div><br/>';
            }
            var currDate = new Date();
            innerHtml += '<br/>' + currDate.toLocaleString()
        }
        $('#weatherDiv').html(innerHtml);
    },

    OnGetWeatherReportError: function (ex) {
       //report to log
    }

}


