function get(name){
   if(name=(new RegExp('[?&]'+encodeURIComponent(name)+'=([^&]*)')).exec(location.search))
      return decodeURIComponent(name[1]);
}
var currentProvince;
var currentDistrict;

// function get_provider_js(){

//     jQuery.ajax({
// 		url: get_district_ajax.ajaxurl,
// 		data: ({action : 'get_provinder'}),
// 		method: 'post',
// 		type: 'json',
// 		beforeSend: function(){
// 			// $('body').prepend('<div class="preload" style="background:#E74C3C; font-size:20px; color:#fff; padding:10px; position:fixed; top:50%; left:50%;">Loading...</div>');
// 		},
// 		success: function(res){
// 			$('.preload').remove();
// 			var jsonObject = JSON.parse(res);

// 			var jsonData = jsonObject.provinces;
// 			var tinh_selected = jsonObject.province_selected;
// 			var option_html = '';
// 			option_html += '<option value="">Chá»n Tá»‰nh/TP</option>';
// 			for (var i = 0; i < jsonData.length; i++) {
// 				var provinder = jsonData[i];
// 				var selected = '';
// 				if(tinh_selected == provinder.term_id)
// 					selected = "selected";
// 				option_html += '<option value="'+ provinder.term_id +'" '+selected+'>'+ provinder.name +'</option>';
// 			}

// 			jQuery('.tinh-tp-select .wpuf-fields select, #tinhthanhpho').html(option_html);
// 			jQuery('.form-group .tinh-tp').html(option_html);
// 			// get_district_js(jsonData[0].term_id);
// 		},
// 	});
// };


function get_district_js(provinder, current_district = 0){
	var option_html = '<option value="">-- Chọn Quận/Huyện  --</option>';
	jQuery('.quick-district').html(option_html);

	var option_html = '<option value="">-- Phường/Xã --</option>';
	jQuery('.quick-ward').html(option_html);

	var dataCities1 = JSON.parse(JSON.stringify(citiescc));
	var found = false;
	for(var i = 0; i < dataCities1.length; i++){
		if(dataCities1[i].id == provinder){
			found = true;
			var districts = dataCities1[i].districts;
			currentProvince = dataCities1[i];
			currentDistrict = districts;
			var option_html = '';
			var res_districts = [];
			option_html += '<option value="">-- Chọn Quận/Huyện --</option>';
			for (var j = 0; j < districts.length; j++) {
				var district = districts[j];
				option_html += '<option value="'+ district.id +'">'+ district.name +'</option>';
				
				var _res_district = {'id':district.id, 'text':district.name};
				res_districts.push(_res_district);
				
			}

			jQuery('.quick-district').empty();
			jQuery('.quick-district').html(option_html);
			jQuery('.quick-ward').empty();
			jQuery('.quick-ward').html('<option value="">-- Chọn Phường/Xã --</option>');
			// jQuery(".quick-ward").select2({
			// 	'data': [{id:"0", text:"Phường/Xã"}],
			// });
			return;
		}
	}

};
function get_district__project_js(provinder, current_district = 0){
	var option_html = '<option value="">-- Chọn Quận/Huyện  --</option>';
	jQuery('.quick-district').html(option_html);

	var option_html = '<option value="">-- Phường/Xã --</option>';
	jQuery('.quick-ward').html(option_html);

	var dataCities1 = JSON.parse(JSON.stringify(citiesccc));
	var found = false;
	for(var i = 0; i < dataCities1.length; i++){
		if(dataCities1[i].id == provinder){
			found = true;
			var districts = dataCities1[i].districts;
			currentProvince = dataCities1[i];
			currentDistrict = districts;
			var option_html = '';
			var res_districts = [];
			option_html += '<option value="">-- Chọn Quận/Huyện --</option>';
			for (var j = 0; j < districts.length; j++) {
				var district = districts[j];
				option_html += '<option value="'+ district.id +'">'+ district.name +'</option>';
				
				var _res_district = {'id':district.id, 'text':district.name};
				res_districts.push(_res_district);
				
			}

			jQuery('.quick-districtp').empty();
			jQuery('.quick-districtp').html(option_html);
			jQuery('.quick-wardp').empty();
			jQuery('.quick-wardp').html('<option value="">-- Chọn Phường/Xã --</option>');
			// jQuery(".quick-ward").select2({
			// 	'data': [{id:"0", text:"Phường/Xã"}],
			// });
			return;
		}
	}

};

