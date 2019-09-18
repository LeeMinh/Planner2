jQuery(document).ready(function($){
	// menu responsive
	jQuery("document").ready(function() {
       jQuery('.menu-open i').click( function () {
           jQuery('.menu-responsive').animate({left: '0px'}, 200);
       });
       jQuery('.menu-close').click( function () {
           jQuery('.menu-responsive').animate({left: '-280px'}, 200);
       });
   });

    setTimeout(function(){
        var highestBox = 0;
        $('#primary .list-realty li').each(function(){
            if($(this).height() > highestBox) {
                highestBox = $(this).height(); 
            }
        });  
        $('#primary .list-realty li').height(highestBox);

// select2 default
// $("#quick-city-sell, .option-select #city").val('1566').trigger('change');

    }, 1000);
/*---------------------------------------------------
/*  Vertical menus toggles
/* -------------------------------------------------*/

    $('#menu-main-menu').addClass('toggle-menu');
    $('.toggle-menu ul.sub-menu, .toggle-menu ul.children').addClass('toggle-submenu');
    $('.toggle-menu ul.sub-menu').parent().addClass('toggle-menu-item-parent');

    $('.toggle-menu .toggle-menu-item-parent').append('<span class="toggle-caret"><i class="fa fa-plus"></i></span>');

    $('.toggle-caret').click(function(e) {
        e.preventDefault();
        $(this).parent().toggleClass('active').children('.toggle-submenu').slideToggle('fast');
    });

    $('.cat-item ul.sub-menu').parent().addClass('toggle-menu-item-parent');
    $('.closed > .toggle-caretc').click(function(e) {
        $(this).parent('.closed').children('ul.sub-menu').slideToggle('fast');
    });
    jQuery( window ).scroll( function () {
        if ( jQuery( this ).scrollTop() > 100 ) {
            jQuery( '#topcontrol').css( { bottom: "45px" } );
        } else {
            jQuery( '#topcontrol' ).css( { bottom: "-100px" } );
        }
    } );

    jQuery( '#topcontrol' ).click( function() {
        jQuery( 'html, body' ).animate( { scrollTop: '0px' }, 800 );
        return false;
    } );
    jQuery(".quick-search .quick-city,.quick-search .quick-district").change(function(){
      jQuery('.quick-search-sl').submit();
  });
    $(".quick-city, .quick-city-edit").select2().on('change', function(){
        get_district_js($(this).val());
    });
    $(".quick-district").select2().on('change', function(){
        get_wards_js($(this).val());
    });
    $(".quick-cityp, .quick-city-editp").select2().on('change', function(){
        get_district__project_js($(this).val());
    });
    $(".quick-districtp").select2().on('change', function(){
        get_wards_project_js($(this).val());
    });
    $(".quick-wardp").select2();
    $("img.lazy").lazyload({
        effect : "fadeIn",
    });
    $("#proj_slide").owlCarousel({
        loop:true,
        autoplay:true,
        autoplayTimeout:5000,
        dots:false,
        nav:false,
        items: 1,
        lazyLoad:true,
        navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
    });
    $(".slide-noi-bat").owlCarousel({
        loop:false,
        autoplay:true,
        autoplayTimeout:3000,
        dots:false,
        nav:true,
        items: 1,
        navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        
    });
    $(".slide-cuc-hot").owlCarousel({
        loop:false,
        autoplay:true,
        autoplayTimeout:3000,
        dots:false,
        nav:true,
        items: 2,
        margin:10,
        navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        
    });
    var sinc1 = $("#sync1");
    var sinc2 = $("#sync2");
    var slidesPerPage = 5; //globaly define number of elements per page
    var syncedSecondary = true;

    sinc1.owlCarousel({
        items: 1,
        nav: true,
        autoHeight:true,       
        slideSpeed : 2000,
        dots: false,
        autoplay:true,
        autoplayTimeout:3000,
        navText: ['<i class="fa fa-chevron-left" aria-hidden="true"></i>','<i class="fa fa-chevron-right" aria-hidden="true"></i>'],
        loop: true,
        responsiveRefreshRate : 200,
    }).on('changed.owl.carousel', syncPosition);

    sinc2
    .on('initialized.owl.carousel', function () {
        sinc2.find(".owl-item").eq(0).addClass("current");
    })
    .owlCarousel({
        margin: 0,
        nav: false,
        dots: false,
        items: 6,
        responsive : {
            0 : {
                items: 4,
            },
                // breakpoint from 480 up
                480 : {
                    items: 4,
                    margin: 0,
                },
                // breakpoint from 768 up
                768 : {
                    items: 4,
                    margin: 0,
                },
                1024 : {
                    items: 6,
                    margin: 0,
                }
            },
            items : slidesPerPage,
            slideBy: slidesPerPage,
            responsiveRefreshRate : 100
        }).on('changed.owl.carousel', syncPosition2);

    function syncPosition(el) {
        var count = el.item.count-1;
        var current = Math.round(el.item.index - (el.item.count/2) - .5);

        if(current < 0) {
            current = count;
        }
        if(current > count) {
            current = 0;
        }

        //end block

        sinc2
        .find(".owl-item")
        .removeClass("current")
        .eq(current)
        .addClass("current");
        var onscreen = sinc2.find('.owl-item.active').length - 1;
        var start = sinc2.find('.owl-item.active').first().index();
        var end = sinc2.find('.owl-item.active').last().index();

        if (current > end) {
            sinc2.data('owl.carousel').to(current, 100, true);
        }
        if (current < start) {
            sinc2.data('owl.carousel').to(current - onscreen, 100, true);
        }
    }

    function syncPosition2(el) {
        if(syncedSecondary) {
            var number = el.item.index;
            sinc1.data('owl.carousel').to(number, 100, true);
        }
    }

    sinc2.on("click", ".owl-item", function(e){
        e.preventDefault();
        var number = $(this).index();
        sinc1.data('owl.carousel').to(number, 300, true);
    });
});
function ShowDistrictLink(showid) {
  if (showid == 1) {
      jQuery(".not-rand").removeClass("hide");
      jQuery("#dshow-all").addClass("hide");
  } else {
      jQuery(".not-rand").addClass("hide");
      jQuery("#dshow-all").removeClass("hide");
  }
}
jQuery('.marquee-with-options').marquee({
  speed: 30,
  delayBeforeStart: 0,
  direction: 'left',
  duplicated: true,
  pauseOnHover: true
});
function ShowBSAdvance() {
    if( jQuery("#bs-advance").is(".hidecc") ) {
        jQuery("#hplAdvance").text("Tìm kiếm nâng cao");
        jQuery("#bs-advance").removeClass('hidecc').addClass('showcc');
    } else {
        jQuery("#hplAdvance").text("Bỏ tìm kiếm nâng cao");
        jQuery("#bs-advance").removeClass('showcc').addClass('hidecc');

    }  
    jQuery("#bs-advance").toggle("slow")
}
jQuery(document).ready(function($){
    setTimeout(function(){
      jQuery('#maps').removeClass("active");
  }, 500);

    $('#type-realty').change(function(){
        var $type = $(this).val();
        $("#term-type").empty();
        $.ajax({
            url: ajax_url,
            type:'POST',
            data:'action=type_term&type=' + $type,
            success:function(results)  {
                $("#term-type").append(results);
            }
        });
    });
    $('#city, #district, #ward').on('change', function() {
        var city = $('#city').val();
        var district = $('#district').val();
        var ward = $('#ward').val();
        change_map(city,district,ward);
    });
})
function xoaanh(id) {
    post_id = jQuery("#current"+id).val();
    jQuery.ajax({
        url: ajax_url,
        type:'POST',
        data:'action=xoaimage&id=' + post_id,
        success:function(results){
            jQuery(".fileUpload"+id).css('display', 'none');
            jQuery("#upload-file"+id).val('');
            jQuery("#thumbimage"+id).attr('src', style_url + '/images/placeholder.png');
        }
    });

}
function duyetbai(id) {
    var item = jQuery('#check-' + id);
    name = item.attr('data-admin');
    link = item.attr('data-link');
    jQuery.ajax({
        url: ajax_url,
        type:'POST',
        data:'action=duyetbai&id=' + id + '&name='+name,
        success:function(results){
            window.location.href = link;
        }
    });
}