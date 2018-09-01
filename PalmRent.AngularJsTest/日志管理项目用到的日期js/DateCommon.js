//把字符串日期转为日期
function convertStrToDate(datetimeStr) {
    var mydateint = Date.parse(datetimeStr);//数值格式的时间
    if (!isNaN(mydateint)) {
        var mydate = new Date(mydateint);
        return mydate;
    }
    var mydate = new Date(datetimeStr);//字符串格式时间
    var monthstr = mydate.getMonth() + 1;
    if (!isNaN(monthstr)) {//转化成功
        return mydate;
    }//字符串格式时间转化失败
    var dateParts = datetimeStr.split(" ");
    var dateToday = new Date();
    var year = dateToday.getFullYear();
    var month = dateToday.getMonth();
    var day = dateToday.getDate();
    if (dateParts.length >= 1) {
        var dataPart = dateParts[0].split("-");//yyyy-mm-dd  格式时间             
        if (dataPart.length == 1) {
            dataPart = dateParts[0].split("/");//yyyy/mm/dd格式时间
        }
        if (dataPart.length == 3) {
            year = Math.floor(dataPart[0]);
            month = Math.floor(dataPart[1]) - 1;
            day = Math.floor(dataPart[2]);
        }
    }
    if (dateParts.length == 2) {//hh:mm:ss格式时间
        var timePart = dateParts[1].split(":");//hh:mm:ss格式时间
        if (timePart.length == 3) {
            var hour = Math.floor(timePart[0]);
            var minute = Math.floor(timePart[1]);
            var second = Math.floor(timePart[2]);
            return new Date(year, month, day, hour, minute, second);
        }
    }
    else {
        return new Date(year, month, day);
    }
}

function Common_GetWeekDate(time) {
    var now = new Date(time); //当前日期 
    this.nowDayOfWeek = now.getDay(); //今天本周的第几天
    this.nowYear = now.getYear(); //当前年 
    this.nowMonth = now.getMonth(); //月 
    this.nowDay = now.getDate(); //日 
    this.beginHour = "09:00:00";
    this.endHour = "23:59:59";

    this.nowYear += (this.nowYear < 2000) ? 1900 : 0; //
    this.nowDayOfWeek = this.nowDayOfWeek == 0 ? 7 : this.nowDayOfWeek; // 如果是周日，就变成周七
}

Common_GetWeekDate.prototype.formatDate = function (date) {//格局化日期：yyyy-MM-dd 
    var myyear = date.getFullYear();
    var mymonth = date.getMonth() + 1;
    var myweekday = date.getDate();
    //alert("formatDate"+myyear+":"+mymonth+":"+myweekday)
    if (mymonth < 10) {
        mymonth = "0" + mymonth;
    }
    if (myweekday < 10) {
        myweekday = "0" + myweekday;
    }
    return (myyear + "-" + mymonth + "-" + myweekday);
}

Common_GetWeekDate.prototype.getWeekStartDate = function () { //获得本周的开端日期
    var weekStartDate = new Date(this.nowYear, this.nowMonth, this.nowDay - this.nowDayOfWeek + 1);
    return this.formatDate(weekStartDate);
}

Common_GetWeekDate.prototype.getWeekEndDate = function () {//获得本周的停止日期
    var weekEndDate = new Date(this.nowYear, this.nowMonth, this.nowDay + (6 - this.nowDayOfWeek + 1));
    return this.formatDate(weekEndDate);
}

Common_GetWeekDate.prototype.getAWeedkYMD = function () {//获得本周周一~周日的年月日
    var ymdArr = [];
    for (var i = 0; i < 7; i++) {
        ymdArr[i] = [];
        //ymdArr[i][0]=this.formatDate(new Date(this.nowYear, this.nowMonth, this.nowDay - this.nowDayOfWeek+i+1));
        ymdArr[i][0] = this.date2str(new Date(this.nowYear, this.nowMonth, this.nowDay - this.nowDayOfWeek + i + 1), 'yyyy-MM-dd');
        ymdArr[i][1] = this.date2str(new Date(this.nowYear, this.nowMonth, this.nowDay - this.nowDayOfWeek + i + 1), 'MM月dd日');
    };

    return ymdArr;
}

Common_GetWeekDate.prototype.date2str = function (x, y) {//date2str(new Date(curTime),"yyyy-MM-dd")
    var z = { y: x.getFullYear(), M: x.getMonth() + 1, d: x.getDate(), h: x.getHours(), m: x.getMinutes(), s: x.getSeconds() };
    return y.replace(/(y+|M+|d+|h+|m+|s+)/g, function (v) { return ((v.length > 1 ? "0" : "") + eval('z.' + v.slice(-1))).slice(-(v.length > 2 ? v.length : 2)) });
}


// var date = new Date(2006,0,12,22,19,35); 
// console.log(date);
// console.log(getdateInstance.getWeekStartDate());
// console.log(getdateInstance.getWeekEndDate());
// console.log(getdateInstance.getAWeedkYMD());


function getThisWeekDateRange(time) {
    var getdateInstance = new Common_GetWeekDate(time);
    var thisWeekStartDate = getdateInstance.getWeekStartDate();
    var thisWeekEndDate = getdateInstance.getWeekEndDate();
    return thisWeekStartDate + "--" + thisWeekEndDate;
}

function getLastWeekDateRange(datetimeStr) {
    //当前时间
    var nowBase = new Date(convertStrToDate(datetimeStr));
    //当前时间往前推一周
    nowBase.setDate(nowBase.getDate() - 7);
    var lastWeekDateRangeTime = getThisWeekDateRange(nowBase.getTime());
    return lastWeekDateRangeTime;
}

//console.log(getThisWeekDateRange(new Date().getTime()));
//console.log(getLastWeekDateRange("2018-09-01"));