function get_wards_js(district){

	var found = false;
	if(typeof currentProvince !== "undefined"){
		var districts = currentProvince.districts;
		for(var i = 0; i<districts.length; i++){

			if(districts[i].id == district){
				found = true;
				var streets = districts[i].ward;
				var option_html = '';
				option_html += '<option value="">-- Chọn Phường/Xã --</option>';
				for (var j = 0; j < streets.length; j++) {
					var ward = streets[j];
					option_html += '<option value="'+ ward.id +'">'+ ward.name +'</option>';
				}
				jQuery('.quick-ward').empty();
				jQuery('.quick-ward').html(option_html);
				return;
			}
		}
	}
};
function get_wards_project_js(districtp){

	var found = false;
	if(typeof currentProvince !== "undefined"){
		var districts = currentProvince.districts;
		for(var i = 0; i<districts.length; i++){

			if(districts[i].id == district){
				found = true;
				var streets = districts[i].ward;
				var option_html = '';
				option_html += '<option value="">-- Chọn Phường/Xã --</option>';
				for (var j = 0; j < streets.length; j++) {
					var ward = streets[j];
					option_html += '<option value="'+ ward.id +'">'+ ward.name +'</option>';
				}
				jQuery('.quick-wardp').empty();
				jQuery('.quick-wardp').html(option_html);
				return;
			}
		}
	}
};
// function list_city(limit){
// 	var dataCities = JSON.parse(JSON.stringify(citiescc));
// 	var option_html = '';
// 	for(var i = 0; i < dataCities.length; i++){
// 		var city = dataCities[i];
// 		var rand_hide = '';
// 		if(i>=limit) {
// 			var rand_hide = ' not-rand hide';
// 		}
// 		if(city.districts != '') {
// 			option_html += '<li class="cat-item cat-item-'+city.id +' '+rand_hide+'"><a href="'+search_url+'?city='+city.id+'">'+city.name+'</a>';
// 			var districts = city.districts;
// 			var districts_html = '<ul class="sub-menu">';
// 			for (var j = 0; j<districts.length; j++) {
// 				districts_html += '<li ><a href="'+search_url+'?district='+districts.id+'">'+districts.name+'</a>';
// 			}
// 			districts_html += '</ul>';
// 			option_html += districts_html + '</li>';
// 		} else {
// 			option_html += '<li class="cat-item cat-item-'+city.id +' '+rand_hide+'"><a href="'+search_url+'?city='+city.id+'">'+city.name+'</a></li>';
// 		}
// 	}
// 	jQuery('#city-district').html(option_html);
// };
function change_map(city,district,ward) {
	var dataCities1 = JSON.parse(JSON.stringify(citiescc));
	var name_city = '';
	var name_district = '';
	var name_ward = '';
	for(var i = 0; i < dataCities1.length; i++){
		if(dataCities1[i].id == city){
			var name_city = dataCities1[i].name;
			var districts = dataCities1[i].districts;
			for(var j = 0; j < districts.length; j++){
				if(districts[j].id == district){
					var name_district = districts[j].name;
					var wards = districts[j].ward;
					for(var z = 0; z < wards.length; z++){
						var name_ward = wards[z].name;
					}
				}
			}
		}
	}
	var text_map = name_ward + ' ' + name_district + ' ' + name_city;
	jQuery('#realty-address').val(text_map);
	if ( text_map == '' )
		return false;

	geocoder.geocode( { 'address': text_map }, function ( results, status ) {
		if ( status == google.maps.GeocoderStatus.OK ) {
			document.getElementById("location-lat").value = results[0].geometry.location.lat();
			document.getElementById("location-lng").value = results[0].geometry.location.lng();

			marker.setPosition(results[0].geometry.location);
			map.setCenter(results[0].geometry.location);
			setInfoWindow(results[0].geometry.location);
		}
	} );
}	