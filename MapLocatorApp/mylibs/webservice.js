var webServiceURL = 'http://204.45.55.12/CompassWS/CompassWS_V1.asmx';
var locGroupFilterPlaceHolder = "<!--locgroupfilterplaceholder-->";

var GLOBAL_SimpleLocByBoundsSoapMessage =
            '<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">' +
            '	<soapenv:Header/>' +
            '	<soapenv:Body>' +
            '		<tem:SimpleLocationSpatialSearchByBounds>' +
            '			<tem:southWestLatitude>{southWestLatitude}</tem:southWestLatitude>' +
            '			<tem:southWestLongitude>{southWestLongitude}</tem:southWestLongitude>' +
            '			<tem:northEastLatitude>{northEastLatitude}</tem:northEastLatitude>' +
            '			<tem:northEastLongitude>{northEastLongitude}</tem:northEastLongitude>' +
            '			<!--Optional:-->' +
            '			<tem:optionalLocationGroupFilter>' +
            '				<!--locgroupfilterplaceholder-->' +
            '			</tem:optionalLocationGroupFilter>' +
            '		</tem:SimpleLocationSpatialSearchByBounds>' +
            '	</soapenv:Body>' +
            '</soapenv:Envelope>';

var GLOBAL_CrimeTypesCountSoapMessage =
            '<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">' +
            '	<soapenv:Header/>' +
            '	<soapenv:Body>' +
            '		<tem:SimpleLocationSummaryByBounds>' +
            '			<tem:southWestLatitude>{southWestLatitude}</tem:southWestLatitude>' +
            '			<tem:southWestLongitude>{southWestLongitude}</tem:southWestLongitude>' +
            '			<tem:northEastLatitude>{northEastLatitude}</tem:northEastLatitude>' +
            '			<tem:northEastLongitude>{northEastLongitude}</tem:northEastLongitude>' +
            '			<tem:locationGroupFilter>{locationGroupFilter}</tem:locationGroupFilter>' + 
            '		</tem:SimpleLocationSummaryByBounds>' +
            '	</soapenv:Body>' +
            '</soapenv:Envelope>';

//var m_markers;

// locationGroupFilterIntList = int array
function CallService(successCallBack, onErrorCallBack, 
        southWestLatitude,
        southWestLongitude,
        northEastLatitude, 
        northEastLongitude,
        locationGroupFilterIntList,
        soapMessage) {

    // check which is the soapMessage to process optionalLocationGroupFilter or locationGroupFilter
    if (soapMessage == GLOBAL_SimpleLocByBoundsSoapMessage) {
        soapMessage = soapMessage.replace(locGroupFilterPlaceHolder,
                                GetGroupFilterSoapSection(locationGroupFilterIntList));
    }
    // in this call, the locationGroupFilter(locationGroupFilterIntList) is required.
    else if (soapMessage == GLOBAL_CrimeTypesCountSoapMessage) {        
        if (locationGroupFilterIntList == null || locationGroupFilterIntList.length == 0) {
            alert("ErrWS54: Location Group Filter parameter missing.");
            return;
        }

        soapMessage = soapMessage.replace("{locationGroupFilter}", locationGroupFilterIntList[0]);
    }

    //var soapMessage = simpleLocByBoundsSoapMessage.replace("{southWestLatitude}", southWestLatitude);
    soapMessage = soapMessage.replace("{southWestLatitude}", southWestLatitude);
    soapMessage = soapMessage.replace("{southWestLongitude}", southWestLongitude);
    soapMessage = soapMessage.replace("{northEastLatitude}", northEastLatitude);
    soapMessage = soapMessage.replace("{northEastLongitude}", northEastLongitude);

    $.ajax({
        url: webServiceURL,
        type: "POST",
        dataType: "xml",
        data: soapMessage,
        processData: false,
        contentType: "text/xml; charset=\"utf-8\"",
        success: successCallBack,
        error: onErrorCallBack
    });

    return false;
}

function GetGroupFilterSoapSection(locationGroupFilterIntList) {
    var result = locGroupFilterPlaceHolder;

    if (locationGroupFilterIntList != null) {
        result = "";
        for (var i = 0; i < locationGroupFilterIntList.length; i++) {
            result += "<tem:int>" + locationGroupFilterIntList[i] + "</tem:int>";
        }
    }

    return result;
}
