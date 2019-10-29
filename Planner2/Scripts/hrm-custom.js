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

//    setTimeout(function(){
//        var highestBox = 0;
//        $('#primary .list-realty li').each(function(){
//            if($(this).height() > highestBox) {
//                highestBox = $(this).height(); 
//            }
//        });  
//        $('#primary .list-realty li').height(highestBox);

//// select2 default
//// $("#quick-city-sell, .option-select #city").val('1566').trigger('change');

//    }, 1000);
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

   
})
 