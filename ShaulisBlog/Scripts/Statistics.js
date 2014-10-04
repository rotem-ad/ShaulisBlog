
var svg;


var Statistics = {
    //flag to determine if graph has already been generated
    HaveGraph: new Array(),

    //turn on or off relevan statistics details
    ShowHideDetails: function (itemId) {
        var detailsId = '#' +itemId + 'Result'; //calc id of result div
        if ($(detailsId).css('display') == 'block') //if item is visible
            $(detailsId).hide();
        else {
            $(detailsId).show();
            
            //make ajax call to update the most recent statistics data
            var url = "/Statistics/ReGenerateStatisticFiles";
            $.get(url, 
                null, //here goes the params if we intend to pass params to function in format: { paramName: data } 
                function (data) {
                    if(data == "OK") //if data was updated successfully --> show it
                     {
                        Statistics.DisplayGraph(itemId + 'Result');
                      }
                    });

            }
          
        
    },

    //generate graph to show results
    DisplayGraph: function (reportName) {
        if (Statistics.HaveGraph == null || Statistics.HaveGraph[reportName] == null) {
            svg = d3.select('#' + reportName).append("svg")
                       .attr("width", width + margin.left + margin.right)
                       .attr("height", height + margin.top + margin.bottom)
                       .append("g")
                       .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


            d3.tsv(reportName + ".tsv",
                function (d) {
                    d.Count = +d.Count;
                    return d;
                },
                function (error, data) {
                    x.domain(data.map(function (d) { return d.Item; }));
                    y.domain([0, d3.max(data, function (d) { return d.Count; })]);

                    svg.append("g")
                        .attr("class", "x axis")
                        .attr("transform", "translate(0," + height + ")")
                        .call(xAxis);

                    svg.append("g")
                        .attr("class", "y axis")
                        .call(yAxis)
                      .append("text")
                        .attr("transform", "rotate(-90)")
                        .attr("y", 6)
                        .attr("dy", ".71em")
                        .style("text-anchor", "end")
                        .text("Count"); //CHANGE!!!!!!!!!!

                    svg.selectAll(".bar")
                        .data(data)
                      .enter().append("rect")
                        .attr("class", "bar")
                        .attr("x", function (d) { return x(d.Item); })
                        .attr("width", x.rangeBand())
                        .attr("y", function (d) { return y(d.Count); })
                        .attr("height", function (d) { return height - y(d.Count); });

                });

            Statistics.HaveGraph[reportName] = true;
        }
    }
 }


 var margin = {top: 20, right: 20, bottom: 30, left: 40},
            width = 600 - margin.left - margin.right,
            height = 400 - margin.top - margin.bottom;

        var x = d3.scale.ordinal()
            .rangeRoundBands([0, width], .1);

        var y = d3.scale.linear()
            .range([height, 0]);

        var xAxis = d3.svg.axis()
            .scale(x)
            .orient("bottom");

        var yAxis = d3.svg.axis()
            .scale(y)
            .orient("left")
            .ticks(10, "");
       

