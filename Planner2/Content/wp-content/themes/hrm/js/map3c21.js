var map,
	places = [],
	geocoder,
	latlngBounds,
	marker,
	infowindow;

if ( typeof realtyMap !== 'undefined' )
	places = realtyMap;

function addMarker(args) {
	var marker,
		markerObj= {},
		infoWindow = null;

	markerObj.map = args.map ? args.map : map;

	if (args.desc) {
		infoWindow = new google.maps.InfoWindow({ // Create info window for this marker
			content: args.desc,
			maxWidth: 400
		});
	}

	/**
	 * Show marker
	 *
	 * @return void
	 */
	function setMarker() {
		if (args.title)
			markerObj.title = args.title;
		if (args.icon)
			markerObj.icon = args.icon;
		markerObj.animation = google.maps.Animation.DROP;
		marker = new google.maps.Marker(markerObj);

		// Show info window when hover
		if (infoWindow) {
			google.maps.event.addListener(marker, 'click', function() {
				infoWindow.open(map, marker);
			});
		}
	}

	if (args.type == 'latlng') {
		markerObj.position = args.latlng ? args.latlng : new google.maps.LatLng(args.lat, args.lng);
		setMarker();

		// Zoom map to include new marker
		latlngBounds.extend(markerObj.position);
		map.fitBounds(latlngBounds);
	} else if (args.type == 'address') {
		geocoder.geocode({'address': args.address}, function(results, status) {
			if (status == google.maps.GeocoderStatus.OK) {
				markerObj.position = results[0].geometry.location;
				setMarker();

				// Zoom map to include new marker
				latlngBounds.extend(markerObj.position);
				map.fitBounds(latlngBounds);
			}
		});
	}
}

function setInfoWindow(latlng) {
    geocoder.geocode({'latLng': latlng}, function(results, status) {
      	if (status == google.maps.GeocoderStatus.OK) {
        	if (results[1]) {
				infoWindow.setContent(results[1].formatted_address);
				infoWindow.open(map, marker);
				marker.setPosition(latlng);
				// map.setCenter(latlng);
        	}
      	}
    });
}

function getLocation( address )
{
	if ( address == '' )
		return false;

	var latLng = false;
}

function onAddressChange(e)
{
	if ( e.value == '' )
		return false;

	geocoder.geocode( { 'address': e.value }, function ( results, status ) {
		if ( status == google.maps.GeocoderStatus.OK ) {
			document.getElementById("location-lat").value = results[0].geometry.location.lat();
			document.getElementById("location-lng").value = results[0].geometry.location.lng();

			marker.setPosition(results[0].geometry.location);
			map.setCenter(results[0].geometry.location);
			setInfoWindow(results[0].geometry.location);
		}
	} );
}

function initMap() {
	var mapOptions = {
			zoom: 5,
			mapTypeId: google.maps.MapTypeId.ROADMAP
		},
		i = places.length,
		el = document.getElementById('canvas-map');
	latlngBounds = new google.maps.LatLngBounds();
	geocoder = new google.maps.Geocoder();

	if ( i <= 0 ) {
		var latlngt = new google.maps.LatLng( 21.03331734643977, 105.85009574890137 );
		
		mapOptions.center = latlngt;
		mapOptions.zoom = 15;
	}

	map = new google.maps.Map(el, mapOptions);

	if ( i <= 0 )
	{
		var address = document.getElementById( 'realty-address' ).value,
			lat = document.getElementById( 'location-lat' ).value,
			lng = document.getElementById( 'location-lng' ).value,
			location;

		if ( ! lat || ! lng )
		{
			document.getElementById( 'location-lat' ).value = 21.03331734643977;
			document.getElementById( 'location-lng' ).value = 105.85009574890137;

			location = mapOptions.center;
		}
		else
		{
			location = new google.maps.LatLng( lat, lng );
		}

		marker = new google.maps.Marker({
			location: location,
			draggable: true,
			map: map
		});

		// Create info window for this marker
		infoWindow = new google.maps.InfoWindow({
			content: address
		});

		setInfoWindow( location );
		map.setCenter( location );

		google.maps.event.addListener(map, 'click', function(event) {
			marker.setPosition(event.latLng);
			map.setCenter(event.latLng);
			document.getElementById("location-lat").value = event.latLng.lat();
			document.getElementById("location-lng").value = event.latLng.lng();

			setInfoWindow( event.latLng );
		});

		google.maps.event.addListener(marker, 'dragend', function(event){
			document.getElementById("location-lat").value = event.latLng.lat();
			document.getElementById("location-lng").value = event.latLng.lng();

			setInfoWindow( event.latLng );
		});

		document.getElementById( 'realty-address' ).onchange = function() { onAddressChange( this ); };

		return false;
	}

	if (places[0].type == 'latlng') {
		mapOptions.center = new google.maps.LatLng(places[0].lat, places[0].lng);

		for (; i--;) {
			addMarker(places[i]);
		}
	} else {
		geocoder.geocode({'address': places[0].address}, function(results, status) {
			if (status == google.maps.GeocoderStatus.OK) {
				mapOptions.center = results[0].geometry.location;

				for (; i--;) {
					addMarker(places[i]);
				}
			}
		});
	}
}
window.onload = initMap;